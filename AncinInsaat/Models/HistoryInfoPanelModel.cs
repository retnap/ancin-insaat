namespace AncinInsaat.Models;

public class HistoryInfoPanelModel
{
    // Pairs this panel's Prev/Next buttons with the matching
    // _HistoryCarousel at runtime (site.js), same Id convention as
    // HistoryCarouselModel.Id.
    public required string Id { get; init; }

    public required string Title { get; init; }
}
