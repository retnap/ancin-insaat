namespace AncinInsaat.Models;

public class SectionHeaderModel
{
    // Small label rendered above the heading (e.g. "HAKKIMIZDA"). Optional —
    // most Section Header usages so far are Title-only or Title+Description
    // (docs/04_ComponentLibrary.md); Eyebrow is an additive third slot for
    // sections that need one, introduced with the Company Overview redesign.
    public string? Eyebrow { get; init; }

    public required string Title { get; init; }
    public string? Description { get; init; }

    // Lets a calling page add a layout modifier (e.g. centering) without
    // the shared partial needing to know about page-specific context.
    public string? CssClass { get; init; }

    // Optional id on the rendered heading so a calling section can point
    // its own aria-labelledby at it (see HeroBanner's aria-labelledby
    // convention). Left null renders no id — Razor omits an attribute
    // outright when its entire value is a null single expression.
    public string? HeadingId { get; init; }
}
