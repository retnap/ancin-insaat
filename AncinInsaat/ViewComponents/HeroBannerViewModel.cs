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

    // La Fiore Karabağ 2. Etap heading identity match (2026-08-20 request)
    // — when the featured project is this slug, Default.cshtml renders
    // this logo image plus HeadingBadgeImageUrl in a .hero-heading-lockup
    // instead of the plain Heading text, exactly like the Project Detail
    // Hero's own HeadingLogoImageUrl/HeadingBadgeImageUrl pair
    // (HeroBannerProjectDetailViewComponent). Null for every other
    // featured project, whose heading and CTA render exactly as before —
    // see HeroBannerViewComponent for the slug gate.
    public string? HeadingLogoImageUrl { get; init; }
    public string? HeadingBadgeImageUrl { get; init; }
    public string? HeadingLogoImgModifierClass { get; init; }

    // First Vaziyet Planı image for the featured project, if any (Home Hero
    // "Vaziyet Planı" button, 2026-08-20 request). Null omits the button —
    // true for every project other than La Fiore Karabağ 2. Etap today,
    // same as the Project Detail Hero when SitePlanImageUrls is empty.
    public string? SitePlanImageUrl { get; init; }
}
