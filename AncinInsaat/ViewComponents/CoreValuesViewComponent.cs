using System.Globalization;
using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Our Values "Core Values" section (docs/03_PageBlueprints.md — Our Values,
// Core Values phase, 2026-08-03). Reuses the Information Card component's
// Grid layout rather than the doc's separately-named Value Card — project
// owner's explicit 2026-08-03 instruction, same precedent as About Us's
// Mission & Vision / Areas of Expertise (InformationCardModel.Layout).
// Six items in a responsive 3/2/1-column grid (Columns=3, the shared
// .grid-cols-3 utility), fading upward with the default data-reveal-group
// stagger (no per-item Reveal set — like Areas of Expertise, these cards
// have no meaningful "from" side).
//
// PLACEHOLDER copy — the client has not yet supplied the real value
// statements. Built only from facts already established elsewhere on the
// site (1973 founding, Aydın, güven/zanaatkârlık positioning). Replace
// before launch.
public class CoreValuesViewComponent : ViewComponent
{
    // Hand-authored icons, same convention as MissionVisionViewComponent/
    // AreasOfExpertiseViewComponent (fill="none", stroke="currentColor",
    // stroke-width 1.6, 24x24 viewBox).

    private const string TrustIconMarkup = """
        <path d="M12 3.5l7 3v5.5c0 5-3.5 8-7 9-3.5-1-7-4-7-9V6.5l7-3z" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <path d="M9 12l2 2 4-4.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        """;

    private const string QualityIconMarkup = """
        <circle cx="12" cy="9.5" r="6" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M9.3 9.6l1.8 1.8 3.4-3.6" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        <path d="M9 14.5L7.5 20l4.5-2.3 4.5 2.3-1.5-5.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        """;

    private const string TransparencyIconMarkup = """
        <rect x="4" y="8" width="10" height="10" rx="1" fill="none" stroke="currentColor" stroke-width="1.6" />
        <rect x="9.5" y="4.5" width="10" height="10" rx="1" fill="none" stroke="currentColor" stroke-width="1.6" />
        """;

    private const string SustainabilityIconMarkup = """
        <path d="M6 18c-1-6 2-11 11-12 1 8-3 12-11 12z" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <path d="M7 17c2-3 5-6 9-9" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        """;

    private const string InnovationIconMarkup = """
        <path d="M12 3.5a5.5 5.5 0 00-3 10.1c.6.4 1 1.1 1 1.9v.5h4v-.5c0-.8.4-1.5 1-1.9A5.5 5.5 0 0012 3.5z" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <path d="M10 19h4M10.5 21h3" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        """;

    private const string CustomerFocusIconMarkup = """
        <circle cx="9" cy="8" r="2.6" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M4.5 19c0-3 2-5 4.5-5s4.5 2 4.5 5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        <circle cx="16.5" cy="9" r="2.2" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M14.5 19c.3-2.4 1.8-4 3.7-4 1.9 0 3.4 1.6 3.8 4" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" />
        """;

    public IViewComponentResult Invoke()
    {
        // English localization (English copy revision, 2026-11-03).
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

        var model = new CoreValuesViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "core-values-heading",
                Eyebrow = isEnglish ? "OUR CORE VALUES" : "TEMEL DEĞERLERİMİZ",
                Title = isEnglish ? "The Principles That Guide Us" : "Bize Yön Veren İlkeler",
                Description = isEnglish
                    ? "The core values we apply consistently to every project throughout our " +
                        "half-century journey in Aydın."
                    : "Aydın'daki yarım asırlık yolculuğumuzda, her projede aynı " +
                    "şekilde uyguladığımız temel değerler."
            },
            InformationCard = new InformationCardModel
            {
                Layout = InformationCardLayout.Grid,
                Columns = 3,
                Items = new List<InformationCardItem>
                {
                    new()
                    {
                        IconMarkup = TrustIconMarkup,
                        Label = isEnglish ? "Trust" : "Güven",
                        Value = isEnglish
                            ? "From the site to the office, we stand behind our word at every " +
                                "step, and our relationship with our customers continues long after handover."
                            : "Sahadan ofise, her adımda verdiğimiz sözün arkasında dururuz; " +
                            "müşterilerimizle kurduğumuz ilişki teslimattan sonra da sürer."
                    },
                    new()
                    {
                        IconMarkup = QualityIconMarkup,
                        Label = isEnglish ? "Quality" : "Kalite",
                        Value = isEnglish
                            ? "We inspect every stage, from material selection to construction, with " +
                                "the same rigour to build durable, lasting homes."
                            : "Malzeme seçiminden uygulamaya kadar her aşamayı aynı titizlikle " +
                            "denetler, kalıcı ve sağlam yaşam alanları inşa ederiz."
                    },
                    new()
                    {
                        IconMarkup = TransparencyIconMarkup,
                        Label = isEnglish ? "Transparency" : "Şeffaflık",
                        Value = isEnglish
                            ? "We keep our customers clearly and honestly informed about the process, " +
                                "costs and schedule."
                            : "Süreç, maliyet ve zaman planlaması hakkında müşterilerimizi net " +
                            "ve dürüst biçimde bilgilendiririz."
                    },
                    new()
                    {
                        IconMarkup = SustainabilityIconMarkup,
                        Label = isEnglish ? "Sustainability" : "Sürdürülebilirlik",
                        Value = isEnglish
                            ? "Through material and construction choices that respect natural resources, " +
                                "we create projects that leave lasting value for future generations."
                            : "Doğal kaynakları gözeten malzeme ve uygulama tercihleriyle, " +
                            "gelecek nesillere değer bırakan projeler üretiriz."
                    },
                    new()
                    {
                        IconMarkup = InnovationIconMarkup,
                        Label = isEnglish ? "Innovation" : "Yenilikçilik",
                        Value = isEnglish
                            ? "We continually improve the homes we build by following modern " +
                                "engineering and architectural practice."
                            : "Modern mühendislik ve mimari yaklaşımları takip ederek yaşam " +
                            "alanlarını sürekli geliştiririz."
                    },
                    new()
                    {
                        IconMarkup = CustomerFocusIconMarkup,
                        Label = isEnglish ? "Customer Focus" : "Müşteri Odaklılık",
                        Value = isEnglish
                            ? "We design and build every project around the needs of the people who " +
                                "will live in it."
                            : "Her projeyi, içinde yaşayacak insanların ihtiyaçlarını merkeze " +
                            "alarak tasarlar ve inşa ederiz."
                    }
                }
            }
        };

        return View(model);
    }
}
