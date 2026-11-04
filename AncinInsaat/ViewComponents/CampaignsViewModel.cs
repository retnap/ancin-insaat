using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class CampaignsViewModel
{
    public required SectionHeaderModel SectionHeader { get; init; }
    public required IReadOnlyList<CampaignCardModel> Cards { get; init; }
    public required string PreviousLabel { get; init; }
    public required string NextLabel { get; init; }
    public required string ListLabel { get; init; }
}

public class CampaignCardModel
{
    public required string Eyebrow { get; init; }
    public required string Name { get; init; }

    // Project.Location; null hides the line.
    public string? Location { get; init; }

    public required string WatchLabel { get; init; }

    // Unique per card; also the Media Viewer group key.
    public required string Key { get; init; }
    public required string VideoSrc { get; init; }
    public required string PosterSrc { get; init; }

    // Exactly one of these is set: the project page the viewer's "View
    // Project" button opens (and closing the viewer goes to), or the message
    // that button shows instead when the campaign has no project page yet.
    public string? ProjectUrl { get; init; }
    public string? ComingSoonMessage { get; init; }

    public required string PlayLabel { get; init; }
    public required string VideoAlt { get; init; }
}
