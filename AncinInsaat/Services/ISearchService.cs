using AncinInsaat.Models;

namespace AncinInsaat.Services;

// Aggregates every registered ISearchIndexProvider into one ordered index —
// the single surface SearchController (and, through it, the Search
// overlay) depends on. Callers never touch individual providers directly.
public interface ISearchService
{
    Task<IReadOnlyList<SearchResultItem>> GetIndexAsync(CancellationToken cancellationToken = default);
}
