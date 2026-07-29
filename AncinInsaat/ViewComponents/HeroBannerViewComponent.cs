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
            Heading = "NYSA GOLD RESIDENCE",
            Subheading = "Ançın İnşaat, güven ve zanaatkârlıkla şekillenen projeleriyle yaşam alanlarını geleceğe taşıyor.",
            PrimaryCtaLabel = "Projelerimizi İnceleyin",
            PrimaryCtaUrl = "/projects",
            SecondaryCtaLabel = "Bize Ulaşın",
            SecondaryCtaUrl = "/contact",

            // Commit 4 (Company Overview) must give its section wrapper
            // id="company-overview" for this anchor to resolve.
            ScrollTargetId = "company-overview",

            BackgroundImageUrl = latestProject is not null
                ? $"/images/projects/{latestProject.Slug}/banner.webp"
                : null,
            ProjectCtaLabel = latestProject is not null ? "Projeye Git" : null,
            ProjectCtaUrl = latestProject is not null ? $"/projects/{latestProject.Slug}" : null
        };

        return View(model);
    }
}
