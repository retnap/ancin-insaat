namespace AncinInsaat.Data.Entities;

// One row per Hero "Vaziyet Planı" (site plan) image (La Fiore Karabağ 2.
// Etap Vaziyet Planı/Concept/Gallery phase, 2026-08-09) — replaces the
// previous single Project.SitePlanImagePath column so a project can offer
// more than one master plan image with Prev/Next in the shared Media
// Viewer. Every project keeps exactly the same single-image behavior as
// before by simply having one row (DisplayOrder = 1).
public class ProjectSitePlanImage
{
    public int Id { get; set; }
    public int ProjectId { get; set; }

    public required string ImagePath { get; set; }
    public string AltText { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
}
