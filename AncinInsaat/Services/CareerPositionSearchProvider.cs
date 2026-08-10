using AncinInsaat.Models;

namespace AncinInsaat.Services;

// Every published CareerPosition as a searchable entry — same
// "read through the query service" precedent as ProjectSearchProvider.
// The Career page has no per-position anchor to deep-link to (positions
// render as one flat card list, docs/03_PageBlueprints.md), so every
// result points at /career itself, same destination as the static
// "Kariyer" entry StaticPageSearchProvider already contributes. SortOrder
// 41+ sits directly after that static Kariyer entry (40) and before
// İletişim (50).
public class CareerPositionSearchProvider : ISearchIndexProvider
{
    private const int BaseSortOrder = 41;

    private readonly ICareerPositionQueryService _careerPositionQueryService;

    public CareerPositionSearchProvider(ICareerPositionQueryService careerPositionQueryService)
    {
        _careerPositionQueryService = careerPositionQueryService;
    }

    public async Task<IReadOnlyList<SearchResultItem>> GetItemsAsync(CancellationToken cancellationToken = default)
    {
        var positions = await _careerPositionQueryService.GetPublishedAsync(cancellationToken);

        return positions
            .Select((position, index) => new SearchResultItem
            {
                Title = position.Title,
                Description = string.IsNullOrWhiteSpace(position.Description)
                    ? "Açık pozisyon detaylarını görüntüleyin."
                    : position.Description,
                Url = "/career",
                Category = "Kariyer",
                Keywords = string.Join(' ', new[] { position.Department, position.Location }
                    .Where(part => !string.IsNullOrWhiteSpace(part))),
                SortOrder = BaseSortOrder + index
            })
            .ToList();
    }
}
