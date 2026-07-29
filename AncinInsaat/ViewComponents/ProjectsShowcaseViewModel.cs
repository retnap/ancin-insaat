using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class ProjectShowcaseCardModel
{
    public required string Name { get; init; }
    public required string CoverImageSrc { get; init; }
    public required string StatusLabel { get; init; }
    public required string StatusModifierClass { get; init; }
    public required string DetailUrl { get; init; }
}

public class ProjectsShowcaseViewModel
{
    public required SectionHeaderModel Header { get; init; }
    public required IReadOnlyList<ProjectShowcaseCardModel> Cards { get; init; }
}
