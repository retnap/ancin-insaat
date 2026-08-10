using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class NavbarViewComponent : ViewComponent
{
    // Controllers grouped under the Kurumsal dropdown — kept in one place
    // so the active-state check and the dropdown's own links can never
    // drift apart (docs/14_Decisions.md, Global Navigation & Search
    // milestone: HR Policy moved here from the now-flattened Kariyer item).
    private static readonly string[] CorporateControllers = { "About", "Values", "HrPolicy", "Kvkk" };

    public IViewComponentResult Invoke()
    {
        var currentController = ViewContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

        var model = new NavbarViewModel
        {
            IsHomeActive = currentController.Equals("Home", StringComparison.OrdinalIgnoreCase),
            IsCorporateActive = CorporateControllers.Any(c => currentController.Equals(c, StringComparison.OrdinalIgnoreCase)),
            IsProjectsActive = currentController.Equals("Projects", StringComparison.OrdinalIgnoreCase),
            IsCareerActive = currentController.Equals("Career", StringComparison.OrdinalIgnoreCase),
            IsContactActive = currentController.Equals("Contact", StringComparison.OrdinalIgnoreCase)
        };

        return View(model);
    }
}
