namespace AncinInsaat.Data.Entities;

public class FloorPlan
{
    public int Id { get; set; }
    public int ProjectId { get; set; }

    // Apartment type badge shown in the Floor Plans section (e.g. "1+1",
    // "2+1") — replaces the previous free-text Title (Project Detail Floor
    // Plans redesign, 2026-07-31).
    public required string ApartmentType { get; set; }

    // English localization (2026-10-02) — nullable companion; null falls
    // back to ApartmentType above. Most projects use universal notation
    // here ("1+1", "2+1"), which needs no translation and simply leaves
    // this null; a project that repurposes ApartmentType as a Turkish
    // floor label (e.g. "Zemin Kat", "Bodrum Kat" — see
    // DbSeeder.BuildNysaGoldFloorPlans) sets this to the English floor
    // name instead.
    public string? ApartmentTypeEn { get; set; }

    // Reserved for the real floor plan drawing — not rendered yet, since the
    // Floor Plans section currently shows a placeholder until real
    // architectural artwork is supplied (see FloorPlanModel).
    public required string ImagePath { get; set; }

    public decimal NetAreaM2 { get; set; }
    public decimal GrossAreaM2 { get; set; }
    public decimal SalesGrossAreaM2 { get; set; }

    // "Bahçeli Brüt Alan" (garden-inclusive gross area) — a distinct third
    // figure some ground-floor/garden units print alongside Net/Brüt
    // (Nysa Gold floor-plan catalog replacement, 2026-10-08). Nullable and
    // separate from SalesGrossAreaM2, which already means something else
    // ("Satışa Esas Brüt Alan") elsewhere in the project — never reuse one
    // for the other. Null for every floor plan without a printed Bahçeli
    // Brüt figure (every project except Nysa Gold's Zemin Kat units today).
    public decimal? BahceliGrossAreaM2 { get; set; }

    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
    public ICollection<FloorPlanRoom> Rooms { get; set; } = new List<FloorPlanRoom>();
}
