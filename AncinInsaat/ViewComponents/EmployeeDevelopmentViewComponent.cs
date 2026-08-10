using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// HR Policy "Employee Development" section (docs/03_PageBlueprints.md —
// Page HR Policy). Composes Section Header + Statistics Card (Section 33 of
// site.css — the newly introduced reusable large-number card,
// docs/04_ComponentLibrary.md) + a short prose column beneath it. Static
// content, same hardcoded-copy precedent as every other page-section
// ViewComponent — HR Policy has exactly one Employee Development section,
// so no calling page needs to pass data in.
//
// PLACEHOLDER copy and figures — the client has not yet supplied real
// employee-development statistics. The "53+" figure reuses the one
// confirmed real fact already established elsewhere on the site ("53 Yıllık
// Tecrübe" from Company Overview); the remaining two figures are
// realistic-but-fictional placeholders and must be replaced with verified
// numbers before launch.
public class EmployeeDevelopmentViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new EmployeeDevelopmentViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "employee-development-heading",
                Eyebrow = "GELİŞİM",
                Title = "Çalışan Gelişimine Yatırım",
                Description = "Ekibimizin mesleki gelişimini destekleyen imkanlar sunuyor, uzun soluklu kariyerler inşa ediyoruz.",
                CssClass = "section-header--center"
            },
            Statistics = new StatisticsCardModel
            {
                Columns = 3,
                Items = new List<StatisticItem>
                {
                    new() { Value = "53+", Label = "Yıllık Sektör Tecrübesi", Reveal = "left" },
                    new() { Value = "150+", Label = "Ekip Üyesi" },
                    new() { Value = "%90+", Label = "Çalışan Bağlılığı", Reveal = "right" }
                }
            },
            Paragraphs = new List<string>
            {
                "Çalışanlarımızın mesleki gelişimini, düzenlediğimiz eğitim programları ve saha " +
                    "tecrübesiyle destekliyoruz. Her ekip üyesinin kendi alanında uzmanlaşabileceği " +
                    "bir gelişim yolculuğu sunmayı hedefliyoruz.",
                "Uzun soluklu istihdamı ve içeriden terfiyi önceliklendiren yaklaşımımızla, " +
                    "ekibimizin Ancın İnşaat bünyesinde kariyerlerini güvenle sürdürmelerini sağlıyoruz."
            }
        };

        return View(model);
    }
}
