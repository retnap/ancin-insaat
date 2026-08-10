using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Career page "Why Join Ançın" section (docs/03_PageBlueprints.md — Page
// Career). Reuses the Information Card component in its Grid layout,
// same precedent as AreasOfExpertiseViewComponent/MissionVisionViewComponent
// rather than the doc's separately-named Feature Card — a responsive
// 3/2/1-column grid of icon+title+description cards.
//
// PLACEHOLDER copy — the client has not yet supplied real working-culture
// content. Replace before launch.
public class WhyJoinAncinViewComponent : ViewComponent
{
    // Hand-authored icons, same convention as AreasOfExpertiseViewComponent
    // (fill="none", stroke="currentColor", stroke-width 1.6, 24x24 viewBox).

    private const string TrustIconMarkup = """
        <path d="M12 3.5l7 3v5.2c0 4.6-2.9 8.6-7 9.8-4.1-1.2-7-5.2-7-9.8V6.5l7-3z" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <path d="M8.7 12.3l2.4 2.4 4.2-4.6" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        """;

    private const string GrowthIconMarkup = """
        <path d="M4 18l5-5.5 3.5 3 6.5-7.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        <path d="M14.5 8h4.5v4.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        """;

    private const string TeamworkIconMarkup = """
        <circle cx="8.5" cy="8" r="2.6" fill="none" stroke="currentColor" stroke-width="1.6" />
        <circle cx="16" cy="9" r="2.2" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M3.5 19v-1.2c0-2.4 2.2-4.3 5-4.3s5 1.9 5 4.3V19" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        <path d="M14 14.3c2.3.2 4 1.9 4 3.9V19" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        """;

    private const string InnovationIconMarkup = """
        <path d="M9 18h6M10 21h4" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        <path d="M12 3.5a5.5 5.5 0 00-3 10.1c.6.4 1 1.1 1 1.9v.5h4v-.5c0-.8.4-1.5 1-1.9A5.5 5.5 0 0012 3.5z" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        """;

    public IViewComponentResult Invoke()
    {
        var model = new WhyJoinAncinViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "why-join-heading",
                Eyebrow = "NEDEN ANÇIN?",
                Title = "Neden Ancın İnşaat'ta Çalışmalısınız?",
                Description = "Yarım asra yaklaşan bir aile şirketinde, güven ve zanaatkârlıkla şekillenen bir kültürün parçası olun."
            },
            InformationCard = new InformationCardModel
            {
                Layout = InformationCardLayout.Grid,
                Columns = 4,
                Items = new List<InformationCardItem>
                {
                    new()
                    {
                        IconMarkup = TrustIconMarkup,
                        Label = "Güven Kültürü",
                        Value = "Yarım asra yaklaşan köklü bir aile şirketinin istikrarı ve güven ortamında çalışın."
                    },
                    new()
                    {
                        IconMarkup = GrowthIconMarkup,
                        Label = "Gelişim Fırsatları",
                        Value = "Mesleki gelişiminizi destekleyen, kariyerinizde ilerlemenize olanak tanıyan bir ortam."
                    },
                    new()
                    {
                        IconMarkup = TeamworkIconMarkup,
                        Label = "Güçlü Ekip Ruhu",
                        Value = "Birlikte üretmeyi ve birbirine destek olmayı önceliklendiren bir çalışma kültürü."
                    },
                    new()
                    {
                        IconMarkup = InnovationIconMarkup,
                        Label = "Yenilikçi Projeler",
                        Value = "Aydın'ın gelişimine katkı sağlayan, modern ve yenilikçi projelerde yer alın."
                    }
                }
            }
        };

        return View(model);
    }
}
