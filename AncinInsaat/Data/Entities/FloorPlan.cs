namespace AncinInsaat.Data.Entities;

public class FloorPlan
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required string Title { get; set; }
    public required string ImagePath { get; set; }
    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
}
