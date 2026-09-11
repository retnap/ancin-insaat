namespace AncinInsaat.Models;

// Home page "Zaman Tüneli" card content only — deliberately separate from
// HistoryEntryModel/CompanyHistoryData (shared with About Us's "Our
// Journey") per the 2026-08-19 request to give Home a fixed
// timeline with an in-card "devamı..." / "<- gizle" expand toggle, without
// touching About Us's shared milestones or link-based cards.
public class HomeTimelineEntryModel
{
    public required string Year { get; init; }
    public required string Title { get; init; }
    public required string ImageSrc { get; init; }
    public required string ImageAlt { get; init; }

    // Always visible, collapsed-state text.
    public required string Summary { get; init; }

    // Additional text revealed only when the card is expanded; Summary +
    // Detail together form the entry's complete text.
    public required string Detail { get; init; }
}

public class HomeTimelineCarouselModel
{
    public required string Id { get; init; }
    public required IReadOnlyList<HomeTimelineEntryModel> Entries { get; init; }
}
