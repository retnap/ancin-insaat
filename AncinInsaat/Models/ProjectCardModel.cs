namespace AncinInsaat.Models;

// The Project Card "Standard" variant (docs/04_ComponentLibrary.md — the
// other variant, Carousel, is ProjectsShowcaseViewComponent's own
// ProjectShowcaseCardModel, left as-is since that component belongs to the
// completed Home milestone). Rendered via the _ProjectCard partial so any
// future grid/listing reuses the same markup instead of duplicating it.
public class ProjectCardModel
{
    public required string Name { get; init; }
    public required string CoverImageSrc { get; init; }
    public required string StatusLabel { get; init; }
    public required string StatusModifierClass { get; init; }

    // "ongoing" | "completed" — read by the Category Tabs client-side
    // filter (site.js) via a data-project-status attribute on the card's
    // wrapper. Not used by the partial itself.
    public required string StatusFilterValue { get; init; }
    public required string DetailUrl { get; init; }

    // Rendered on the Standard card face and also read by the Projects
    // listing's Location filter dropdown via a data-project-location
    // attribute on the card's wrapper.
    public string? Location { get; init; }

    // Not rendered on the card — read only by the Project Type filter
    // dropdown via a data-project-type attribute on the card's wrapper.
    // Null for a project with no assigned type (see Project.ProjectType).
    public string? ProjectType { get; init; }
}
