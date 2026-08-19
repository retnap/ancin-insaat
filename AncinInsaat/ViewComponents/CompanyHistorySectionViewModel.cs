using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class CompanyHistorySectionViewModel
{
    public required TimelineHeaderModel Header { get; init; }
    public required VideoShowcaseModel Video { get; init; }
    public required HistoryInfoPanelModel InfoPanel { get; init; }
    public required HomeTimelineCarouselModel Carousel { get; init; }
}
