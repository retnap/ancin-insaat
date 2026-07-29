using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class CompanyHistorySectionViewModel
{
    public required string HeadingId { get; init; }
    public required string DecorativeTitle { get; init; }
    public required VideoShowcaseModel Video { get; init; }
    public required HistoryInfoPanelModel InfoPanel { get; init; }
    public required HistoryCarouselModel Carousel { get; init; }
}
