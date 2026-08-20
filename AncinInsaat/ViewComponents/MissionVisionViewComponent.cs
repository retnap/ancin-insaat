using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// About Us "Mission & Vision" section (2026-08-13 Folkart-reference
// revision — docs/14_Decisions.md). Was an icon-based Information Card
// grid; rebuilt as a full-width darkened image panel with "Misyon"/
// "Vizyon" set over it, matching the reference screenshot's composition.
// Reuses the exact background-image mechanism Hero Banner/CTA Banner
// already use (a CSS custom property read by Default.cshtml/site.css,
// with a gradient overlay baked into the CSS rather than the image) so
// this panel needs no component of its own to swap the artwork — only
// PlaceholderImageUrl below changes when the client supplies a real photo.
//
// Mission/Vision copy is unchanged from the original grid version —
// still PLACEHOLDER text (the client has not supplied the real
// statements); replace before launch.
public class MissionVisionViewComponent : ViewComponent
{
    // Real photo supplied 2026-08-20 (project owner request) — replaces the
    // placeholder SVG that stood in for it since 2026-08-13. Nothing else in
    // the panel needs to change: background-size: cover on
    // .mission-vision-panel (site.css) already crops any image to the
    // panel's own dimensions without distorting it.
    private const string PlaceholderImageUrl = "/images/company/ancın misyon 1.png";

    public IViewComponentResult Invoke()
    {
        var model = new MissionVisionViewModel
        {
            BackgroundImageUrl = PlaceholderImageUrl,
            MissionLabel = "Misyon",
            MissionText = "Aydın'da, sağlam mühendislik ve insana değer veren bir anlayışla; " +
                "sakinlerine uzun yıllar boyunca güvenle yaşayabilecekleri, kaliteli ve " +
                "kalıcı yaşam alanları inşa etmek.",
            VisionLabel = "Vizyon",
            VisionText = "Yarım asra yaklaşan tecrübesini gelecek nesillere aktaran, bölgesinde " +
                "güven ve zanaatkârlığın öncüsü olarak anılan bir inşaat markası olmak."
        };

        return View(model);
    }
}
