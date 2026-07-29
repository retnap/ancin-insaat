using AncinInsaat.Models;

namespace AncinInsaat.Services;

// Resolves per-page SEO metadata (title, description, canonical, OG image)
// from the SeoMetadata table, keyed by a page identifier every controller
// action defines for itself (e.g. "home"). One lookup path shared by every
// page, so future pages integrate by seeding a row and calling this same
// service rather than each hand-rolling its own meta tags.
public interface ISeoService
{
    Task<SeoModel> GetPageSeoAsync(string pageKey, HttpRequest request, CancellationToken cancellationToken = default);
}
