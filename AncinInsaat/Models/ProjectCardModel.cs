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

    // Set only for the slugs in ProjectsController.CardLogoImageUrlsBySlug
    // (currently Nysa Gold Residence only, mirroring
    // ProjectsShowcaseViewComponent's Home-carousel treatment). When
    // present, _ProjectCard.cshtml overlays this image, centered, on top
    // of the card's background image. Null for every other project, which
    // render no such element at all.
    public string? LogoImageUrl { get; init; }

    // Set only for the slugs in ProjectsController.CardTitleOverlayTextBySlug
    // (Ferhunde Hanım Apt. and Q-Latis, 2026-08-20 client request — neither
    // has a logo asset). When present, _ProjectCard.cshtml overlays this
    // text, centered, on top of the card's background image, mirroring
    // LogoImageUrl's own positioning but rendered as styled HTML text
    // instead of an image. Null for every other project.
    public string? TitleOverlayText { get; init; }

    // True only for the slugs in ProjectsController.CardsWithCaptionHidden —
    // the logo or title overlay above already carries the project's
    // branding, so _ProjectCard.cshtml omits the name/location caption
    // entirely for that card. False for every other project, whose caption
    // renders exactly as before.
    public bool HideCaption { get; init; }
}
