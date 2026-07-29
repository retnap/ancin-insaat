using AncinInsaat.Data.Entities;
using AncinInsaat.Models;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Home page only, directly below Company History — added at the project
// owner's explicit 2026-07-28 request, using Inspirationals/Terzioglu's
// "TAMAMLANAN PROJELER" section for layout/hierarchy inspiration only (not
// its copy: "Hayallerinizi Yaşatır" is intentionally excluded per that
// request). Lists every published project via the same
// IProjectQueryService the Hero Banner already uses, so Project stays the
// single source of truth for project data site-wide rather than a second,
// parallel list — see DbSeeder for the 7 projects added to back this
// section's full carousel-derived list (docs/03_PageBlueprints.md's Home
// "Featured Projects" only anticipated a curated subset; this shows all
// published projects instead, per that request).
public class ProjectsShowcaseViewComponent : ViewComponent
{
    private const string FallbackCoverImage = "/images/projects/project-cover-placeholder.webp";

    private readonly IProjectQueryService _projectQueryService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProjectsShowcaseViewComponent(IProjectQueryService projectQueryService, IWebHostEnvironment webHostEnvironment)
    {
        _projectQueryService = projectQueryService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var projects = await _projectQueryService.GetPublishedProjectsAsync();

        var cards = projects.Select(project => new ProjectShowcaseCardModel
        {
            Name = project.Name,
            CoverImageSrc = ResolveCoverImage(project.CoverImage),
            StatusLabel = project.Status == ProjectStatus.Completed ? "Tamamlandı" : "Devam Ediyor",
            StatusModifierClass = project.Status == ProjectStatus.Completed
                ? "project-status-badge--completed"
                : "project-status-badge--ongoing",
            DetailUrl = $"/projects/{project.Slug}"
        }).ToList();

        var model = new ProjectsShowcaseViewModel
        {
            Header = new SectionHeaderModel
            {
                Eyebrow = "PORTFÖYÜMÜZ",
                Title = "PROJELER",
                HeadingId = "projects-showcase-heading"
            },
            Cards = cards
        };

        return View(model);
    }

    // Falls back to the shared placeholder cover whenever a project's real
    // photo has not been supplied yet (currently every project besides
    // nysa-gold), rather than hardcoding which slugs have real media — once
    // a real /images/projects/{slug}/cover.webp is dropped in, this starts
    // rendering it automatically with no code change.
    private string ResolveCoverImage(string coverImage)
    {
        if (string.IsNullOrWhiteSpace(coverImage))
        {
            return FallbackCoverImage;
        }

        var relativePath = coverImage.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

        return File.Exists(absolutePath) ? coverImage : FallbackCoverImage;
    }
}
