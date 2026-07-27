using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Per 01_SiteMap.md: visible on every page, middle-right, navigates to
// the Contact page. No model — content and destination are fixed.
public class FloatingContactButtonViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
