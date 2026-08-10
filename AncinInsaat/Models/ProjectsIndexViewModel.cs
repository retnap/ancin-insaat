namespace AncinInsaat.Models;

// View model for Views/Projects/Index.cshtml — cards plus the two Filter
// Dropdowns' option lists, computed once in ProjectsController from the
// same published-projects query so the page never queries AppDbContext
// directly.
public class ProjectsIndexViewModel
{
    public required IReadOnlyList<ProjectCardModel> Cards { get; init; }
    public required IReadOnlyList<string> LocationOptions { get; init; }
    public required IReadOnlyList<string> ProjectTypeOptions { get; init; }
}
