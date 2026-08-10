using AncinInsaat.Models;

namespace AncinInsaat.Services;

public class SearchService : ISearchService
{
    private readonly IEnumerable<ISearchIndexProvider> _providers;

    public SearchService(IEnumerable<ISearchIndexProvider> providers)
    {
        _providers = providers;
    }

    public async Task<IReadOnlyList<SearchResultItem>> GetIndexAsync(CancellationToken cancellationToken = default)
    {
        var items = new List<SearchResultItem>();

        foreach (var provider in _providers)
        {
            items.AddRange(await provider.GetItemsAsync(cancellationToken));
        }

        return items
            .OrderBy(item => item.SortOrder)
            .ToList();
    }
}
