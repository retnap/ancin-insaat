using AncinInsaat.Data.Entities;

namespace AncinInsaat.Services;

// Read-only accessor for the single SiteSettings row (pending a future
// Admin Panel). FooterViewComponent reads its Email from here; its
// Address/Phone are still hardcoded.
// This is the first real consumer of the table: Organization/WebSite
// structured data needs one source of truth for company identity rather
// than a second hardcoded copy alongside Footer's.
public interface ISiteSettingsService
{
    Task<SiteSettings?> GetAsync(CancellationToken cancellationToken = default);
}
