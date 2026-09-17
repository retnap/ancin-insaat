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

        var model = new HeroBannerViewModel
        {
            Heading = "LA FIORE KARABAĞ 2. ETAP",
            Subheading = "Ancın İnşaat, güven ve zanaatkârlıkla şekillenen projeleriyle yaşam alanlarını geleceğe taşıyor.",
            PrimaryCtaLabel = "Projelerimizi İnceleyin",
            PrimaryCtaUrl = "/projects",
            SecondaryCtaLabel = "Bize Ulaşın",
            SecondaryCtaUrl = "/contact",

            // Commit 4 (Company Overview) must give its section wrapper
            // id="company-overview" for this anchor to resolve.
            ScrollTargetId = "company-overview",

            // Nysa Gold temporarily points at the client's freshly uploaded
            // banner asset (still its original PNG, not yet converted/renamed
            // into the banner.webp slot every other project uses) per the
            // 2026-08-20 request to preview it as-is before optimization.
            // Le Jardin follows the same approach (2026-08-20 request). La
            // Fiore Karabağ 2. Etap was repointed to its newest client-
            // supplied banner photo (2026-09-17 Home Page Banner refresh) —
            // same file ProjectsController's HeroBannerImageOverridesBySlug
            // already uses for this project's own Project Detail hero; the
            // previous "lafiore 2.etap banner deneme.png" stays on disk
            // untouched.
            BackgroundImageUrl = latestProject is not null
                ? latestProject.Slug == "nysa-gold"
                    ? "/images/projects/nysa-gold/banner/nysa gold 4k.png"
                    : latestProject.Slug == "le-jardin"
                        ? "/images/projects/le-jardin/banner/le jardin banner.png"
                        : latestProject.Slug == "la-fiore-karabag-2-etap"
                            ? "/images/projects/la-fiore-karabag-2-etap/banner/la-fiore-karabag-ikinci-yeni-banner-2.jpeg"
                            : $"/images/projects/{latestProject.Slug}/banner.webp"
                : null,
            ProjectCtaLabel = latestProject is not null ? "Projeye Git" : null,
            ProjectCtaUrl = latestProject is not null ? $"/projects/{latestProject.Slug}" : null,
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
