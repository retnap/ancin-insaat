using AncinInsaat.Data.Entities;

namespace AncinInsaat.Services;

// Notifies the company inbox (Smtp:To) about a form submission that has
// already been saved. Implementations never throw: a failed or disabled
// send is logged only, so a visitor's submission is never lost or shown
// as failed because of email.
public interface IEmailNotificationService
{
    Task NotifyContactMessageAsync(ContactMessage message, CancellationToken cancellationToken = default);

    Task NotifyJobApplicationAsync(JobApplicationSubmission application, CancellationToken cancellationToken = default);
}
