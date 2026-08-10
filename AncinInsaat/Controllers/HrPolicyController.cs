using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

// Route is explicit ("hr-policy") to match docs/01_SiteMap.md's "/hr-policy"
// — same explicit-attribute-route precedent as AboutController/
// ValuesController/CareerController. Static page — no database entity, no
// form, no additional service — matching docs/06_ContentStructure.md's
// classification of HR Policy as static content.
public class HrPolicyController : Controller
{
    private readonly ISeoService _seoService;

    public HrPolicyController(ISeoService seoService)
    {
        _seoService = seoService;
    }

    [HttpGet("hr-policy")]
    public async Task<IActionResult> Index()
    {
        ViewData["Seo"] = await _seoService.GetPageSeoAsync("hr-policy", Request);
        ViewData["SeoPageType"] = "AboutPage";

        return View();
    }
}
