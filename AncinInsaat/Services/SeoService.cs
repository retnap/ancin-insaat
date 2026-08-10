using AncinInsaat.Data;
using AncinInsaat.Models;
using Microsoft.EntityFrameworkCore;

namespace AncinInsaat.Services;

public class SeoService : ISeoService
{
    // Used only when a page has no SeoMetadata row yet (e.g. a new page
    // wired before its copy is seeded) — keeps the site indexable with
    // generic-but-safe tags instead of a missing-metadata crash.
    private const string DefaultTitle = "Ancın İnşaat";
    private const string DefaultDescription = "Ancın İnşaat — Aydın merkezli, güven ve zanaatkârlıkla şekillenen konut projeleri.";
    private const string DefaultOgImage = "/images/seo/og-home.webp";

    private readonly AppDbContext _context;

    public SeoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SeoModel> GetPageSeoAsync(string pageKey, HttpRequest request, CancellationToken cancellationToken = default)
    {
        var metadata = await _context.SeoMetadata
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Page == pageKey, cancellationToken);

        var baseUrl = $"{request.Scheme}://{request.Host}";
        var canonicalPath = metadata is not null && !string.IsNullOrWhiteSpace(metadata.CanonicalUrl)
            ? metadata.CanonicalUrl
            : "/";
        var ogImagePath = metadata is not null && !string.IsNullOrWhiteSpace(metadata.OpenGraphImage)
            ? metadata.OpenGraphImage
            : DefaultOgImage;

        return new SeoModel
        {
            Title = metadata is not null && !string.IsNullOrWhiteSpace(metadata.MetaTitle) ? metadata.MetaTitle : DefaultTitle,
            Description = metadata is not null && !string.IsNullOrWhiteSpace(metadata.MetaDescription) ? metadata.MetaDescription : DefaultDescription,
            CanonicalUrl = CombineUrl(baseUrl, canonicalPath),
            OgImageUrl = CombineUrl(baseUrl, ogImagePath)
        };
    }

    private static string CombineUrl(string baseUrl, string path)
    {
        return path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            ? path
            : $"{baseUrl}/{path.TrimStart('/')}";
    }
}
