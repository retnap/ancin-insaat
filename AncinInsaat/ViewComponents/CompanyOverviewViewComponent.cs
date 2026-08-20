using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Home page only (docs/03_PageBlueprints.md — Home > Company Overview).
// Static content per 06_ContentStructure.md ("Company Overview" is listed
// as Home content, not a standalone reusable section), consistent with
// FooterViewComponent and CtaBannerViewComponent's hardcoded-copy approach.
//
// PLACEHOLDER copy and image — the client has not yet supplied the real
// company introduction text or photography. Realistic-but-fictional per
// CLAUDE.md Placeholder Content rules; deliberately avoids any invented
// business fact (founding year, project count, employee count, etc.) that
// only the client can confirm. Replace both before launch.
//
// Layout/copy direction (2026-07-28 refinement) takes the "eyebrow + big
// heading + supporting paragraph + CTA" structure from
// Inspirationals/Terzioglu/terzioglu-48-yillik-tecrube.png as inspiration
// only — typography, color and spacing stay on our own Design System
// tokens rather than that reference's gold/sans-serif treatment. "50
// Yıllık Tecrübe" (corrected from "53", Home Page revision #3, 2026-08-10)
// is a placeholder headline supplied directly by the project owner, not a
// researched or invented company fact.
public class CompanyOverviewViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new CompanyOverviewViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "company-overview-heading",
                Eyebrow = "HAKKIMIZDA",
                Title = "50 Yıllık Tecrübe",
                Description = "Ancın İnşaat; güvenilirlik, zanaatkârlık ve modern mimari " +
                    "anlayışını bir araya getirerek yaşam alanları üretir. Her projede " +
                    "uzun soluklu değer yaratmayı ve sakinlerine huzurlu, işlevsel bir " +
                    "yaşam sunmayı hedefleriz."
            },
            Image = new ImageBlockModel
            {
                // Temporary "50 yıl" visual (Home Page revision #3,
                // 2026-08-10), replaced with the project owner's final
                // supplied file (2026-08-20) — the original
                // overview-placeholder.svg asset is left in place, unused,
                // rather than deleted.
                Src = "/images/logos/50-yil.jpeg",

                // Empty on purpose: this is a decorative stand-in graphic
                // that conveys no information beyond what the heading/
                // description already state. Give it a real descriptive alt
                // once the final company photography replaces it.
                Alt = "",

                // 50-yil.jpeg is a tall/portrait source image; the shared
                // .image-block 4:3 ratio crops too much of it away. This
                // modifier gives the box a taller ratio and anchors it to
                // the top of the grid row so it only grows downward.
                CssClass = "company-overview-image"
            },
            CtaLabel = "Devamını Oku",
            CtaUrl = "/about-us"
        };

        return View(model);
    }
}
