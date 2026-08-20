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
        "Ançın İnşaat, Aydın'da attığı ilk adımlardan bu yana, sağlam mühendislik anlayışını " +
            "insana değer veren bir yaklaşımla birleştirerek yaşam alanları inşa ediyor. Yarım " +
            "asra yaklaşan yolculuğumuz boyunca, her projede kalıcı değer yaratma hedefinden hiç " +
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
            "gösteriyoruz. Şeffaf iletişim ve hızlı çözüm odaklı yaklaşımımız, Ançın İnşaat " +
            "imzasını taşıyan her projenin ayrılmaz bir parçası.",
        "Geleceğe baktığımızda, Aydın ve çevresinde daha fazla aileye kaliteli ve güvenli yaşam " +
            "alanları sunmaya devam etmeyi hedefliyoruz. Yarım asırlık tecrübemizi yeni nesil " +
            "projelere taşırken, her zaman insana değer veren anlayışımızdan ödün vermiyoruz."
    };

    public IViewComponentResult Invoke(string part)
    {
        var model = part switch
        {
            "intro" => new CompanyIntroductionViewModel { SectionId = "about-intro", Paragraphs = IntroParagraphs },
            "closing" => new CompanyIntroductionViewModel { SectionId = "about-closing", Paragraphs = ClosingParagraphs },
            _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Expected \"intro\" or \"closing\".")
        };

        return View(model);
    }
}
