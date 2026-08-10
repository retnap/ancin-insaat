using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

// Route is explicit ("values") to match docs/10_SEO.md's URL Structure and
// docs/01_SiteMap.md's "/values" — same explicit-attribute-route precedent
// as AboutController ("about-us").
public class ValuesController : Controller
{
    private readonly ISeoService _seoService;

    public ValuesController(ISeoService seoService)
    {
        _seoService = seoService;
    }

    [HttpGet("values")]
    public async Task<IActionResult> Index()
    {
        ViewData["Seo"] = await _seoService.GetPageSeoAsync("values", Request);
        ViewData["SeoPageType"] = "AboutPage";

        return View();
    }
}
