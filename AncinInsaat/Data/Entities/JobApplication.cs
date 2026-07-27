namespace AncinInsaat.Data.Entities;

public class JobApplication
{
    public int Id { get; set; }
    public int CareerPositionId { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public required string CVPath { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }

    public CareerPosition CareerPosition { get; set; } = null!;
}
