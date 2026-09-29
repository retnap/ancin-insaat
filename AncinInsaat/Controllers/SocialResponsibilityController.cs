using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

// Route is explicit ("sosyal-sorumluluk-projelerimiz") — same
// explicit-attribute-route precedent as AboutController/ValuesController/
// HrPolicyController/KvkkController. New "Sosyal Sorumluluk Projelerimiz"
// Kurumsal navigation item (2026-09-28 client request) — intentionally an
// empty content page for now (see Views/SocialResponsibility/Index.cshtml):
// no static content, form or database entity exists for it yet, matching
// every other static Corporate page's thin-controller shape.
public class SocialResponsibilityController : Controller
{
    private readonly ISeoService _seoService;

    public SocialResponsibilityController(ISeoService seoService)
    {
        _seoService = seoService;
    }

    [HttpGet("sosyal-sorumluluk-projelerimiz")]
    public async Task<IActionResult> Index()
    {
        ViewData["Seo"] = await _seoService.GetPageSeoAsync("sosyal-sorumluluk-projelerimiz", Request);
        ViewData["SeoPageType"] = "WebPage";

        return View();
    }
}
