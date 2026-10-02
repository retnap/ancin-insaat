using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

// Route is explicit ("sosyal-sorumluluk-projelerimiz") — same
// explicit-attribute-route precedent as AboutController/ValuesController/
// HrPolicyController/KvkkController. "Sosyal Sorumluluk Projelerimiz"
// Kurumsal navigation item (2026-09-28 client request). Built out
// 2026-10-02 (Folkart "/sosyal-sorumluluk" reference revision — see
// Views/SocialResponsibility/Index.cshtml and
// SocialResponsibilityPillarsViewComponent) as static, hardcoded
// placeholder content — still no form or database entity, matching every
// other static Corporate page's thin-controller shape.
public class SocialResponsibilityController : Controller
{
    private readonly ISeoService _seoService;

    public SocialResponsibilityController(ISeoService seoService)
    {
        _seoService = seoService;
    }

    [HttpGet("sosyal-sorumluluk-projelerimiz")]
    [HttpGet("en/social-responsibility")]
    public async Task<IActionResult> Index()
    {
        ViewData["Seo"] = await _seoService.GetPageSeoAsync("sosyal-sorumluluk-projelerimiz", Request);
        ViewData["SeoPageType"] = "WebPage";

        return View();
    }
}
