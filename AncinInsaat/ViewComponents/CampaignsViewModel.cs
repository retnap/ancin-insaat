using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class CampaignsViewModel
{
    public required SectionHeaderModel SectionHeader { get; init; }
    public required IReadOnlyList<CampaignCardModel> Cards { get; init; }
}

public class CampaignCardModel
{
    public required string Eyebrow { get; init; }
    public required string Name { get; init; }

    // Project.Location; null hides the line.
    public string? Location { get; init; }

    public required string WatchLabel { get; init; }
    public required string Slug { get; init; }
    public required string VideoSrc { get; init; }
    public required string PosterSrc { get; init; }
    public required string ProjectUrl { get; init; }
    public required string PlayLabel { get; init; }
    public required string VideoAlt { get; init; }
}
