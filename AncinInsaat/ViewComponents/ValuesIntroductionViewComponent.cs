using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Our Values "Introduction" section (docs/03_PageBlueprints.md — Our
// Values, Introduction phase, 2026-08-03). Folkart-reference revision
// (2026-08-16, project owner-approved): the reference has no separate
// section heading here, just a full-width introductory paragraph directly
// under the "Değerlerimiz" page title — same "drop the extra heading, keep
// the paragraphs" treatment as About/CompanyIntroductionViewComponent's own
// 2026-08-13 Folkart-reference revision. Static content, same
// hardcoded-copy precedent as CompanyIntroductionViewComponent — Our
// Values has exactly one Introduction, so no calling page needs to pass
// data in.
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
