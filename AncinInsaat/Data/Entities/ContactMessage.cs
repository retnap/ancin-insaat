namespace AncinInsaat.Data.Entities;

public class ContactMessage
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Subject { get; set; }
    public required string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
