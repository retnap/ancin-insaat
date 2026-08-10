namespace AncinInsaat.Models;

// Project Detail page (/projects/{slug}) — built by ProjectsController.Details
// from the existing IProjectQueryService result. Keeps the view free of any
// Data.Entities.Project-specific logic (status label mapping, description
// splitting), matching how ProjectsController.Index already shapes Project
// into ProjectCardModel rather than passing the entity straight to the view.
public class ProjectDetailViewModel
{
    public required string Name { get; init; }
    public required string Slug { get; init; }
    public required string StatusLabel { get; init; }
    public required string StatusModifierClass { get; init; }

    // Convention-based path (/images/projects/{slug}/banner.webp), mirroring
    // HeroBannerViewComponent's Home hero — not existence-checked the way
    // ProjectCardModel's cover image is, since a 404'd CSS background-image
    // degrades silently to the Hero's gradient overlay rather than showing a
    // broken-image icon.
    public required string HeroBackgroundImageUrl { get; init; }

    public string? ShortDescription { get; init; }

    // Description split into paragraphs on blank lines. Empty when the
    // project has no Description yet — the view renders nothing rather than
    // an empty <p>.
    public required IReadOnlyList<string> DescriptionParagraphs { get; init; }

    public required ProjectInformationModel Information { get; init; }

    // Concept section's media — the first existence-filtered gallery image,
    // falling back to the same line-art placeholder ProjectsShowcase already
    // uses for a missing cover, so the section never shows a broken image
    // (Project Detail redesign, 2026-07-31).
    public required string ConceptImageUrl { get; init; }

    // Existence-checked paths to Project.ConceptVideoPath/
    // ConceptVideoPosterPath — both null unless a project has a real concept
    // video (Davutlar D Latis Media phase, 2026-08-09). When set,
    // _ProjectConcept.cshtml renders the poster + an inline, click-to-play
    // <video> instead of its usual exterior-photo carousel.
    public string? ConceptVideoUrl { get; init; }
    public string? ConceptVideoPosterUrl { get; init; }

    // Concept section's carousel slides (Le Jardin Concept carousel
    // generalization, 2026-08-09) — project.ConceptVideos and
    // project.ConceptImages merged into one existence-filtered list, ordered
    // by DisplayOrder across both, so a project's Concept carousel can freely
    // mix video and image slides in any order with no per-project code (see
    // ProjectsController.Details). Empty for every project without any
    // ProjectConceptVideo/ProjectConceptImage rows, in which case
    // _ProjectConcept.cshtml falls back to ConceptVideoUrl/
    // ConceptVideoPosterUrl (single video) or the exterior-photo carousel
    // exactly as before — this is a pure addition, not a replacement, of the
    // original single-media path.
    public required IReadOnlyList<ConceptSlideModel> ConceptSlides { get; init; }

    // Optional richer paragraph for the Concept section, distinct from
    // ShortDescription (Davutlar D Latis Media phase, 2026-08-09). Falls
    // back to ShortDescription, then DescriptionParagraphs[0], exactly as
    // _ProjectConcept.cshtml already did before this field existed.
    public string? ConceptDescription { get; init; }

    // Ordered by ProjectNearbyPlace.DisplayOrder. Empty when the project has
    // none yet — Details.cshtml skips the whole Location & Distances section
    // in that case, since there is nothing to show alongside the location
    // illustration.
    public required IReadOnlyList<NearbyPlaceModel> NearbyPlaces { get; init; }

    // Existence-filtered — see ProjectsController.FileExistsInWebRoot.
    // Empty when the project has no gallery photos yet; the Gallery section
    // still renders in that case, showing its own empty state rather than
    // being skipped, per docs/03_PageBlueprints.md's Gallery section always
    // being part of the page.
    public required IReadOnlyList<GalleryImageModel> GalleryImages { get; init; }

    // Distinct, non-null Category values present in GalleryImages, in
    // display order (Exterior, Interior, Social Areas, then anything else
    // alphabetically — never a static list, matching the Projects listing's
    // Filter Dropdown precedent). The Gallery's category dropdown — and its
    // category cards — render only when this has 2+ entries; a single
    // category (or none) leaves nothing meaningful to filter and the
    // section falls back to a plain image grid (Project Detail redesign,
    // 2026-07-31; Gallery Category Cards redesign, 2026-08-04).
    public required IReadOnlyList<GalleryCategoryModel> GalleryCategories { get; init; }

    // Existence-checked path to Project.CataloguePath, or null when absent —
    // Details.cshtml skips the whole Project Catalogue section in that case.
    public string? CatalogueUrl { get; init; }

    // When true, _ProjectCatalogue.cshtml's button shows a toast instead of
    // downloading CatalogueUrl (La Fiore Karabağ 2. Etap Vaziyet Planı/
    // Concept/Gallery phase, 2026-08-09). False for every other project.
    public required bool CatalogueComingSoon { get; init; }

    // When true, the Hero's "Kataloğu İncele" button also fires the
    // Catalogue Coming Soon toast immediately, in addition to its existing
    // scroll (Alinda Gold Residence revision, 2026-08-10) — see
    // Project.CatalogueComingSoonHeroToast. Always false unless
    // CatalogueComingSoon is also true (ProjectsController.Details ANDs
    // them), so this can never show without CatalogueComingSoon's message
    // being the correct one. False for every other project, which keeps the
    // Hero button as a plain scroll link exactly as before.
    public required bool CatalogueComingSoonHeroToast { get; init; }

    // When true, the Hero's "Vaziyet Planı" button renders even though
    // SitePlanImageUrls is empty, showing a Coming Soon toast instead of the
    // Media Viewer (Alinda Gold Residence revision, 2026-08-10) — see
    // Project.SitePlanComingSoon. False for every other project, which
    // keeps hiding the button entirely exactly as before.
    public required bool SitePlanComingSoon { get; init; }

    // Existence-filtered paths from Project.SitePlanImages, ordered by
    // DisplayOrder — the Hero's "Vaziyet Planı" button only renders when
    // this is non-empty (Nysa Gold reference redesign, 2026-08-07). More
    // than one entry gives the Hero button's Media Viewer group Prev/Next
    // across all of them (La Fiore Karabağ 2. Etap Vaziyet Planı/Concept/
    // Gallery phase, 2026-08-09) — every other project still has exactly
    // one, rendering identically to before.
    public required IReadOnlyList<string> SitePlanImageUrls { get; init; }

    // Existence-checked path to Project.LocationImagePath, or null when
    // absent — _ProjectLocation.cshtml falls back to the shared location
    // illustration in that case (Davutlar D Latis Media phase, 2026-08-09).
    public string? LocationImageUrl { get; init; }

    // One card per line of Project.Amenities, each optionally paired with a
    // "Social Areas" gallery photo (see SocialFacilityModel). Empty when the
    // project has no Amenities yet — Details.cshtml skips the whole Social
    // Facilities section in that case.
    public required IReadOnlyList<SocialFacilityModel> SocialFacilities { get; init; }

    // Existence-filtered, same as GalleryImages. Empty when the project has
    // no real floor plan artwork yet — Details.cshtml skips the whole Floor
    // Plans section in that case rather than showing an empty state, since
    // 03_PageBlueprints.md does not require one there.
    public required IReadOnlyList<FloorPlanModel> FloorPlans { get; init; }
}
