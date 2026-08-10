using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class BreadcrumbViewComponent : ViewComponent
{
    // The root crumb is prepended automatically — every trail in
    // 01_SiteMap.md starts there, so callers only need to pass what
    // follows it. homeLabel defaults to "Home" (unchanged for every
    // existing caller); Projects/Index.cshtml is the first to override it
    // with "Anasayfa" (Home Page revision #4, 2026-08-10) rather than
    // fixing the default site-wide, which would also relabel the other six
    // pages already using this component.
    public IViewComponentResult Invoke(IReadOnlyList<BreadcrumbItem> trail, string homeLabel = "Home")
    {
        var items = new List<BreadcrumbItem> { new() { Label = homeLabel, Url = "/" } };
        items.AddRange(trail);

        return View(new BreadcrumbViewModel { Items = items });
    }
}
