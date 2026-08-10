using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Real logo images (wwwroot/images/logos/project-logos/), supplied
// 2026-07-28. Confirmed with the client the same day: despite the
// component/folder naming, these are Ançın's own project wordmarks (a
// portfolio trust strip), not third-party partner logos — nothing
// user-facing says "Partners" (see the section's aria-label in
// Default.cshtml). logo_01-removebg-preview is excluded per client
// instruction; it is Ançın's own company mark, not a project. ImageAlt
// transcribes the wordmark text visible in each source file rather than
// inventing a label. Part of the shared layout (renders on every page
// below the CTA Banner), so content here must stay generic rather than
// page-specific.
//
// 2026-08-03: each logo now links to its project detail page. The slug
// isn't duplicated here — ImageAlt is matched against Project.Name via
// IProjectQueryService (the same source ProjectsShowcaseViewComponent
// reads) to build the DetailUrl, so Project stays the single source of
// truth for slugs.
public class PartnerLogosViewComponent : ViewComponent
{
    private static readonly IReadOnlyList<(string ImageSrc, string ImageAlt)> Partners = new List<(string ImageSrc, string ImageAlt)>
    {
        ("/images/logos/project-logos/logo_02-removebg-preview.png", "Nlatis"),
        ("/images/logos/project-logos/logo_03-removebg-preview.png", "Tralles Gold Residence"),
        ("/images/logos/project-logos/logo_04-removebg-preview.png", "Alinda Gold Residence"),
        ("/images/logos/project-logos/logo_05-removebg-preview.png", "Magnesia Gold Residence"),
        ("/images/logos/project-logos/logo_06-removebg-preview.png", "La Fiore Karabağ"),
        ("/images/logos/project-logos/logo_07-removebg-preview.png", "La Fiore Karabağ 2. Etap"),
        ("/images/logos/project-logos/logo_08-removebg-preview.png", "Le Jardin"),
        ("/images/logos/project-logos/logo_09-removebg-preview.png", "La Via Villalar 1. Etap"),
        ("/images/logos/project-logos/logo_10-removebg-preview.png", "Nysa Gold Residence"),
        ("/images/logos/project-logos/logo_11-removebg-preview.png", "Davutlar D Latis")
    };

    private readonly IProjectQueryService _projectQueryService;

    public PartnerLogosViewComponent(IProjectQueryService projectQueryService)
    {
        _projectQueryService = projectQueryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var projects = await _projectQueryService.GetPublishedProjectsAsync();
        var slugsByName = projects.ToDictionary(project => project.Name, project => project.Slug);

        var items = Partners
            .Select(partner => new PartnerLogoItem
            {
                ImageSrc = partner.ImageSrc,
                ImageAlt = partner.ImageAlt,
                DetailUrl = $"/projects/{slugsByName[partner.ImageAlt]}"
            })
            .ToList();

        var model = new PartnerLogosViewModel { Partners = items };
        return View(model);
    }
}
