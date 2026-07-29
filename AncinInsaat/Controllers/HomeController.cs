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

    public async Task<IActionResult> Index()
    {
        ViewData["Seo"] = await _seoService.GetPageSeoAsync("home", Request);

        return View();
    }
}
