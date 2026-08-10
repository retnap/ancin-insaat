namespace AncinInsaat.Data.Entities;

public class FloorPlan
{
    public int Id { get; set; }
    public int ProjectId { get; set; }

    // Apartment type badge shown in the Floor Plans section (e.g. "1+1",
    // "2+1") — replaces the previous free-text Title (Project Detail Floor
    // Plans redesign, 2026-07-31).
    public required string ApartmentType { get; set; }

    // Reserved for the real floor plan drawing — not rendered yet, since the
    // Floor Plans section currently shows a placeholder until real
    // architectural artwork is supplied (see FloorPlanModel).
    public required string ImagePath { get; set; }

    public decimal NetAreaM2 { get; set; }
    public decimal GrossAreaM2 { get; set; }
    public decimal SalesGrossAreaM2 { get; set; }

    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
    public ICollection<FloorPlanRoom> Rooms { get; set; } = new List<FloorPlanRoom>();
}
