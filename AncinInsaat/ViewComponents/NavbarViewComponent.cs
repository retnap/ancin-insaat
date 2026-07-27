using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class NavbarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var currentController = ViewContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

        var model = new NavbarViewModel
        {
            IsHomeActive = currentController.Equals("Home", StringComparison.OrdinalIgnoreCase),
            IsProjectsActive = currentController.Equals("Projects", StringComparison.OrdinalIgnoreCase),
            IsContactActive = currentController.Equals("Contact", StringComparison.OrdinalIgnoreCase)
        };

        return View(model);
    }
}
