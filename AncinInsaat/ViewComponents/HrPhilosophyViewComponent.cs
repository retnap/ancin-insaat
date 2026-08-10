using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// HR Policy "HR Philosophy" section (docs/03_PageBlueprints.md — Page HR
// Policy). Reuses the exact "Section Header + Image Block + copy" shape
// Company Overview/Company Introduction already established, in the Section
// Header + paragraphs / Image Block order Company Overview (Home) uses —
// Section 7's plain .grid.grid-cols-2 utility, no accent frame flourish
// (that stays Company Introduction's own signature). Static content, same
// hardcoded-copy precedent as every other page-section ViewComponent — HR
// Policy has exactly one HR Philosophy section, so no calling page needs to
// pass data in.
//
// PLACEHOLDER copy and image — the client has not yet supplied the real HR
// philosophy text or workplace photography. Realistic-but-fictional,
// deliberately reuses only the one confirmed real fact already established
// elsewhere on the site (1973 founding / "53 Yıllık Tecrübe"). Replace both
// before launch.
public class HrPhilosophyViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new HrPhilosophyViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "hr-philosophy-heading",
                Eyebrow = "İNSAN KAYNAKLARI",
                Title = "İnsana Değer Veren Bir Kurum Kültürü"
            },
            Paragraphs = new List<string>
            {
                "Ancın İnşaat'ta 53 yıllık tecrübemizin arkasında, her zaman insana verdiğimiz " +
                    "değer var. Çalışanlarımızın memnuniyetini ve gelişimini, tamamladığımız " +
                    "projeler kadar önemli görüyoruz.",
                "Şeffaf iletişimi, adil fırsat eşitliğini ve sürekli gelişimi önceliklendiren bir " +
                    "çalışma ortamı sunuyor; ekibimizin uzun soluklu ve güvene dayalı bir kariyer " +
                    "geçirebileceği bir kurum kültürü inşa ediyoruz."
            },
            Image = new ImageBlockModel
            {
                Src = "/images/company/hr-philosophy-placeholder.svg",

                // Empty on purpose, same reasoning as Company Overview/Company
                // Introduction's images: an abstract placeholder graphic that
                // conveys no information beyond what the heading/paragraphs
                // already state.
                Alt = ""
            }
        };

        return View(model);
    }
}
