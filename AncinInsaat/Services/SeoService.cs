using System.Globalization;
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
    private const string DefaultTitleEn = "Ancın İnşaat";
    private const string DefaultDescriptionEn = "Ancın İnşaat — a premium residential and construction developer based in Aydın, Türkiye.";
    private const string DefaultOgImage = "/images/seo/og-home.webp";

    private readonly AppDbContext _context;
    private readonly IDisplayImageResolver _displayImageResolver;

    public SeoService(AppDbContext context, IDisplayImageResolver displayImageResolver)
    {
        _context = context;
        _displayImageResolver = displayImageResolver;
    }

    public async Task<SeoModel> GetPageSeoAsync(string pageKey, HttpRequest request, string? ogImageFallbackPath = null, CancellationToken cancellationToken = default)
    {
        var metadata = await _context.SeoMetadata
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Page == pageKey, cancellationToken);

        // English localization (2026-10-02) — one SeoMetadata row per page
        // still covers both languages (MetaTitleEn/MetaDescriptionEn are
        // nullable companions, not a second row/page key), and the
        // canonical URL always reflects whichever URL this exact request
        // actually hit (its own "/en" prefix or lack of one) rather than
        // always pointing at the Turkish page.
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

        var baseUrl = SiteUrls.CanonicalBaseUrl;
        var turkishCanonicalPath = metadata is not null && !string.IsNullOrWhiteSpace(metadata.CanonicalUrl)
            ? metadata.CanonicalUrl
            : "/";
        var canonicalPath = isEnglish
            ? LanguageUrlService.GetEnglishEquivalent(turkishCanonicalPath, QueryString.Empty)
            : turkishCanonicalPath;
        var ogImagePath = metadata is not null && !string.IsNullOrWhiteSpace(metadata.OpenGraphImage)
            ? metadata.OpenGraphImage
            : DefaultOgImage;

        // Never advertise a missing file as og:image (2026-10-28) — several
        // seeded project rows still point at a removed cover.webp.
        if (!_displayImageResolver.Exists(ogImagePath))
        {
            ogImagePath = ogImageFallbackPath is not null && _displayImageResolver.Exists(ogImageFallbackPath)
                ? ogImageFallbackPath
                : DefaultOgImage;
        }

        var title = isEnglish
            ? FirstNonEmpty(metadata?.MetaTitleEn, metadata?.MetaTitle, DefaultTitleEn)
            : FirstNonEmpty(metadata?.MetaTitle, DefaultTitle);
        var description = isEnglish
            ? FirstNonEmpty(metadata?.MetaDescriptionEn, DefaultDescriptionEn)
            : FirstNonEmpty(metadata?.MetaDescription, DefaultDescription);

        return new SeoModel
        {
            Title = title,
            Description = description,
            CanonicalUrl = CombineUrl(baseUrl, canonicalPath),
            OgImageUrl = CombineUrl(baseUrl, ogImagePath)
        };
    }

    private static string FirstNonEmpty(params string?[] candidates) =>
        candidates.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c)) ?? string.Empty;

    private static string CombineUrl(string baseUrl, string path)
    {
        return path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            ? path
            : $"{baseUrl}/{path.TrimStart('/')}";
    }
}
