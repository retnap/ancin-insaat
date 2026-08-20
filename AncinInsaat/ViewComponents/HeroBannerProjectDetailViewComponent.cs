using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Third Hero Banner variant (docs/04_ComponentLibrary.md), alongside Home
// and Standard — Project Detail redesign, 2026-07-31. Full-viewport and
// bottom-anchored like Home's hero (reuses .hero/.hero-bg as-is, including
// the existing cinematic zoom), but page-agnostic content passed straight
// from ProjectsController.Details rather than resolved from
// IProjectQueryService, matching HeroBannerStandardViewComponent's
// precedent. Neither Home's variant (data-driven off the latest featured
// project only) nor Standard's (explicitly no Project CTA / Scroll
// Indicator, per its own decision record) fit a per-project hero with a
// Catalogue button, a Contact button and a scroll indicator, so this is a
// new component rather than a mode flag on either.
//
// Originally two markup branches gated on slug == "nysa-gold" (its own
// left-aligned reference redesign vs. every other project's centered
// original). Client approved the Nysa Gold architecture as the Hero for
// every Project Detail page (2026-08-08), so Default.cshtml is now a
// single unconditional layout — the one per-project variance left is
// typography, resolved here from Slug via ProjectHeroFontMap (also reused
// by HeroBannerViewComponent for the Home Hero, 2026-08-10), matching each
// project's own logo wordmark. A slug with no entry (e.g. Ferhunde Hanım
// Apt., La Via AVM — outside the client's font brief) renders no modifier
// class and falls back to .hero-heading's own default.
public class HeroBannerProjectDetailViewComponent : ViewComponent
{
    // Nysa Gold Residence real logo lockup (2026-08-20 client-supplied
    // asset, replaced same-day with the client's revised file — "nysa gold
    // logo_last.png") — renders as an <img> in place of the styled text
    // heading for this slug only (Default.cshtml). Every other project
    // keeps the real-HTML-text heading described above; a slug with no
    // entry here falls back to that text path unconditionally.
    private static readonly IReadOnlyDictionary<string, string> HeadingLogoImageUrls =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["nysa-gold"] = "/images/projects/nysa-gold/banner/nysa gold logo_last.png",
            // Le Jardin client-supplied logo lockup (2026-08-20 request) —
            // same treatment as Nysa Gold above, reusing the exact same
            // generic .hero-heading-logo-img sizing/positioning (no CSS
            // change needed for this slug).
            ["le-jardin"] = "/images/projects/le-jardin/banner/le jardin logo.png",
            // La Fiore Karabağ 1. Etap, La Via Villalar 1. Etap and D-Latis
            // client-supplied logo lockups (2026-08-20 request) — same
            // treatment as above. D-Latis's file is tightly trimmed like
            // Nysa Gold's (no left-edge correction needed); the other two
            // have baked-in transparent padding corrected in Default.cshtml
            // via their own hero-heading-logo-img modifier class (see
            // site.css).
            ["la-fiore-karabag"] = "/images/projects/la-fiore-karabag/banner/lafiore  logo.png",
            ["kuyulu-la-via-villalar-birinci-etap"] = "/images/projects/kuyulu-la-via-villalar-birinci-etap/banner/lavia  logo.png",
            ["davutlar-d-latis"] = "/images/projects/davutlar-d-latis/banner/dlatis logo.png",
            // Tralles Gold Residence, Alinda Gold Residence, Magnesia Gold
            // Residence and Nlatis client-supplied logo lockups (2026-08-20
            // request) — same treatment as above. All four have their own
            // baked-in transparent padding, corrected in Default.cshtml via
            // their own hero-heading-logo-img modifier class (see site.css).
            ["tralles-gold"] = "/images/projects/tralles-gold/banner/tralles gold logo.png",
            ["alinda-gold"] = "/images/projects/alinda-gold/banner/alinda logo.png",
            ["magnesia-gold"] = "/images/projects/magnesia-gold/banner/magnesia gold logo.png",
            ["nlatis"] = "/images/projects/nlatis/banner/nlatis logo.png",
            // La Fiore Karabağ 2. Etap client-supplied logo lockup
            // (2026-08-20 request) — same treatment as above. The file is
            // pixel-identical in size (1316x295) and transparent-padding
            // bbox to La Fiore Karabağ 1. Etap's own logo above, so it
            // reuses that project's hero-heading-logo-img modifier class
            // in Default.cshtml rather than needing its own.
            ["la-fiore-karabag-2-etap"] = "/images/projects/la-fiore-karabag-2-etap/banner/lafiore -ikinci-logo.png"
        };

    // La Fiore Karabağ 2. Etap "2-etap.png" badge (client-supplied asset,
    // 2026-08-20 request) — rendered next to HeadingLogoImageUrl above so
    // the Hero heading clearly reads as "LA FIORE  2. ETAP" rather than
    // the generic La Fiore Karabağ 1. Etap wordmark alone. No other slug
    // has an entry, so every other project's heading is unaffected.
    private static readonly IReadOnlyDictionary<string, string> HeadingBadgeImageUrls =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["la-fiore-karabag-2-etap"] = "/images/projects/la-fiore-karabag-2-etap/banner/2-etap.png"
        };

    // Q-Latis Hero heading text override (2026-08-20 client request) — the
    // project's own Name ("Hacıfeyzullah - Q-Latis", ProjectsController's
    // heading argument) is the record's full/legal name used everywhere
    // else on the page (title, breadcrumb, SEO); the client wants only the
    // brand-facing "Q-LATIS" in the Hero itself. A slug with no entry here
    // renders its own Name unchanged, exactly as before.
    private static readonly IReadOnlyDictionary<string, string> HeadingTextOverrides =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["q-latis"] = "Q-Latis"
        };

    public IViewComponentResult Invoke(
        string slug,
        string heading,
        string? subheading,
        string backgroundImageUrl,
        string? statusLabel,
        string? statusModifierClass,
        string? catalogueUrl,
        bool catalogueComingSoonHeroToast,
        IReadOnlyList<string> sitePlanImageUrls,
        bool sitePlanComingSoon,
        string scrollTargetId)
    {
        var model = new HeroBannerProjectDetailViewModel
        {
            Slug = slug,
            Heading = HeadingTextOverrides.GetValueOrDefault(slug, heading),
            Subheading = subheading,
            BackgroundImageUrl = backgroundImageUrl,
            HeadingFontModifierClass = ProjectHeroFontMap.HeadingFontModifierClasses.GetValueOrDefault(slug),
            HeadingLogoImageUrl = HeadingLogoImageUrls.GetValueOrDefault(slug),
            HeadingBadgeImageUrl = HeadingBadgeImageUrls.GetValueOrDefault(slug),
            StatusLabel = statusLabel,
            StatusModifierClass = statusModifierClass,
            CatalogueUrl = catalogueUrl,
            CatalogueComingSoonHeroToast = catalogueComingSoonHeroToast,
            SitePlanImageUrls = sitePlanImageUrls,
            SitePlanComingSoon = sitePlanComingSoon,
            ScrollTargetId = scrollTargetId
        };

        return View(model);
    }
}
