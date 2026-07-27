namespace AncinInsaat.Data.Entities;

public class CareerPosition
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}
