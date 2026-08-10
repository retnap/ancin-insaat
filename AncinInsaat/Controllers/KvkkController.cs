using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

// Route is explicit ("kvkk") to match docs/01_SiteMap.md's "/kvkk" and the
// existing Navbar link (Views/Shared/Components/Navbar/Default.cshtml) —
// same explicit-attribute-route precedent as AboutController/
// ValuesController/HrPolicyController. Static legal page — no database
// entity, no form, no additional service — matching
// docs/06_ContentStructure.md's classification of KVKK as static content.
public class KvkkController : Controller
{
    private readonly ISeoService _seoService;

    public KvkkController(ISeoService seoService)
    {
        _seoService = seoService;
    }

    [HttpGet("kvkk")]
    public async Task<IActionResult> Index()
    {
        ViewData["Seo"] = await _seoService.GetPageSeoAsync("kvkk", Request);
        ViewData["SeoPageType"] = "WebPage";

        return View();
    }
}
