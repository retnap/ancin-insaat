using AncinInsaat.Data;
using AncinInsaat.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// About Us "Our Journey" section (docs/03_PageBlueprints.md — About Us
// Foundation phase, 2026-08-01) — the reuse docs/14_Decisions.md's
// 2026-07-28 Company History entry already flagged: History Info Panel and
// History Carousel, unchanged, driven by the same AncinInsaat.Data.
// CompanyHistoryData milestones CompanyHistorySectionViewComponent (Home)
// reads, so editing a milestone updates both pages. Unlike Home's version,
// this has no Video Showcase (that is Home's own promotional teaser, not
// company history).
//
// A distinct Id ("our-journey" vs Home's "company-history") on both
// InfoPanel and Carousel lets site.js pair this page's own instance
// without colliding with Home's, per HistoryCarouselModel.Id's existing
// multi-instance design.
//
// Timeline header redesign (2026-08-03): swapped the standalone
// SectionHeader (eyebrow + title + description) for the same
// TimelineHeaderModel/_TimelineHeader partial Home's Company History
// uses, per the project owner's request that every Timeline usage share
// exactly one header design — eyebrow and description had no equivalent
// slot in the new design and were dropped rather than kept alongside it.
public class OurJourneyViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        // History Info Panel's own small nav-adjacent label — unrelated to
        // the Timeline header redesign below, left as its pre-existing text.
        const string infoPanelTitle = "Kilometre Taşlarımız";

        var model = new OurJourneyViewModel
        {
            Header = new TimelineHeaderModel
            {
                HeadingId = "our-journey-heading",

                // Lowercase on purpose (2026-08-03 header redesign) — see
                // CompanyHistorySectionViewComponent's matching comment.
                DecorativeTitle = "zaman tüneli",
                Subtitle = "İlklerin Mimarı"
            },
            InfoPanel = new HistoryInfoPanelModel
            {
                Id = "our-journey",
                Title = infoPanelTitle
            },
            Carousel = new HistoryCarouselModel
            {
                Id = "our-journey",
                Entries = CompanyHistoryData.Milestones
            }
        };

        return View(model);
    }
}
