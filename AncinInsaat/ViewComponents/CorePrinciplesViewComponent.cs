using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// HR Policy "Core Principles" section (docs/03_PageBlueprints.md — Page HR
// Policy). Reuses the Information Card component in its Grid layout, same
// precedent as WhyJoinAncinViewComponent/MissionVisionViewComponent/
// AreasOfExpertiseViewComponent/CoreValuesViewComponent rather than the
// doc's separately-named Information Card variant needing its own markup —
// a responsive 4/2/1-column grid of icon+title+description cards. Static
// content, same hardcoded-copy precedent as those components — HR Policy
// has exactly one Core Principles section, so no calling page needs to pass
// data in.
//
// PLACEHOLDER copy — the client has not yet supplied the real HR principles.
// Replace before launch.
public class CorePrinciplesViewComponent : ViewComponent
{
    // Hand-authored icons, same convention as WhyJoinAncinViewComponent/
    // MissionVisionViewComponent (fill="none", stroke="currentColor",
    // stroke-width 1.6, 24x24 viewBox).

    private const string TransparencyIconMarkup = """
        <path d="M2.5 12S6 5.5 12 5.5 21.5 12 21.5 12 18 18.5 12 18.5 2.5 12 2.5 12z" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <circle cx="12" cy="12" r="3" fill="none" stroke="currentColor" stroke-width="1.6" />
        """;

    private const string FairnessIconMarkup = """
        <path d="M12 3.5v17M7 6.5h10" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        <path d="M4 9l3-2.5L10 9M14 9l3-2.5L20 9" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        <path d="M4 9a3 3 0 006 0M14 9a3 3 0 006 0" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        <path d="M8.5 20.5h7" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        """;

    private const string DevelopmentIconMarkup = """
        <path d="M4 18l5-5.5 3.5 3 6.5-7.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        <path d="M14.5 8h4.5v4.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        """;

    private const string RespectIconMarkup = """
        <circle cx="8.5" cy="8" r="2.6" fill="none" stroke="currentColor" stroke-width="1.6" />
        <circle cx="16" cy="9" r="2.2" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M3.5 19v-1.2c0-2.4 2.2-4.3 5-4.3s5 1.9 5 4.3V19" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        <path d="M14 14.3c2.3.2 4 1.9 4 3.9V19" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        """;

    public IViewComponentResult Invoke()
    {
        var model = new CorePrinciplesViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "core-principles-heading",
                Eyebrow = "TEMEL İLKELERİMİZ",
                Title = "İnsan Kaynakları Politikamızın Temelleri",
                Description = "Çalışma kültürümüzü şekillendiren, her kararımıza yön veren temel ilkeler."
            },
            InformationCard = new InformationCardModel
            {
                Layout = InformationCardLayout.Grid,
                Columns = 4,
                Items = new List<InformationCardItem>
                {
                    new()
                    {
                        IconMarkup = TransparencyIconMarkup,
                        Label = "Şeffaflık ve Güven",
                        Value = "Açık iletişimi ve karşılıklı güveni her sürecin merkezine koyuyoruz."
                    },
                    new()
                    {
                        IconMarkup = FairnessIconMarkup,
                        Label = "Fırsat Eşitliği",
                        Value = "İşe alımdan terfiye kadar her aşamada adil ve eşit fırsatlar sunuyoruz."
                    },
                    new()
                    {
                        IconMarkup = DevelopmentIconMarkup,
                        Label = "Sürekli Gelişim",
                        Value = "Çalışanlarımızın mesleki ve kişisel gelişimini sürekli destekliyoruz."
                    },
                    new()
                    {
                        IconMarkup = RespectIconMarkup,
                        Label = "Saygı ve İş Birliği",
                        Value = "Karşılıklı saygıya dayanan, iş birliğini önceliklendiren bir ekip ruhunu benimsiyoruz."
                    }
                }
            }
        };

        return View(model);
    }
}
