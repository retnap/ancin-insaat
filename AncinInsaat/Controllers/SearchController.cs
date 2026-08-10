using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

// Serves the global Search Service's index as JSON. The Search overlay
// fetches this once (site.js caches it in memory) and filters client-side
// as the visitor types — no per-keystroke request, no external search
// library, per docs/14_Decisions.md (Global Navigation & Search milestone).
[Route("api/search")]
public class SearchController : Controller
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var items = await _searchService.GetIndexAsync(cancellationToken);
        return Json(items);
    }
}
