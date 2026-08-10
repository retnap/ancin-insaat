namespace AncinInsaat.Data.Entities;

public class ProjectImage
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required string ImagePath { get; set; }
    public string AltText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    // Nullable, freeform string rather than an enum so the taxonomy can grow
    // without a migration (Gallery redesign, 2026-07-31) — mirrors
    // Project.ProjectType's precedent. Initial reference values: "Exterior",
    // "Interior", "Social Areas". The Gallery's category dropdown is built
    // from the distinct, non-null values actually present on a project's
    // images, never a static list, so an image with no Category simply
    // never appears in a filtered view (only "Tüm Görseller").
    public string? Category { get; set; }

    // Nullable, freeform strings — Gallery block/apartment-type filter tiers
    // (La Fiore Karabağ 2. Etap pilot, 2026-08-06). Only this project's
    // seeded images populate them today; every other project's images leave
    // both null, so _ProjectGallery.cshtml's Block/Apartment chip rows (only
    // rendered when at least one image on the page carries a value) never
    // appear anywhere else. Not project-scoped in code — any future project
    // adopting this Gallery tier only needs to populate these same columns,
    // same as Category's own precedent.
    public string? Block { get; set; }
    public string? ApartmentType { get; set; }

    // Nullable — when set, this row is a Gallery video card rather than a
    // plain photo (Gallery video support, 2026-08-09). ImagePath still holds
    // a real image (the video's poster frame) and goes through the exact
    // same originals/thumbnails pipeline as any other gallery photo, so the
    // grid card always has a lightweight WebP thumbnail to show; VideoPath
    // is only ever read once the visitor presses Play (see
    // _ProjectGallery.cshtml / site.js's shared video modal, the same one
    // Concept video slides already use). Null on every image-only row,
    // which is every row on every project except where explicitly seeded.
    public string? VideoPath { get; set; }

    public Project Project { get; set; } = null!;
}
