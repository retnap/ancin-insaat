namespace AncinInsaat.Data.Entities;

// Structured Location & Distances data (Project Detail redesign,
// 2026-07-31) — a dedicated entity rather than a freeform text field like
// Amenities, since Name/Distance are two distinct attributes of one row,
// not a single line of prose.
public class ProjectNearbyPlace
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required string Name { get; set; }
    public required string Distance { get; set; }
    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
}
