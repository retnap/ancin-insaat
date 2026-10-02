namespace AncinInsaat.Data.Entities;

public class CareerPosition
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // English localization (2026-10-02) — nullable companions; null falls
    // back to the Turkish fields above (see CareerPositionQueryService).
    // Location is not duplicated — it already holds a place name (e.g.
    // "Aydın"), which stays as-is in English.
    public string? TitleEn { get; set; }
    public string? DepartmentEn { get; set; }
    public string? DescriptionEn { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
}
