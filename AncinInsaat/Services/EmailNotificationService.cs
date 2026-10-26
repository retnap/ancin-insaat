using System.Globalization;
using System.Text;
using AncinInsaat.Data.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AncinInsaat.Services;

// MailKit-based IEmailNotificationService. Emails are always Turkish (they
// go to the company inbox, whatever language the visitor used) and plain
// text, so no visitor input is ever rendered as HTML. Reply-To is the
// visitor's address so the team can answer straight from the inbox.
public class EmailNotificationService : IEmailNotificationService
{
    private const string DefaultFromDisplayName = "Ancın İnşaat Web Sitesi";
    private const int SmtpTimeoutMilliseconds = 15_000;

    private static readonly CultureInfo Turkish = new("tr-TR");

    private readonly SmtpOptions _options;
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(IOptions<SmtpOptions> options, ILogger<EmailNotificationService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task NotifyContactMessageAsync(ContactMessage message, CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured)
        {
            _logger.LogInformation("SMTP is not configured — no email sent for contact message {ContactMessageId}.", message.Id);
            return;
        }

        try
        {
            var email = CreateMessage(
                $"Web Sitesi İletişim Formu: {SanitizeHeader(message.FullName)}",
                message.FullName,
                message.Email);

            var body = new StringBuilder()
                .AppendLine("Web sitesindeki iletişim formundan yeni bir mesaj gönderildi.")
                .AppendLine()
                .AppendLine($"Ad Soyad : {SanitizeHeader(message.FullName)}")
                .AppendLine($"E-posta  : {SanitizeHeader(message.Email)}")
                .AppendLine($"Telefon  : {SingleLine(message.Phone)}")
                .AppendLine($"Konu     : {SingleLine(message.Subject)}")
                .AppendLine($"Tarih    : {FormatTurkeyTime(message.CreatedAt)}")
                .AppendLine()
                .AppendLine("Mesaj:")
                .AppendLine(message.Message);

            email.Body = new TextPart("plain") { Text = body.ToString() };

            await SendAsync(email, cancellationToken);
            _logger.LogInformation("Notification email sent for contact message {ContactMessageId}.", message.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Notification email failed for contact message {ContactMessageId}; the message is saved in the database.", message.Id);
        }
    }

    public async Task NotifyJobApplicationAsync(JobApplicationSubmission application, CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured)
        {
            _logger.LogInformation("SMTP is not configured — no email sent for the job application.");
            return;
        }

        try
        {
            var email = CreateMessage(
                $"Web Sitesi İş Başvurusu: {SanitizeHeader(application.FullName)} – {SanitizeHeader(application.Position)}",
                application.FullName,
                application.Email);

            var body = new StringBuilder()
                .AppendLine("Web sitesindeki kariyer formundan yeni bir iş başvurusu gönderildi.")
                .AppendLine()
                .AppendLine($"Pozisyon : {SanitizeHeader(application.Position)}")
                .AppendLine($"Ad Soyad : {SanitizeHeader(application.FullName)}")
                .AppendLine($"E-posta  : {SanitizeHeader(application.Email)}")
                .AppendLine($"Telefon  : {SingleLine(application.Phone)}")
                .AppendLine($"Tarih    : {FormatTurkeyTime(DateTime.UtcNow)}")
                .AppendLine()
                .AppendLine("Mesaj:")
                .AppendLine(ValueOrDash(application.Message))
                .AppendLine()
                .AppendLine("CV ekte (PDF).");

            // The CV was already validated and stored by JobApplicationService;
            // IFormFile.OpenReadStream starts a fresh read from the beginning.
            var cv = new MemoryStream();
            await using (var source = application.Cv.OpenReadStream())
            {
                await source.CopyToAsync(cv, cancellationToken);
            }
            cv.Position = 0;

            var builder = new BodyBuilder { TextBody = body.ToString() };
            builder.Attachments.Add(
                $"CV - {SanitizeFileName(application.FullName)}.pdf",
                cv,
                new ContentType("application", "pdf"));

            email.Body = builder.ToMessageBody();

            await SendAsync(email, cancellationToken);
            _logger.LogInformation("Notification email sent for the job application.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Notification email failed for the job application; the application is saved in the database.");
        }
    }

    private MimeMessage CreateMessage(string subject, string visitorName, string visitorEmail)
    {
        if (string.IsNullOrWhiteSpace(_options.From) || string.IsNullOrWhiteSpace(_options.To))
        {
            throw new InvalidOperationException("Smtp:From and Smtp:To must both be set when Smtp:Host is set.");
        }

        var from = MailboxAddress.Parse(_options.From);
        if (string.IsNullOrWhiteSpace(from.Name))
        {
            from.Name = DefaultFromDisplayName;
        }

        var email = new MimeMessage();
        email.From.Add(from);
        email.To.AddRange(InternetAddressList.Parse(_options.To));
        email.Subject = subject;

        // The address was already validated by the form's [EmailAddress];
        // TryParse is a second guard so a malformed value just omits Reply-To.
        if (MailboxAddress.TryParse(visitorEmail, out var replyTo))
        {
            replyTo.Name = SanitizeHeader(visitorName);
            email.ReplyTo.Add(replyTo);
        }

        return email;
    }

    private async Task SendAsync(MimeMessage email, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient { Timeout = SmtpTimeoutMilliseconds };

        // EHLO with the sender's domain instead of the machine's host name —
        // Google's SMTP relay can reject an EHLO name that isn't a real domain.
        var fromDomain = email.From.Mailboxes.First().Domain;
        if (!string.IsNullOrWhiteSpace(fromDomain))
        {
            client.LocalDomain = fromDomain;
        }

        var socketOptions = _options.UseStartTls
            ? SecureSocketOptions.StartTls
            : _options.Port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None;

        await client.ConnectAsync(_options.Host!, _options.Port, socketOptions, cancellationToken);

        if (!string.IsNullOrWhiteSpace(_options.Username))
        {
            await client.AuthenticateAsync(_options.Username, _options.Password ?? string.Empty, cancellationToken);
        }

        await client.SendAsync(email, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    // Header values (subject, display names) must stay on one line: any
    // CR/LF or other control character is replaced and whitespace collapsed.
    private static string SanitizeHeader(string value)
    {
        var cleaned = new string(value.Select(c => char.IsControl(c) ? ' ' : c).ToArray());
        return string.Join(' ', cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    private static string SanitizeFileName(string value)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var cleaned = new string(SanitizeHeader(value).Where(c => !invalid.Contains(c)).ToArray()).Trim();
        return cleaned.Length == 0 ? "Basvuru" : cleaned;
    }

    private static string ValueOrDash(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "-" : value;

    // Labelled one-line body fields get the same flattening as headers so a
    // pasted line break can't make the email layout misleading.
    private static string SingleLine(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "-" : SanitizeHeader(value);

    private static string FormatTurkeyTime(DateTime utc)
    {
        try
        {
            var turkey = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
            return TimeZoneInfo.ConvertTimeFromUtc(utc, turkey).ToString("dd.MM.yyyy HH:mm", Turkish);
        }
        catch (TimeZoneNotFoundException)
        {
            return utc.ToString("dd.MM.yyyy HH:mm", Turkish) + " (UTC)";
        }
    }
}
