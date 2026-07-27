namespace AncinInsaat.Data.Entities;

public class SiteSettings
{
    public int Id { get; set; }
    public required string CompanyName { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string WorkingHours { get; set; } = string.Empty;
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? LinkedIn { get; set; }
    public string? YouTube { get; set; }
    public string? GoogleMaps { get; set; }
    public string Logo { get; set; } = string.Empty;
    public string FooterText { get; set; } = string.Empty;
}
