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
// tokens rather than that reference's gold/sans-serif treatment. "53
// Yıllık Tecrübe" is a placeholder headline supplied directly by the
// project owner, not a researched or invented company fact.
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
                Title = "53 Yıllık Tecrübe",
                Description = "Ançın İnşaat; güvenilirlik, zanaatkârlık ve modern mimari " +
                    "anlayışını bir araya getirerek yaşam alanları üretir. Her projede " +
                    "uzun soluklu değer yaratmayı ve sakinlerine huzurlu, işlevsel bir " +
                    "yaşam sunmayı hedefleriz."
            },
            Image = new ImageBlockModel
            {
                Src = "/images/company/overview-placeholder.svg",

                // Empty on purpose: this is an abstract placeholder graphic
                // (no photography supplied yet) that conveys no information
                // beyond what the heading/description already state, so it
                // is decorative for screen-reader purposes. Give it a real
                // descriptive alt once actual company photography replaces it.
                Alt = ""
            },
            CtaLabel = "Devamını Oku",
            CtaUrl = "/about-us"
        };

        return View(model);
    }
}
