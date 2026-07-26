using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
