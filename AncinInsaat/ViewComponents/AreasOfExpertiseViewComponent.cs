using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// About Us "Areas of Expertise" section (docs/03_PageBlueprints.md — About
// Us, Areas of Expertise phase, 2026-08-01). Reuses the Information Card
// component in its Grid layout rather than the doc's separately-named
// Expertise Card — a responsive 3/2/1-column grid of icon+title+description
// cards, same .grid-cols-3 utility Columns=3 maps onto (Section 1 of
// site.css) and the same fade-up-with-stagger every ungrouped Grid item
// already gets from data-reveal-group (Reveal left empty on every item).
//
// Items mirror Project.ProjectType's existing reference taxonomy
// ("Residence", "Villa", "Commercial", "Office", "Mixed Use" — see
// Project.cs) rather than inventing unrelated categories, matching the
// English labels already shown as-is in the Projects page's Project Type
// filter (no Turkish translation layer exists for this taxonomy yet).
//
// PLACEHOLDER descriptions — the client has not yet supplied real
// per-category copy. Replace before launch.
public class AreasOfExpertiseViewComponent : ViewComponent
{
    // Hand-authored icons, same convention as ContactController/
    // MissionVisionViewComponent (fill="none", stroke="currentColor",
    // stroke-width 1.6, 24x24 viewBox).

    private const string ResidenceIconMarkup = """
        <path d="M4 11.5L12 4l8 7.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        <path d="M6 10v9a1 1 0 001 1h10a1 1 0 001-1v-9" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <path d="M10 20v-5h4v5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        """;

    private const string VillaIconMarkup = """
        <path d="M3.5 12L8 7.5l4 3 4-3 4.5 4.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        <path d="M5 11v7.5a1 1 0 001 1h12a1 1 0 001-1V11" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <path d="M9 19.5v-4h6v4" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        """;

    private const string CommercialIconMarkup = """
        <path d="M4 9.5L5 4h14l1 5.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <path d="M4 9.5a2.5 2.5 0 005 0 2.5 2.5 0 005 0 2.5 2.5 0 005 0 2.5 2.5 0 005 0" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        <path d="M5.5 11v8a1 1 0 001 1h11a1 1 0 001-1v-8" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <path d="M10 20v-4h4v4" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        """;

    private const string OfficeIconMarkup = """
        <rect x="7" y="3.5" width="10" height="17" rx="1" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M10 7h1M13 7h1M10 10.5h1M13 10.5h1M10 14h1M13 14h1" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        <path d="M10.5 20.5v-3h3v3" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        """;

    private const string MixedUseIconMarkup = """
        <rect x="3.5" y="10" width="8" height="10" rx="1" fill="none" stroke="currentColor" stroke-width="1.6" />
        <rect x="12.5" y="5" width="8" height="15" rx="1" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M6 13.5h3M6 16.5h3M15 8.5h3M15 11.5h3M15 14.5h3" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        """;

    public IViewComponentResult Invoke()
    {
        var model = new AreasOfExpertiseViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "areas-of-expertise-heading",
                Eyebrow = "UZMANLIK ALANLARIMIZ",
                Title = "Uzmanlık Alanlarımız",
                Description = "Farklı ölçek ve ihtiyaçlara uygun, uçtan uca proje geliştirme deneyimimiz."
            },
            InformationCard = new InformationCardModel
            {
                Layout = InformationCardLayout.Grid,
                Columns = 3,
                Items = new List<InformationCardItem>
                {
                    new()
                    {
                        IconMarkup = ResidenceIconMarkup,
                        Label = "Residence",
                        Value = "Modern mimari ve yaşam konforunu bir araya getiren, ailelerin uzun " +
                            "yıllar güvenle yaşayabileceği konut projeleri geliştiriyoruz."
                    },
                    new()
                    {
                        IconMarkup = VillaIconMarkup,
                        Label = "Villa",
                        Value = "Geniş yaşam alanları ve özel bahçe konseptleriyle, ayrıcalıklı bir " +
                            "yaşam sunan villa projelerine imza atıyoruz."
                    },
                    new()
                    {
                        IconMarkup = CommercialIconMarkup,
                        Label = "Commercial",
                        Value = "İşletmelerin ihtiyaçlarına uygun, fonksiyonel ve stratejik konumlarda " +
                            "ticari yapılar inşa ediyoruz."
                    },
                    new()
                    {
                        IconMarkup = OfficeIconMarkup,
                        Label = "Office",
                        Value = "Verimliliği ve konforu önceliklendiren, modern çalışma ortamları " +
                            "sunan ofis projeleri geliştiriyoruz."
                    },
                    new()
                    {
                        IconMarkup = MixedUseIconMarkup,
                        Label = "Mixed Use",
                        Value = "Konut, ticaret ve sosyal yaşamı tek bir yapıda buluşturan karma " +
                            "kullanım projeleri tasarlıyoruz."
                    }
                }
            }
        };

        return View(model);
    }
}
