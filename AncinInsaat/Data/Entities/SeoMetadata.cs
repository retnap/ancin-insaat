namespace AncinInsaat.Data.Entities;

public class SeoMetadata
{
    public int Id { get; set; }
    public required string Page { get; set; }
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string CanonicalUrl { get; set; } = string.Empty;
    public string OpenGraphImage { get; set; } = string.Empty;
}
