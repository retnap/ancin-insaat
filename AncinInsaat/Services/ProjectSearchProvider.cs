using AncinInsaat.Data.Entities;
using AncinInsaat.Models;

namespace AncinInsaat.Services;

// Every published project as a searchable Project Detail entry — reads
// IProjectQueryService rather than AppDbContext directly, matching every
// other page's read path. SortOrder 21+ keeps project entries directly
// after the Projeler listing (20) and before Kariyer (40); relative order
// between projects follows GetPublishedProjectsAsync's own
// DisplayOrder-based sort.
public class ProjectSearchProvider : ISearchIndexProvider
{
    private const int BaseSortOrder = 21;

    private readonly IProjectQueryService _projectQueryService;

    public ProjectSearchProvider(IProjectQueryService projectQueryService)
    {
        _projectQueryService = projectQueryService;
    }

    public async Task<IReadOnlyList<SearchResultItem>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _projectQueryService.GetPublishedProjectsAsync(cancellationToken);

        return projects
            .Select((project, index) => new SearchResultItem
            {
                Title = project.Name,
                Description = string.IsNullOrWhiteSpace(project.ShortDescription)
                    ? "Proje detaylarını görüntüleyin."
                    : project.ShortDescription,
                Url = $"/projects/{project.Slug}",
                Category = "Projeler",
                // Location/status/type aren't shown on the result card but
                // let a query for e.g. a district or "tamamlandı" surface
                // the right project — same status-label convention as
                // ProjectsController/ProjectsShowcaseViewComponent.
                Keywords = string.Join(' ', new[]
                {
                    project.Location,
                    project.Status == ProjectStatus.Completed ? "Tamamlandı" : "Devam Ediyor",
                    project.ProjectType
                }.Where(part => !string.IsNullOrWhiteSpace(part))),
                SortOrder = BaseSortOrder + index
            })
            .ToList();
    }
}
