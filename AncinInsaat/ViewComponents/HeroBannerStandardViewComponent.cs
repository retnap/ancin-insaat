using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// The non-Home Hero Banner variant (docs/04_ComponentLibrary.md's "Standard"
// variant) — a shorter, page-agnostic banner with no Project CTA and no
// Scroll Indicator, since 03_PageBlueprints.md lists only "Hero Banner" for
// Projects/Project Detail/About/etc., unlike Home which explicitly adds
// those two. Content is entirely caller-supplied rather than resolved from
// IProjectQueryService here, since the same component serves both a static
// page heading (Projects listing) and per-project content (Project Detail).
public class HeroBannerStandardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(
        string heading,
        string? subheading = null,
        string? backgroundImageUrl = null,
        string? statusLabel = null,
        string? statusModifierClass = null)
    {
        var model = new HeroBannerStandardViewModel
        {
            Heading = heading,
            Subheading = subheading,
            BackgroundImageUrl = backgroundImageUrl,
            StatusLabel = statusLabel,
            StatusModifierClass = statusModifierClass
        };

        return View(model);
    }
}
