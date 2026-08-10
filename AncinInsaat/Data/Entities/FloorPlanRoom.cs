namespace AncinInsaat.Data.Entities;

// One row of the Floor Plans section's room list (e.g. "Salon" — 16.72 m²)
// — always shown in DisplayOrder, which also supplies the list's visible
// numbering (Project Detail Floor Plans redesign, 2026-07-31).
public class FloorPlanRoom
{
    public int Id { get; set; }
    public int FloorPlanId { get; set; }
    public required string Name { get; set; }
    public decimal AreaM2 { get; set; }
    public int DisplayOrder { get; set; }

    public FloorPlan FloorPlan { get; set; } = null!;
}
