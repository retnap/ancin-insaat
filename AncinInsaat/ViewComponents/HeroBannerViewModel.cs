namespace AncinInsaat.ViewComponents;

public class HeroBannerViewModel
{
    public required string Heading { get; init; }
    public required string Subheading { get; init; }
    public required string PrimaryCtaLabel { get; init; }
    public required string PrimaryCtaUrl { get; init; }
    public required string SecondaryCtaLabel { get; init; }
    public required string SecondaryCtaUrl { get; init; }

    // Id (without '#') of the section the Scroll Indicator scrolls to.
    public required string ScrollTargetId { get; init; }

    // Null when no featured project exists — the hero then falls back
    // to the plain neutral gradient background (site.css) and omits the
    // Project CTA entirely rather than link/point at nothing.
    public string? BackgroundImageUrl { get; init; }
    public string? ProjectCtaLabel { get; init; }
    public string? ProjectCtaUrl { get; init; }

    // Resolved from the featured project's slug via ProjectHeroFontMap —
    // the same map every Project Detail Hero uses (Home Hero title
    // typography match, 2026-08-10). Null for any project outside that map
    // (or when there is no featured project), in which case the heading
    // renders exactly as before.
    public string? HeadingFontModifierClass { get; init; }
}
