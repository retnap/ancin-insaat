using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class BreadcrumbViewComponent : ViewComponent
{
    // "Home" is prepended automatically — every trail in 01_SiteMap.md
    // starts there, so callers only need to pass what follows it.
    public IViewComponentResult Invoke(IReadOnlyList<BreadcrumbItem> trail)
    {
        var items = new List<BreadcrumbItem> { new() { Label = "Home", Url = "/" } };
        items.AddRange(trail);

        return View(new BreadcrumbViewModel { Items = items });
    }
}
