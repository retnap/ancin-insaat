namespace AncinInsaat.Data.Entities;

public class Project
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public string Location { get; set; } = string.Empty;

    // Nullable free-text field (not an enum) so the taxonomy can grow without
    // a migration — see 08_DatabasePlan.md. Initial reference values:
    // Residence, Villa, Commercial, Office, Mixed Use. A project with no
    // ProjectType simply does not surface in the Projects listing's
    // Project Type filter until one is assigned.
    public string? ProjectType { get; set; }

    public DateTime? CompletionDate { get; set; }
    public string CoverImage { get; set; } = string.Empty;

    // Nullable and generic on purpose — not every project has a catalogue or
    // a published amenities list, and neither field encodes project-specific
    // logic. CataloguePath points at a downloadable file (see
    // 07_AssetStructure.md's documents/catalogues/ convention); Amenities is
    // freeform text, one item per line, rendered as a list where present.
    public string? CataloguePath { get; set; }

    // When true, _ProjectCatalogue.cshtml's "Proje Kataloğunu İndir" button
    // shows a toast ("Proje kataloğu yakında eklenecektir.") instead of
    // downloading CataloguePath (La Fiore Karabağ 2. Etap Vaziyet Planı/
    // Concept/Gallery phase, 2026-08-09) — CataloguePath itself stays set so
    // switching back to a real download later is a one-flag change. False
    // (default) for every other project, which keeps downloading exactly as
    // before.
    public bool CatalogueComingSoon { get; set; }

    // When true, the Hero's "Kataloğu İncele" button also fires the
    // Catalogue Coming Soon toast immediately on click, in addition to its
    // existing href="#project-catalogue" scroll (Alinda Gold Residence
    // revision, 2026-08-10). Only meaningful alongside CatalogueComingSoon —
    // ProjectsController.Details ANDs the two together defensively so this
    // can never fire a toast with the wrong message. False (default) for
    // every project including La Fiore Karabağ 2. Etap/Kuyulu AVM, which
    // keep their existing scroll-only Hero click behavior unchanged; a
    // project opts in individually rather than this being derived from
    // CatalogueComingSoon alone.
    public bool CatalogueComingSoonHeroToast { get; set; }

    // When true, the Hero's "Vaziyet Planı" button renders even though
    // SitePlanImages is empty, showing a Coming Soon toast instead of
    // opening the Media Viewer (Alinda Gold Residence revision, 2026-08-10).
    // False (default) for every other project, which keeps hiding the
    // button entirely when there is no site plan yet, exactly as before.
    public bool SitePlanComingSoon { get; set; }

    // Concept Video (Davutlar D Latis Media phase, 2026-08-09) — both null
    // for every project without a concept video, in which case
    // _ProjectConcept.cshtml renders its existing exterior-photo carousel
    // exactly as before. When both are set, the Concept media card shows
    // ConceptVideoPosterPath and swaps to an inline <video> (muted-free,
    // preload="none") on Play instead of opening the Media Viewer.
    public string? ConceptVideoPath { get; set; }
    public string? ConceptVideoPosterPath { get; set; }

    // Optional richer paragraph for the Concept section specifically
    // (Davutlar D Latis Media phase, 2026-08-09) — decouples Concept's copy
    // from ShortDescription, which stays reserved for the short Hero
    // subheading. Null for every project without one, in which case
    // _ProjectConcept.cshtml falls back to ShortDescription/Description
    // exactly as before.
    public string? ConceptDescription { get; set; }

    // Optional per-project photo for the right side of the Location &
    // Distances section (Davutlar D Latis Media phase, 2026-08-09). Null
    // for every project without one, in which case _ProjectLocation.cshtml
    // falls back to the shared /images/company/location-illustration.svg
    // exactly as before.
    public string? LocationImagePath { get; set; }

    // Unused since the Catalogue section was standardized on the single
    // _ProjectCatalogue partial across every project (2026-08-06); kept as a
    // column rather than dropped to avoid an unrelated migration. Every
    // project's Catalogue now renders from CataloguePath alone.
    public string? CatalogueTitle { get; set; }

    public string? Amenities { get; set; }

    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
    public ICollection<FloorPlan> FloorPlans { get; set; } = new List<FloorPlan>();
    public ICollection<Partner> Partners { get; set; } = new List<Partner>();
    public ICollection<ProjectNearbyPlace> NearbyPlaces { get; set; } = new List<ProjectNearbyPlace>();

    // Concept section video slides (Davutlar D Latis Concept redesign,
    // 2026-08-09) — see ProjectConceptVideo for how this relates to
    // ConceptVideoPath/ConceptVideoPosterPath above.
    public ICollection<ProjectConceptVideo> ConceptVideos { get; set; } = new List<ProjectConceptVideo>();

    // Concept section image-carousel slides (La Fiore Karabağ 2. Etap
    // Vaziyet Planı/Concept/Gallery phase, 2026-08-09) — the image-only
    // sibling of ConceptVideos, for a project whose Concept carousel has no
    // video asset. See ProjectConceptImage.
    public ICollection<ProjectConceptImage> ConceptImages { get; set; } = new List<ProjectConceptImage>();

    // Hero "Vaziyet Planı" (site plan) images (La Fiore Karabağ 2. Etap
    // Vaziyet Planı/Concept/Gallery phase, 2026-08-09) — replaces the
    // previous single SitePlanImagePath column. Every project prior to this
    // change has exactly one row (DisplayOrder = 1), so its Hero behavior is
    // unchanged; a project can now offer more with Prev/Next in the shared
    // Media Viewer. Empty for every project without a site plan yet.
    public ICollection<ProjectSitePlanImage> SitePlanImages { get; set; } = new List<ProjectSitePlanImage>();
}
