using AncinInsaat.Models;

namespace AncinInsaat.Services;

// One content type's contribution to the global Search Service index
// (docs/14_Decisions.md, Global Navigation & Search milestone). Every
// registered ISearchIndexProvider is combined by ISearchService — a future
// content type (Blog, News, Documents, Media Library) becomes searchable by
// implementing this interface and registering it in Program.cs, with no
// change to ISearchService, SearchController, or the Search overlay.
public interface ISearchIndexProvider
{
    Task<IReadOnlyList<SearchResultItem>> GetItemsAsync(CancellationToken cancellationToken = default);
}
