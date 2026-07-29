namespace AncinInsaat.Models;

// Per-page SEO metadata, resolved by ISeoService and handed to _Layout via
// ViewData["Seo"] — mirrors the existing ViewData["Title"] convention
// (Views/Home/Index.cshtml) rather than introducing a parallel mechanism.
// All URLs are absolute (scheme + host already applied) since Open Graph
// and canonical tags are meaningless as relative paths.
public class SeoModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string CanonicalUrl { get; init; }
    public required string OgImageUrl { get; init; }
}
