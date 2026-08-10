using AncinInsaat.Data;
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
// About Us Foundation phase (2026-08-01): the milestone list itself moved
// out to AncinInsaat.Data.CompanyHistoryData, now shared with the new
// OurJourneyViewComponent, so Home and About Us read one source instead of
// each keeping its own copy — see docs/14_Decisions.md.
//
// PLACEHOLDER video — the client has not yet supplied the real promotional
// footage; VideoSrc stays null until it does.
//
// Timeline header redesign (2026-08-03): the section heading is now the
// shared TimelineHeaderModel/_TimelineHeader partial (also used by
// OurJourneyViewComponent) instead of this component's own DecorativeTitle
// string, per the project owner's request that every Timeline usage share
// exactly one header design.
public class CompanyHistorySectionViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        const string sectionTitle = "Zaman Tüneli";

        var model = new CompanyHistorySectionViewModel
        {
            Header = new TimelineHeaderModel
            {
                HeadingId = "company-history-heading",

                // Lowercase on purpose (2026-08-03 header redesign) — the
                // giant background watermark keeps its natural casing
                // rather than sectionTitle's Title Case, which stays on
                // InfoPanel.Title below for that smaller, ordinary heading.
                DecorativeTitle = "zaman tüneli",
                Subtitle = "İlklerin Mimarı"
            },
            Video = new VideoShowcaseModel
            {
                Id = "company-history",
                Title = "Ancın İnşaat Tanıtım Filmi",
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
                Entries = CompanyHistoryData.Milestones
            }
        };

        return View(model);
    }
}
