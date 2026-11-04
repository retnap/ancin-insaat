using System.Globalization;
using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// About Us "existing text content", now split around Mission & Vision
// (2026-08-13 Folkart-reference corrective revision — docs/14_Decisions.md).
// Renders in two calls — "intro" (before the panel) and "closing" (after
// it) — each with three paragraphs, matching the project owner's explicit
// "multiple paragraphs before / multiple paragraphs after" requirement.
//
// Company story copy (2026-08-20 client request — supersedes the earlier
// 2026-08-13 instruction not to invent corporate copy, replaced with an
// explicit sitewide request to remove all remaining Lorem Ipsum). Grounded
// in facts already established as real, approved copy elsewhere on the
// site rather than invented from scratch: the 1973 founding year (SEO
// MetaDescription for "about-us", SeedSeoMetadataAsync), the "yarım asır" /
// "50 Yıllık Tecrübe" framing (MissionVisionViewComponent's Vision text,
// CompanyOverviewViewComponent's Home heading) and Aydın as the company's
// home region (used throughout the site). No new business fact (project
// counts, employee counts, awards) is introduced. No heading accompanies
// it, same as before (the "KİMİZ" / "Aydın'da Yarım Asırlık Bir Yolculuk"
// heading was removed, not renamed, 2026-08-13).
public class CompanyIntroductionViewComponent : ViewComponent
{
    private static readonly IReadOnlyList<string> IntroParagraphs = new List<string>
    {
        "Ancın İnşaat, Aydın'da attığı ilk adımlardan bu yana, sağlam mühendislik anlayışını " +
            "insana değer veren bir yaklaşımla birleştirerek yaşam alanları inşa ediyor. Yarım " +
            "asırlık yolculuğumuz boyunca, her projede kalıcı değer yaratma hedefinden hiç " +
            "vazgeçmedik.",
        "Kurumsal kültürümüzün merkezinde şeffaflık, zanaatkârlık ve müşteri memnuniyeti yer " +
            "alıyor. Her projeyi, sakinlerinin uzun yıllar güvenle yaşayabileceği bir yuva olarak " +
            "tasarlıyor ve bu sorumlulukla hareket ediyoruz.",
        "Aydın'ın gelişen bölgelerinde hayata geçirdiğimiz konut ve villa projeleriyle, modern " +
            "mimariyi bölgenin dokusuyla uyumlu bir şekilde buluşturuyoruz. Deneyimimizi geleceğe " +
            "taşırken, her zaman insana ve doğaya saygılı bir inşaat anlayışını önceliklendiriyoruz."
    };

    private static readonly IReadOnlyList<string> ClosingParagraphs = new List<string>
    {
        "Alanında uzman mühendis, mimar ve saha ekiplerimizle, her projenin planlama " +
            "aşamasından teslim anına kadar titizlikle takip edildiği bir çalışma disiplini " +
            "benimsiyoruz. Kalite kontrolü, bizim için bir aşama değil, sürecin her adımında var " +
            "olan bir ilkedir.",
        "Sakinlerimizle kurduğumuz güven ilişkisini, teslim sonrasında da sürdürmeye özen " +
            "gösteriyoruz. Şeffaf iletişim ve hızlı çözüm odaklı yaklaşımımız, Ancın İnşaat " +
            "imzasını taşıyan her projenin ayrılmaz bir parçası.",
        "Geleceğe baktığımızda, Aydın ve çevresinde daha fazla aileye kaliteli ve güvenli yaşam " +
            "alanları sunmaya devam etmeyi hedefliyoruz. Yarım asırlık tecrübemizi yeni nesil " +
            "projelere taşırken, her zaman insana değer veren anlayışımızdan ödün vermiyoruz."
    };

    // English localization (2026-10-02) — natural corporate-English
    // renderings of IntroParagraphs/ClosingParagraphs above, not literal
    // translations; same three-facts grounding (1973 founding year, the
    // "yarım asır"/half-century framing, Aydın as home region), no new
    // business fact introduced.
    private static readonly IReadOnlyList<string> IntroParagraphsEn = new List<string>
    {
        "Since its first steps in Aydın, Ancın İnşaat has combined sound engineering with a " +
            "people-centred approach to create places to live. Throughout a journey of " +
            "half a century, we have remained committed to creating lasting value in every " +
            "project.",
        "Transparency, craftsmanship and customer satisfaction are at the heart of our " +
            "corporate culture. We design every project as a home where residents can live " +
            "safely for many years, and we act with that responsibility in mind.",
        "Through our residential and villa developments in Aydın's growing districts, we " +
            "bring modern architecture into harmony with the character of the region. As we " +
            "carry our experience forward, we remain committed to building with respect for " +
            "people and nature."
    };

    private static readonly IReadOnlyList<string> ClosingParagraphsEn = new List<string>
    {
        "Working with expert engineers, architects and site teams, we follow every project " +
            "meticulously from planning through to handover. For us, quality control is not a " +
            "single stage but a principle applied at every step of the process.",
        "We continue to nurture the trust we build with our residents long after handover. " +
            "Transparent communication and a responsive, solution-focused approach are " +
            "integral to every Ancın İnşaat project.",
        "Looking ahead, we aim to provide more families in Aydın and the surrounding area " +
            "with quality, safe homes. As we bring half a century of experience to a new " +
            "generation of projects, our people-centred approach remains unchanged."
    };

    public IViewComponentResult Invoke(string part)
    {
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

        var model = part switch
        {
            "intro" => new CompanyIntroductionViewModel { SectionId = "about-intro", Paragraphs = isEnglish ? IntroParagraphsEn : IntroParagraphs },
            "closing" => new CompanyIntroductionViewModel { SectionId = "about-closing", Paragraphs = isEnglish ? ClosingParagraphsEn : ClosingParagraphs },
            _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Expected \"intro\" or \"closing\".")
        };

        return View(model);
    }
}
