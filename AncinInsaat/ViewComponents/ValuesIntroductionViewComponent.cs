using System.Globalization;
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
        // English localization (English copy revision, 2026-11-03) — same
        // per-request culture check as every other localized component.
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

        var model = new ValuesIntroductionViewModel
        {
            Paragraphs = isEnglish
                ? new List<string>
                {
                    "For Ancın İnşaat, values are not principles framed on a wall but a way " +
                        "of working that we apply to every project. Since 1973, every building " +
                        "we have delivered in Aydın has been shaped by the same diligence and " +
                        "sense of responsibility.",
                    "The values below guide our team's everyday decisions and approach on " +
                        "site; they are how we keep the promise we make to our customers."
                }
                : new List<string>
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
