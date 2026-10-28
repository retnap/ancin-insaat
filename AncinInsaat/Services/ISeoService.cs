using AncinInsaat.Models;

namespace AncinInsaat.Services;

// Resolves per-page SEO metadata (title, description, canonical, OG image)
// from the SeoMetadata table, keyed by a page identifier every controller
// action defines for itself (e.g. "home"). One lookup path shared by every
// page, so future pages integrate by seeding a row and calling this same
// service rather than each hand-rolling its own meta tags.
public interface ISeoService
{
    // ogImageFallbackPath (2026-10-28): used when the row's OpenGraphImage
    // file does not exist on disk — e.g. Project Detail passes its resolved
    // hero banner, since most projects' seeded cover.webp was removed.
    Task<SeoModel> GetPageSeoAsync(string pageKey, HttpRequest request, string? ogImageFallbackPath = null, CancellationToken cancellationToken = default);
}
