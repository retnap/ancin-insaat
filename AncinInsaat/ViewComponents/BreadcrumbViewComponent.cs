using System.Globalization;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class BreadcrumbViewComponent : ViewComponent
{
    // The root crumb is prepended automatically — every trail in
    // 01_SiteMap.md starts there, so callers only need to pass what
    // follows it.
    //
    // English localization (2026-10-02) — homeLabel/the root Url are now
    // derived from the current request's culture rather than a per-caller
    // literal, so every page's breadcrumb (including Career/Kvkk, which
    // never passed an explicit homeLabel and so silently showed the
    // English default "Home" even on the Turkish site) consistently reads
    // "Anasayfa"/"/" in Turkish and "Home"/"/en" in English. homeLabel
    // stays available as an explicit override for the rare case a caller
    // needs something else, but no current caller passes one anymore.
    public IViewComponentResult Invoke(IReadOnlyList<BreadcrumbItem> trail, string? homeLabel = null)
    {
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);
        var resolvedHomeLabel = homeLabel ?? (isEnglish ? "Home" : "Anasayfa");
        var rootUrl = isEnglish ? $"/{LanguageUrlService.EnglishPrefixSegment}" : "/";

        var items = new List<BreadcrumbItem> { new() { Label = resolvedHomeLabel, Url = rootUrl } };
        items.AddRange(trail);

        return View(new BreadcrumbViewModel { Items = items });
    }
}
