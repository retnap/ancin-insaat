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
            Heading = heading,
            Subheading = subheading,
            BackgroundImageUrl = backgroundImageUrl,
            HeadingFontModifierClass = ProjectHeroFontMap.HeadingFontModifierClasses.GetValueOrDefault(slug),
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
