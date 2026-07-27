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
    public DateTime? CompletionDate { get; set; }
    public string CoverImage { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
    public ICollection<FloorPlan> FloorPlans { get; set; } = new List<FloorPlan>();
    public ICollection<Partner> Partners { get; set; } = new List<Partner>();
}
