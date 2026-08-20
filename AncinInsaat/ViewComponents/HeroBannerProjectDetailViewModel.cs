namespace AncinInsaat.ViewComponents;

public class HeroBannerProjectDetailViewModel
{
    // No longer branches the markup (every project uses the same Hero
    // architecture, 2026-08-08) — kept because HeadingFontModifierClass is
    // resolved from it in the ViewComponent, and it still drives the
    // transparent-navbar-overlap marker in
    // Views/Shared/Components/HeroBannerProjectDetail/Default.cshtml.
    public required string Slug { get; init; }

    public required string Heading { get; init; }
    public string? Subheading { get; init; }
    public required string BackgroundImageUrl { get; init; }

    // CSS modifier class matching this project's own logo wordmark (see
    // HeroBannerProjectDetailViewComponent.HeadingFontModifierClasses).
    // Null for any slug outside the client's font brief — the heading then
    // renders with no modifier and falls back to .hero-heading's default.
    public string? HeadingFontModifierClass { get; init; }

    // Set only for the slugs in HeroBannerProjectDetailViewComponent's
    // HeadingLogoImageUrls map (currently Nysa Gold Residence only). When
    // present, Default.cshtml renders this image in place of Heading's
    // text, and omits Subheading entirely — the client-supplied logo file
    // already carries its own wordmark/subtitle lockup.
    public string? HeadingLogoImageUrl { get; init; }

    // Set only for La Fiore Karabağ 2. Etap (client request, 2026-08-20) —
    // a small secondary badge image ("2-etap.png") rendered beside
    // HeadingLogoImageUrl inside the same <h1> (Default.cshtml's
    // .hero-heading-lockup) so the heading reads as one combined lockup:
    // "LA FIORE  2. ETAP". Null for every other slug, including La Fiore
    // Karabağ 1. Etap, whose logo renders alone exactly as before.
    public string? HeadingBadgeImageUrl { get; init; }

    public string? StatusLabel { get; init; }
    public string? StatusModifierClass { get; init; }

    // Null omits the Katalog button entirely (no CataloguePath, or the file
    // does not exist on disk).
    public string? CatalogueUrl { get; init; }

    // When true, the Katalog button (rendered only when CatalogueUrl is also
    // set) also fires the Catalogue Coming Soon toast on click, in addition
    // to its existing scroll (Alinda Gold Residence revision, 2026-08-10).
    // False for every other project, whose Katalog button stays a plain
    // scroll link exactly as before.
    public bool CatalogueComingSoonHeroToast { get; init; }

    // Empty omits the Vaziyet Planı button entirely — unless
    // SitePlanComingSoon is true, in which case the button still renders,
    // showing a Coming Soon toast instead of opening the Media Viewer
    // (Alinda Gold Residence revision, 2026-08-10). 1 entry (Nysa Gold
    // Residence, Davutlar D Latis) renders a single button exactly as
    // before; 2+ entries (La Fiore Karabağ 2. Etap Vaziyet Planı/Concept/
    // Gallery phase, 2026-08-09) additionally render hidden same-group
    // trigger elements so the Media Viewer's Prev/Next activate across all
    // of them.
    public required IReadOnlyList<string> SitePlanImageUrls { get; init; }

    public bool SitePlanComingSoon { get; init; }

    public required string ScrollTargetId { get; init; }
}
