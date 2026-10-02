namespace AncinInsaat.Data.Entities;

public class SiteSettings
{
    public int Id { get; set; }
    public required string CompanyName { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string WorkingHours { get; set; } = string.Empty;

    // English localization (2026-10-02) — nullable companion; null falls
    // back to WorkingHours above. CompanyName/Address/Phone/Email are not
    // duplicated — they are facts (brand name, real address, real contact
    // details), never translated.
    public string? WorkingHoursEn { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? LinkedIn { get; set; }
    public string? YouTube { get; set; }
    public string? GoogleMaps { get; set; }
    public string Logo { get; set; } = string.Empty;
    public string FooterText { get; set; } = string.Empty;

    // English localization (2026-10-02) — nullable companion; null falls
    // back to FooterText above.
    public string? FooterTextEn { get; set; }
}
