namespace AncinInsaat.Models;

public class VideoShowcaseModel
{
    // Used to build unique ids for the trigger/modal pair, so the same
    // partial can be reused more than once on a single page without
    // colliding aria-controls / element ids (see e.g. Project Detail's
    // future video use, 07_AssetStructure.md "Project Videos").
    public required string Id { get; init; }

    // Accessible name for both the play button (sr-only label) and the
    // opened dialog (aria-label) — not visible copy, so it stays out of
    // the CSS layout entirely.
    public required string Title { get; init; }

    public required string PosterSrc { get; init; }
    public required string PosterAlt { get; init; }

    // Lets a calling section add a layout modifier (e.g. a compact size
    // variant) without this shared partial needing to know about
    // page-specific context — same pattern as SectionHeaderModel.CssClass.
    public string? CssClass { get; init; }

    // Null until a real video file is supplied — the partial renders a
    // clearly-labelled "coming soon" state inside the modal instead of a
    // broken <video> with no source. See CompanyHistorySectionViewComponent.
    public string? VideoSrc { get; init; }
}
