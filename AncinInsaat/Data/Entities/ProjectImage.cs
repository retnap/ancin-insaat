namespace AncinInsaat.Data.Entities;

public class ProjectImage
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required string ImagePath { get; set; }
    public string AltText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
}
