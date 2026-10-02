using System.Globalization;
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
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

        var positions = await _careerPositionQueryService.GetPublishedAsync(cancellationToken);

        return positions
            .Select((position, index) =>
            {
                var title = isEnglish && !string.IsNullOrWhiteSpace(position.TitleEn) ? position.TitleEn : position.Title;
                var description = isEnglish && !string.IsNullOrWhiteSpace(position.DescriptionEn) ? position.DescriptionEn : position.Description;
                var department = isEnglish && !string.IsNullOrWhiteSpace(position.DepartmentEn) ? position.DepartmentEn : position.Department;

                return new SearchResultItem
                {
                    Title = title,
                    Description = string.IsNullOrWhiteSpace(description)
                        ? (isEnglish ? "View this open position's details." : "Açık pozisyon detaylarını görüntüleyin.")
                        : description,
                    Url = isEnglish ? "/en/career" : "/career",
                    Category = isEnglish ? "Career" : "Kariyer",
                    Keywords = string.Join(' ', new[] { department, position.Location }
                        .Where(part => !string.IsNullOrWhiteSpace(part))),
                    SortOrder = BaseSortOrder + index
                };
            })
            .ToList();
    }
}
