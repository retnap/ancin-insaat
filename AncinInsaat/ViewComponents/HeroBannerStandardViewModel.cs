namespace AncinInsaat.ViewComponents;

public class HeroBannerStandardViewModel
{
    public required string Heading { get; init; }
    public string? Subheading { get; init; }

    // Null falls back to the same neutral gradient the Home hero uses when
    // it has no featured project — see site.css's --hero-bg-image default.
    public string? BackgroundImageUrl { get; init; }

    // Optional status badge rendered above the heading (Project Detail's
    // "Ongoing"/"Completed" — see docs/03_PageBlueprints.md "Page — Project
    // Detail"). Reuses the same StatusLabel/StatusModifierClass shape as
    // ProjectCardModel rather than inventing a second badge model. Null on
    // every caller that has no status to show (e.g. Projects Listing).
    public string? StatusLabel { get; init; }
    public string? StatusModifierClass { get; init; }
}
