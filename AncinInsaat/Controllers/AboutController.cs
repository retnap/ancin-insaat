using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

// Route is explicit ("about-us") rather than relying on the default
// {controller=Home}/{action=Index}/{id?} convention (which would resolve
// to "/About") — docs/03_PageBlueprints.md and docs/10_SEO.md both specify
// "/about-us" as the page's canonical URL (see docs/01_SiteMap.md, updated
// to match — project owner's explicit 2026-08-01 decision resolving a
// conflict between the two).
public class AboutController : Controller
{
    private readonly ISeoService _seoService;

    public AboutController(ISeoService seoService)
    {
        _seoService = seoService;
    }

    [HttpGet("about-us")]
    public async Task<IActionResult> Index()
    {
        ViewData["Seo"] = await _seoService.GetPageSeoAsync("about-us", Request);
        ViewData["SeoPageType"] = "AboutPage";

        return View();
    }
}
