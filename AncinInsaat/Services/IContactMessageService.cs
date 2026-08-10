using AncinInsaat.Data.Entities;

namespace AncinInsaat.Services;

// The Contact Form's write path — the first one in the app (every other
// service so far is read-only, see IProjectQueryService/ISiteSettingsService).
// Kept as its own service rather than the controller touching AppDbContext
// directly, matching "business logic belongs to services" (Claude.md Coding
// Rules) even though the logic here is currently a single insert.
public interface IContactMessageService
{
    Task SaveAsync(ContactMessage message, CancellationToken cancellationToken = default);
}
