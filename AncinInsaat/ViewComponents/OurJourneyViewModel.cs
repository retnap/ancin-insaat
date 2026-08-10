using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class OurJourneyViewModel
{
    public required TimelineHeaderModel Header { get; init; }
    public required HistoryInfoPanelModel InfoPanel { get; init; }
    public required HistoryCarouselModel Carousel { get; init; }
}
