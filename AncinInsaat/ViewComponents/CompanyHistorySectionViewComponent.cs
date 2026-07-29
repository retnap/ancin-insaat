using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Home page only. Not yet present in docs/03_PageBlueprints.md's Home
// section list (Timeline there is scoped to About Us > "Our Journey") —
// added at the project owner's explicit 2026-07-28 request, closely
// following Inspirationals/Terzioglu's "ZAMAN TÜNELİ" section for
// layout/interaction only. Typography and colour stay on our own Design
// System tokens rather than that reference's treatment, per
// docs/02_DesignSystem.md.
//
// Rebuilt again 2026-07-28 at the project owner's explicit request to
// closely match the Inspirationals reference's actual composition: the
// reference is NOT a vertical timeline with years on a rail — every
// historical entry is an independent card (year included), and the left
// column is pure navigation. So this now composes three reusable pieces
// (VideoShowcase + HistoryInfoPanel + HistoryCarousel) instead of the
// previous (VideoShowcase + SectionHeader + HistoryTimeline +
// HistoryCarousel), with HistoryInfoPanel/HistoryCarousel intended for
// later reuse on About Us "Our Journey" and eventual Admin Panel data.
//
// PLACEHOLDER copy, video and history entries — the client has not yet
// supplied the real promotional video or company history. Years are
// kept consistent with the "53 Yıllık Tecrübe" headline already
// supplied by the project owner in Company Overview (founded ~1973),
// but the milestone descriptions themselves are generic corporate
// placeholder text, not researched or invented company facts. Card
// images reuse Company Overview's abstract placeholder graphic rather
// than inventing per-era photography. Replace all of it, including
// VideoSrc, before launch.
public class CompanyHistorySectionViewComponent : ViewComponent
{
    private static readonly IReadOnlyList<HistoryEntryModel> PlaceholderHistory = new List<HistoryEntryModel>
    {
        new()
        {
            Year = "1973",
            Title = "Temellerin Atılması",
            Description = "Ançın İnşaat, Denizli'de ilk projeleriyle inşaat sektöründeki yolculuğuna başladı.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        },
        new()
        {
            Year = "1990",
            Title = "Kurumsal Büyüme",
            Description = "Artan proje hacmiyle birlikte kurumsal yapı ve saha organizasyonu güçlendirildi.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        },
        new()
        {
            Year = "2005",
            Title = "Yeni Nesil Konut Anlayışı",
            Description = "Modern mimari yaklaşımlar ve daha yüksek yapı standartları projelere yansıtıldı.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        },
        new()
        {
            Year = "2015",
            Title = "Bölgesel Genişleme",
            Description = "Farklı bölgelerdeki yeni projelerle şirketin proje portföyü genişledi.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        },
        new()
        {
            Year = "2026",
            Title = "Bugün",
            Description = "Ançın İnşaat, güven ve kaliteyi önceliklendirerek yoluna kararlılıkla devam ediyor.",
            ImageSrc = "/images/company/overview-placeholder.svg",
            ImageAlt = "",
            LinkUrl = "/projects",
            LinkLabel = "devamı"
        }
    };

    public IViewComponentResult Invoke()
    {
        const string sectionTitle = "Zaman Tüneli";

        var model = new CompanyHistorySectionViewModel
        {
            HeadingId = "company-history-heading",
            DecorativeTitle = sectionTitle,
            Video = new VideoShowcaseModel
            {
                Id = "company-history",
                Title = "Ançın İnşaat Tanıtım Filmi",
                PosterSrc = "/images/company/video-poster-placeholder.svg",

                // Empty on purpose — abstract placeholder graphic, decorative
                // for screen-reader purposes until real video-frame artwork
                // replaces it (same reasoning as Company Overview's image).
                PosterAlt = "",
                VideoSrc = null,
                CssClass = "video-showcase--compact"
            },
            InfoPanel = new HistoryInfoPanelModel
            {
                Id = "company-history",
                Title = sectionTitle
            },
            Carousel = new HistoryCarouselModel
            {
                Id = "company-history",
                Entries = PlaceholderHistory
            }
        };

        return View(model);
    }
}
