using System.Globalization;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class NavbarViewComponent : ViewComponent
{
    // Controllers grouped under the Kurumsal dropdown — kept in one place
    // so the active-state check and the dropdown's own links can never
    // drift apart (docs/14_Decisions.md, Global Navigation & Search
    // milestone: HR Policy moved here from the now-flattened Kariyer item).
    // "SocialResponsibility" (Sosyal Sorumluluk Projelerimiz, 2026-09-28
    // client request) added alongside the existing four.
    private static readonly string[] CorporateControllers = { "About", "Values", "HrPolicy", "Kvkk", "SocialResponsibility" };

    public IViewComponentResult Invoke()
    {
        var currentController = ViewContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

        // English localization (2026-10-02) — the language switch must
        // land on the EN/TR equivalent of the page being viewed right now,
        // not always Home, so it is computed from the actual request path
        // (including its query string, e.g. a Projects filter) rather than
        // from any static per-controller URL.
        var currentPath = Request.Path;
        var currentQuery = Request.QueryString;

        var model = new NavbarViewModel
        {
            IsHomeActive = currentController.Equals("Home", StringComparison.OrdinalIgnoreCase),
            IsCorporateActive = CorporateControllers.Any(c => currentController.Equals(c, StringComparison.OrdinalIgnoreCase)),
            IsProjectsActive = currentController.Equals("Projects", StringComparison.OrdinalIgnoreCase),
            IsCareerActive = currentController.Equals("Career", StringComparison.OrdinalIgnoreCase),
            IsContactActive = currentController.Equals("Contact", StringComparison.OrdinalIgnoreCase),
            IsEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase),
            TurkishUrl = LanguageUrlService.GetTurkishEquivalent(currentPath, currentQuery),
            EnglishUrl = LanguageUrlService.GetEnglishEquivalent(currentPath, currentQuery)
        };

        return View(model);
    }
}
