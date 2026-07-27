namespace AncinInsaat.Data.Entities;

public class Partner
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required string Name { get; set; }
    public string Logo { get; set; } = string.Empty;
    public string? Website { get; set; }
    public int DisplayOrder { get; set; }

    public Project Project { get; set; } = null!;
}
