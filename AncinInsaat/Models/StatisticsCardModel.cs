namespace AncinInsaat.Models;

// Statistics Card (docs/04_ComponentLibrary.md) — a large-number-focused
// sibling to Information Card, not a replacement for it: Information Card
// pairs an icon with a heading/description; Statistics Card pairs one large
// numeric value with a short label, for sections that want to foreground a
// fact/figure (e.g. "53+ Yıllık Tecrübe") rather than an icon+prose card.
// Reuses the exact same .card primitive and .grid/.grid-cols-N utility
// Information Card's Grid layout already established, so it drops into any
// page the same way — first used by HR Policy's Employee Development
// section, intentionally generic for future reuse (e.g. About Us).
public class StatisticsCardModel
{
    public required IReadOnlyList<StatisticItem> Items { get; init; }

    // Fed straight into the .grid-cols-N utility, which only defines 2/3/4 —
    // same convention as InformationCardModel.Columns.
    public int Columns { get; init; } = 3;
}

public class StatisticItem
{
    // The large focal figure, e.g. "53+", "%92", "150+". Free text (not a
    // numeric type) so it can carry its own unit/prefix/suffix without
    // extra formatting logic — matches how "53 Yıllık Tecrübe" was already
    // supplied as a single string for Company Overview.
    public required string Value { get; init; }

    public required string Label { get; init; }

    // Optional — data-reveal direction for this card ("left"/"right"), same
    // convention as InformationCardItem.Reveal. Empty renders the default
    // fade-up, staggered via the wrapping data-reveal-group.
    public string Reveal { get; init; } = "";
}
