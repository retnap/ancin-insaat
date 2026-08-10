using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Map Component (docs/04_ComponentLibrary.md — Contact, and reusable on
// future corporate pages). Renders a live embed when embedUrl is supplied,
// otherwise a static on-brand illustration + "Google Maps'te Aç" link —
// same placeholder-until-real-data approach Project Detail's Location &
// Distances section already established for the same underlying problem
// (no interactive map without a Google Maps Embed API key, see
// docs/14_Decisions.md). A caller only ever needs to change which
// argument it passes; the component's own markup/behaviour never changes.
public class MapViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string? embedUrl, string? linkUrl)
    {
        return View(new MapViewModel { EmbedUrl = embedUrl, LinkUrl = linkUrl });
    }
}
