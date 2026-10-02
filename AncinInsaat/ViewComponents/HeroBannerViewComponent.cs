using System.Globalization;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Home variant only (docs/04_ComponentLibrary.md also lists a Standard
// variant for other major pages). Add the Standard variant as its own
// model/invocation when the first non-Home page needs it, rather than
// guessing its shape now.
//
// PLACEHOLDER copy — real headline/subheading are not yet confirmed by
// the client. Realistic-but-fictional per CLAUDE.md Placeholder Content
// rules; replace before launch. Hardcoded rather than DB-backed,
// consistent with FooterViewComponent (static content per
// 06_ContentStructure.md).
//
// The background image and "Projeye Git" CTA, however, always reflect
// whichever project IProjectQueryService reports as the latest featured
// project — see GetLatestFeaturedProjectAsync. Promoting the Hero to a
// different project is therefore a data change (DisplayOrder/IsFeatured
// + that project's own /images/projects/{slug}/banner.webp), never a
// change to this component or to site.css.
public class HeroBannerViewComponent : ViewComponent
{
    private readonly IProjectQueryService _projectQueryService;

    public HeroBannerViewComponent(IProjectQueryService projectQueryService)
    {
        _projectQueryService = projectQueryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var latestProject = await _projectQueryService.GetLatestFeaturedProjectAsync();

        // English localization (2026-10-02) — Heading is deliberately NOT
        // translated even in English: it is this project's display name
        // ("La Fiore Karabağ 2. Etap"), and project names/phase suffixes
        // stay untranslated everywhere else on the site (cards, SEO
        // titles, breadcrumbs), so translating it only here would create
        // the exact inconsistency docs/13_DevelopmentRules.md's
        // terminology-consistency rule warns against.
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);
        var pathPrefix = isEnglish ? "/en" : "";

        var model = new HeroBannerViewModel
        {
            Heading = "LA FIORE KARABAĞ 2. ETAP",
            Subheading = isEnglish
                ? "Ançın İnşaat carries living spaces into the future through projects shaped by trust and craftsmanship."
                : "Ancın İnşaat, güven ve zanaatkârlıkla şekillenen projeleriyle yaşam alanlarını geleceğe taşıyor.",
            PrimaryCtaLabel = isEnglish ? "Explore Our Projects" : "Projelerimizi İnceleyin",
            PrimaryCtaUrl = $"{pathPrefix}/projects",
            SecondaryCtaLabel = isEnglish ? "Contact Us" : "Bize Ulaşın",
            SecondaryCtaUrl = $"{pathPrefix}/contact",

            // Commit 4 (Company Overview) must give its section wrapper
            // id="company-overview" for this anchor to resolve.
            ScrollTargetId = "company-overview",

            // Nysa Gold temporarily points at the client's freshly uploaded
            // banner asset (still its original PNG, not yet converted/renamed
            // into the banner.webp slot every other project uses) per the
            // 2026-08-20 request to preview it as-is before optimization.
            // Le Jardin follows the same approach (2026-08-20 request). La
            // Fiore Karabağ 2. Etap (2026-09-28 client request) is repointed
            // at the same copied Gallery photo #36 (banner/la-fiore-2-etap-
            // banner-36.jpeg) now used by this project's own Project Detail
            // Hero (ProjectsController.HeroBannerImageOverridesBySlug) and
            // Home "Devam Eden Projeler" card (ProjectsShowcaseViewComponent.
            // CardImageOverridesBySlug), so all three surfaces stay in sync
            // on the exact same physical asset; the previous "la-fiore-
            // karabag-ikinci-yeni-banner-2.jpeg" stays on disk untouched.
            BackgroundImageUrl = latestProject is not null
                ? latestProject.Slug == "nysa-gold"
                    ? "/images/projects/nysa-gold/banner/nysa gold 4k.png"
                    : latestProject.Slug == "le-jardin"
                        ? "/images/projects/le-jardin/banner/le jardin banner.png"
                        : latestProject.Slug == "la-fiore-karabag-2-etap"
                            ? "/images/projects/la-fiore-karabag-2-etap/banner/la-fiore-2-etap-banner-36.jpeg"
                            : $"/images/projects/{latestProject.Slug}/banner.webp"
                : null,
            ProjectCtaLabel = latestProject is not null ? (isEnglish ? "View Project" : "Projeye Git") : null,
            ProjectCtaUrl = latestProject is not null ? $"{pathPrefix}/projects/{latestProject.Slug}" : null,
            // Mobile Performance & Responsive Pass (2026-10-02) — same
            // ProjectHeroMobileFocusMap the Project Detail Hero resolves
            // from, so the Home Hero crops the latest featured project's own
            // banner identically on narrow viewports. Null today (La Fiore
            // Karabağ 2. Etap's banner is already evenly centered, no entry
            // in the map) — only takes effect if a mapped slug is ever
            // promoted to featured.
            MobileBackgroundPosition = latestProject is not null
                ? ProjectHeroMobileFocusMap.MobileBackgroundPositions.GetValueOrDefault(latestProject.Slug)
                : null,
            HeadingFontModifierClass = latestProject is not null
                ? ProjectHeroFontMap.HeadingFontModifierClasses.GetValueOrDefault(latestProject.Slug)
                : null,

            // La Fiore Karabağ 2. Etap heading identity + 3-button hero
            // actions (2026-08-20 request) — gated to this exact slug so
            // no other featured project's heading or CTA changes if it is
            // ever promoted here instead (Default.cshtml falls back to the
            // plain text heading and single "Projeye Git" link unless
            // HeadingLogoImageUrl is set). Same asset paths as the Project
            // Detail Hero's own HeadingLogoImageUrls/HeadingBadgeImageUrls
            // maps (HeroBannerProjectDetailViewComponent).
            HeadingLogoImageUrl = latestProject?.Slug == "la-fiore-karabag-2-etap"
                ? "/images/projects/la-fiore-karabag-2-etap/banner/lafiore -ikinci-logo.png"
                : null,
            HeadingBadgeImageUrl = latestProject?.Slug == "la-fiore-karabag-2-etap"
                ? "/images/projects/la-fiore-karabag-2-etap/banner/2-etap.png"
                : null,
            HeadingLogoImgModifierClass = latestProject?.Slug == "la-fiore-karabag-2-etap"
                ? "hero-heading-logo-img--la-fiore-karabag"
                : null,
            SitePlanImageUrl = latestProject?.SitePlanImages
                .OrderBy(sitePlan => sitePlan.DisplayOrder)
                .Select(sitePlan => sitePlan.ImagePath)
                .FirstOrDefault()
        };

        return View(model);
    }
}
