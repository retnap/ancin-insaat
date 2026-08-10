using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// About Us "Company Introduction" section (docs/03_PageBlueprints.md —
// About Us Foundation phase, 2026-08-01). Reuses the exact building blocks
// Company Overview already established for this same "Section Header +
// Image Block + copy" shape (docs/02_DesignSystem.md's ".image-block"
// comment already anticipates this as "Company Story"), but composed with
// its own image/text ratio and column order (see CompanyIntroductionViewModel.TextFirst)
// so the page reads as About Us's own presentation rather than a repeat of
// Home. Static content per CLAUDE.md's Placeholder Content rules, same
// hardcoded-copy precedent as CompanyOverviewViewComponent — About Us has
// exactly one Company Introduction, so no calling page needs to pass data in.
//
// PLACEHOLDER copy and image — the client has not yet supplied the real
// company story text or photography. Realistic-but-fictional, deliberately
// avoids any invented business fact beyond the founding year (1973) and the
// "53 Yıllık Tecrübe" figure the project owner already supplied for Company
// Overview. Replace both before launch.
public class CompanyIntroductionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new CompanyIntroductionViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "company-introduction-heading",
                Eyebrow = "KİMİZ",
                Title = "Aydın'da Yarım Asırlık Bir Yolculuk"
            },
            Paragraphs = new List<string>
            {
                "Ancın İnşaat, 1973 yılından bu yana Aydın'da yaşam alanları inşa ediyor. " +
                    "Yarım asra yaklaşan bu yolculukta değişmeyen tek şey, her projeye aynı " +
                    "özenle yaklaşma prensibimiz oldu.",
                "Bugün de aynı anlayışla; sağlam mühendislik, modern mimari ve insana değer " +
                    "veren bir yaklaşımı bir araya getirerek, sakinlerine uzun yıllar boyunca " +
                    "güvenle yaşayabilecekleri alanlar sunmaya devam ediyoruz."
            },
            Image = new ImageBlockModel
            {
                Src = "/images/company/introduction-placeholder.svg",

                // Empty on purpose, same reasoning as Company Overview's image:
                // an abstract placeholder graphic that conveys no information
                // beyond what the heading/paragraphs already state.
                Alt = ""
            },
            TextFirst = false
        };

        return View(model);
    }
}
