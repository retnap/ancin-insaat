using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

public class HomeController : Controller
{
    private readonly ISeoService _seoService;

    public HomeController(ISeoService seoService)
    {
        _seoService = seoService;
    }

    // Explicit attribute routes for both languages (English localization,
    // 2026-10-02) — see RouteSegmentRequestCultureProvider. Once an action
    // carries any attribute route, ASP.NET Core MVC stops matching it via
    // Program.cs's conventional default route, so "" (root) must be listed
    // here explicitly alongside "en"; omitting it is what caused "/" to
    // 404 after "en" was added.
    [HttpGet("")]
    [HttpGet("en")]
    public async Task<IActionResult> Index()
    {
        ViewData["Seo"] = await _seoService.GetPageSeoAsync("home", Request);

        return View();
    }
}
