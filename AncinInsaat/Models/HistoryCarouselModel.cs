namespace AncinInsaat.Models;

public class HistoryEntryModel
{
    public required string Year { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string ImageSrc { get; init; }
    public required string ImageAlt { get; init; }
    public required string LinkUrl { get; init; }
    public required string LinkLabel { get; init; }
}

public class HistoryCarouselModel
{
    // Namespaces the data attributes so _HistoryCarousel can be paired at
    // runtime with its _HistoryInfoPanel (Prev/Next lives on the panel,
    // not the carousel — see CompanyHistorySectionViewComponent) and so
    // more than one instance can coexist on a page without colliding
    // (e.g. a future About Us "Our Journey" use).
    public required string Id { get; init; }
    public required IReadOnlyList<HistoryEntryModel> Entries { get; init; }
}
