namespace AncinInsaat.Models;

// One entry in the global Search Service index (docs/14_Decisions.md,
// Global Navigation & Search milestone). Deliberately provider-agnostic —
// carries nothing specific to pages or projects — so any future content
// type (Blog, News, Documents, Media) can produce the same shape without
// the Search overlay or ISearchService changing.
public class SearchResultItem
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Url { get; init; }
    public required string Category { get; init; }

    // Extra searchable terms that are never rendered (e.g. a project's
    // location/status, a career position's department) — folded into the
    // client-side match index alongside Title/Description/Category
    // (search.js) so a query can hit a field the result card doesn't show.
    public string Keywords { get; init; } = string.Empty;

    // Determines the index's overall ordering (ISearchService.GetIndexAsync
    // sorts by this ascending) — grouped in ranges of 10 per provider today
    // (Sayfa 0-9, Kurumsal 10-19, Projeler 20-39, Kariyer/İletişim 40-59) so
    // a future provider can slot into a free range without renumbering the
    // existing ones.
    public int SortOrder { get; init; }
}
