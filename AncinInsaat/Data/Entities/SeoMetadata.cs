namespace AncinInsaat.Data.Entities;

public class SeoMetadata
{
    public int Id { get; set; }
    public required string Page { get; set; }
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;

    // English localization (2026-10-02) — nullable companions; SeoService
    // falls back to the Turkish fields above when null. CanonicalUrl is
    // not duplicated: it is always the Turkish path, and SeoService
    // derives the English canonical from it via LanguageUrlService.
    public string? MetaTitleEn { get; set; }
    public string? MetaDescriptionEn { get; set; }
    public string CanonicalUrl { get; set; } = string.Empty;
    public string OpenGraphImage { get; set; } = string.Empty;
}
