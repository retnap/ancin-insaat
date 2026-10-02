using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace AncinInsaat.Services;

/// <summary>
/// Derives the request culture purely from the URL's leading "/en" segment
/// (English localization, 2026-10-02) — no cookie, no session, no
/// Accept-Language negotiation. The URL is the single source of truth for
/// language: visiting a Turkish path always renders Turkish, visiting its
/// "/en" equivalent always renders English, regardless of any previous
/// request. This keeps the language switch a plain navigation (and
/// therefore shareable/bookmarkable/crawlable per-language URL) instead of
/// hidden server-side state that could desync from what the URL shows.
/// </summary>
public class RouteSegmentRequestCultureProvider : IRequestCultureProvider
{
    private const string EnglishCulture = "en-US";
    private const string TurkishCulture = "tr-TR";

    public Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var culture = LanguageUrlService.IsEnglishPath(httpContext.Request.Path)
            ? EnglishCulture
            : TurkishCulture;

        return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(culture));
    }
}
