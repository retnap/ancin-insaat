using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Part of the shared layout (renders on every page, between the page
// content and Partner Logos) as a single reusable "All Projects" banner
// rather than a page-specific contact CTA — see Milestone 2 design
// refinement (Folkart reference). Copy must stay page-agnostic.
public class CtaBannerViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

        var model = new CtaBannerViewModel
        {
            Heading = isEnglish ? "All Projects" : "Tüm Projeler",
            LinkUrl = isEnglish ? "/en/projects" : "/projects"
        };

        return View(model);
    }
}
