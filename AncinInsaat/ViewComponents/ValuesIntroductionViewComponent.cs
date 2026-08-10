using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Our Values "Introduction" section (docs/03_PageBlueprints.md — Our
// Values, Introduction phase, 2026-08-03). Section Header (left, reveals
// from the left) beside its own copy (right, reveals from the right) — an
// even 50/50 split via the shared .grid-cols-2 utility (Section 7 of
// site.css), the same utility Company Overview/Company Introduction already
// use for a two-up section, just without an Image Block: this Introduction
// has no photography slot per 03_PageBlueprints.md (Section Header + Rich
// Text only), so the left column carries the heading instead of a media
// block. Static content, same hardcoded-copy precedent as
// CompanyIntroductionViewComponent — Our Values has exactly one
// Introduction, so no calling page needs to pass data in.
//
// PLACEHOLDER copy — the client has not yet supplied the real philosophy
// statement. Built only from facts already established on About Us (1973
// founding, Aydın, güven/zanaatkârlık positioning). Replace before launch.
public class ValuesIntroductionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new ValuesIntroductionViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "values-introduction-heading",
                Eyebrow = "FELSEFEMİZ",
                Title = "Her Projede Aynı Özen"
            },
            Paragraphs = new List<string>
            {
                "Ancın İnşaat için değerler, duvara asılı bir ilke listesi değil; her " +
                    "projede tekrarlanan bir çalışma biçimidir. 1973'ten bu yana Aydın'da " +
                    "inşa ettiğimiz her yapı, aynı titizlik ve sorumluluk anlayışıyla " +
                    "şekillendi.",
                "Aşağıdaki değerler, ekibimizin günlük kararlarına ve sahaya bakışına " +
                    "rehberlik eder; müşterilerimize verdiğimiz sözün karşılığıdır."
            }
        };

        return View(model);
    }
}
