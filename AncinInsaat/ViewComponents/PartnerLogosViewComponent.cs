using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Real logo images (wwwroot/images/logos/project-logos/), supplied
// 2026-07-28. Confirmed with the client the same day: despite the
// component/folder naming, these are Ançın's own project wordmarks (a
// portfolio trust strip), not third-party partner logos — nothing
// user-facing says "Partners" (see the section's aria-label in
// Default.cshtml). logo_01-removebg-preview is excluded per client
// instruction; it is Ançın's own company mark, not a project. ImageAlt
// transcribes the wordmark text visible in each source file rather than
// inventing a label. Part of the shared layout (renders on every page
// below the CTA Banner), so content here must stay generic rather than
// page-specific.
public class PartnerLogosViewComponent : ViewComponent
{
    private static readonly IReadOnlyList<PartnerLogoItem> Partners = new List<PartnerLogoItem>
    {
        new() { ImageSrc = "/images/logos/project-logos/logo_02-removebg-preview.png", ImageAlt = "Nlatis" },
        new() { ImageSrc = "/images/logos/project-logos/logo_03-removebg-preview.png", ImageAlt = "Tralles Gold Residence" },
        new() { ImageSrc = "/images/logos/project-logos/logo_04-removebg-preview.png", ImageAlt = "Alinda Gold Residence" },
        new() { ImageSrc = "/images/logos/project-logos/logo_05-removebg-preview.png", ImageAlt = "Magnesia Gold Residence" },
        new() { ImageSrc = "/images/logos/project-logos/logo_06-removebg-preview.png", ImageAlt = "La Fiore Karabağ" },
        new() { ImageSrc = "/images/logos/project-logos/logo_07-removebg-preview.png", ImageAlt = "La Fiore Karabağ 2. Etap" },
        new() { ImageSrc = "/images/logos/project-logos/logo_08-removebg-preview.png", ImageAlt = "Le Jardin" },
        new() { ImageSrc = "/images/logos/project-logos/logo_09-removebg-preview.png", ImageAlt = "Lavia Kuyulu" },
        new() { ImageSrc = "/images/logos/project-logos/logo_10-removebg-preview.png", ImageAlt = "Nysa Gold Residence" },
        new() { ImageSrc = "/images/logos/project-logos/logo_11-removebg-preview.png", ImageAlt = "Dlatis Thermal Wellness Residence" }
    };

    public IViewComponentResult Invoke()
    {
        var model = new PartnerLogosViewModel { Partners = Partners };
        return View(model);
    }
}
