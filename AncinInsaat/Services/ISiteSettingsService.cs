using AncinInsaat.Data.Entities;

namespace AncinInsaat.Services;

// Read-only accessor for the single SiteSettings row — the same data
// FooterViewComponent currently hardcodes (pending a future Admin Panel).
// This is the first real consumer of the table: Organization/WebSite
// structured data needs one source of truth for company identity rather
// than a second hardcoded copy alongside Footer's.
public interface ISiteSettingsService
{
    Task<SiteSettings?> GetAsync(CancellationToken cancellationToken = default);
}
