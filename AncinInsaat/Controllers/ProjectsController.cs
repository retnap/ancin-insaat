using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

// Only the detail route is implemented so far — the /projects listing page
// (docs/01_SiteMap.md) is a separate, not-yet-requested page. Routing is
// still correct today: every card in the Home Projects Showcase links here.
public class ProjectsController : Controller
{
    private readonly IProjectQueryService _projectQueryService;

    public ProjectsController(IProjectQueryService projectQueryService)
    {
        _projectQueryService = projectQueryService;
    }

    [HttpGet("projects/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var project = await _projectQueryService.GetPublishedProjectBySlugAsync(slug);

        if (project is null)
        {
            return NotFound();
        }

        return View(project);
    }
}
