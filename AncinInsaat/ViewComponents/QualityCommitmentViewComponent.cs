using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Our Values "Quality Commitment" section (docs/03_PageBlueprints.md — Our
// Values, Quality Commitment phase, 2026-08-03). Centered Section Header
// (section-header--center, the same modifier Contact's Social Media
// section already uses) above a single narrow column of copy — no new
// component, same Section Header + Rich Text pairing the blueprint calls
// for. Static content, same hardcoded-copy precedent as
// ValuesIntroductionViewComponent — Our Values has exactly one Quality
// Commitment section, so no calling page needs to pass data in.
//
// PLACEHOLDER copy — the client has not yet supplied the real quality
// commitment statement. Built only from facts already established
// elsewhere on the site (1973 founding, güven/zanaatkârlık positioning).
// Replace before launch.
public class QualityCommitmentViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new QualityCommitmentViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "quality-commitment-heading",
                Eyebrow = "KALİTE ANLAYIŞIMIZ",
                Title = "Değerlerimiz, Her Projede Somutlaşır",
                CssClass = "section-header--center"
            },
            Paragraphs = new List<string>
            {
                "Yukarıdaki değerler bizim için birer söylemden ibaret değildir. Malzeme " +
                    "tedarikinden statik hesaplara, uygulama denetiminden teslim sonrası " +
                    "iletişime kadar her aşamada aynı kalite anlayışını uygularız.",
                "Bu yaklaşımın karşılığı, sakinlerine uzun yıllar boyunca güvenle " +
                    "yaşayabilecekleri, sağlam ve kalıcı yaşam alanlarıdır."
            }
        };

        return View(model);
    }
}
