namespace AncinInsaat.Services;

// Bound from the "Smtp" configuration section. Never set in appsettings*.json
// or source (Claude.md secrets rule) — on the server every value comes from
// environment variables (Smtp__Host, Smtp__Port, ...). An empty Host means
// email notifications are disabled and forms only save to the database.
public class SmtpOptions
{
    public const string SectionName = "Smtp";

    public string? Host { get; set; }
    public int Port { get; set; } = 587;
    public bool UseStartTls { get; set; } = true;

    // Optional — Google Workspace SMTP relay authenticates by server IP.
    public string? Username { get; set; }
    public string? Password { get; set; }

    // Either "web@ancin.com.tr" or "Display Name <web@ancin.com.tr>".
    public string? From { get; set; }
    public string? To { get; set; }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(Host);
}
