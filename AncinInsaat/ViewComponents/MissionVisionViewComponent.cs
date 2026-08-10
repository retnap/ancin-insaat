using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// About Us "Mission & Vision" section (docs/03_PageBlueprints.md — About Us,
// Mission & Vision phase, 2026-08-01). Reuses the Information Card component
// in its Grid layout (InformationCardModel.Layout) rather than introducing
// the doc's separately-named Mission Card/Vision Card — two items, one
// left-revealing, one right-revealing, same convention Company
// Introduction's media/content split and Contact's info/form split already
// use for a two-up section. Static content, same hardcoded-copy precedent
// as CompanyIntroductionViewComponent — About Us has exactly one Mission &
// Vision section, so no calling page needs to pass data in.
//
// PLACEHOLDER copy — the client has not yet supplied the real mission/vision
// statements. Realistic-but-fictional, built only from facts already
// established elsewhere on the page (1973 founding, Aydın, güven/
// zanaatkârlık positioning from Company Introduction). Replace before launch.
public class MissionVisionViewComponent : ViewComponent
{
    // Target/compass mark — "mission" as a fixed, focused aim. Same
    // hand-authored-icon convention as ContactController's icon consts
    // (fill="none", stroke="currentColor", stroke-width 1.6, 24x24 viewBox).
    private const string MissionIconMarkup = """
        <circle cx="12" cy="12" r="8.5" fill="none" stroke="currentColor" stroke-width="1.6" />
        <circle cx="12" cy="12" r="4.5" fill="none" stroke="currentColor" stroke-width="1.6" />
        <circle cx="12" cy="12" r="1" fill="currentColor" stroke="none" />
        """;

    // Eye mark — "vision" as foresight/outlook.
    private const string VisionIconMarkup = """
        <path d="M2.5 12S6 5.5 12 5.5 21.5 12 21.5 12 18 18.5 12 18.5 2.5 12 2.5 12z" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <circle cx="12" cy="12" r="3" fill="none" stroke="currentColor" stroke-width="1.6" />
        """;

    public IViewComponentResult Invoke()
    {
        var model = new MissionVisionViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "mission-vision-heading",
                Eyebrow = "MİSYON & VİZYON",
                Title = "Bizi Yönlendiren Amaç",
                Description = "Her projede aynı özenle ilerlememizi sağlayan amacımız ve ulaşmak istediğimiz gelecek."
            },
            InformationCard = new InformationCardModel
            {
                Layout = InformationCardLayout.Grid,
                Columns = 2,
                Items = new List<InformationCardItem>
                {
                    new()
                    {
                        IconMarkup = MissionIconMarkup,
                        Label = "Misyon",
                        Value = "Aydın'da, sağlam mühendislik ve insana değer veren bir anlayışla; " +
                            "sakinlerine uzun yıllar boyunca güvenle yaşayabilecekleri, kaliteli ve " +
                            "kalıcı yaşam alanları inşa etmek.",
                        Reveal = "left"
                    },
                    new()
                    {
                        IconMarkup = VisionIconMarkup,
                        Label = "Vizyon",
                        Value = "Yarım asra yaklaşan tecrübesini gelecek nesillere aktaran, bölgesinde " +
                            "güven ve zanaatkârlığın öncüsü olarak anılan bir inşaat markası olmak.",
                        Reveal = "right"
                    }
                }
            }
        };

        return View(model);
    }
}
