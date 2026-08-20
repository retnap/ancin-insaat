using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class ProjectShowcaseCardModel
{
    public required string Name { get; init; }
    public required string CoverImageSrc { get; init; }
    public required string StatusLabel { get; init; }
    public required string StatusModifierClass { get; init; }
    public required string DetailUrl { get; init; }

    // Set only for the slugs in ProjectsShowcaseViewComponent's
    // CardLogoImageUrlsBySlug map (currently Nysa Gold Residence only).
    // When present, Default.cshtml overlays this image, centered, on top
    // of the card's background image. Null for every other project, which
    // render no such element at all.
    public string? LogoImageUrl { get; init; }
}

public class ProjectsShowcaseViewModel
{
    public required SectionHeaderModel Header { get; init; }
    public required IReadOnlyList<ProjectShowcaseCardModel> Cards { get; init; }
}
