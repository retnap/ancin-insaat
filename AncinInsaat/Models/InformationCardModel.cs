namespace AncinInsaat.Models;

// Views/Shared/_InformationCard.cshtml — the "Information Card" component
// (docs/04_ComponentLibrary.md, Icon variant), first used by Contact
// Information but kept generic ("Corporate pages, Contact, HR Policy" per
// the same doc) rather than Contact-specific.
//
// Layout.Grid (About Us Mission & Vision / Areas of Expertise phase,
// 2026-08-01) reuses the exact same Items shape — icon + heading (Label) +
// description (Value) — but renders each item as its own standalone card
// inside the site's existing .grid/.grid-cols-* utility (Section 1 of
// site.css) instead of List's single wrapping card + row list, so no new
// Mission Card/Vision Card/Expertise Card component is needed.
public class InformationCardModel
{
    public required IReadOnlyList<InformationCardItem> Items { get; init; }
    public InformationCardLayout Layout { get; init; } = InformationCardLayout.List;

    // Grid layout only — fed straight into the .grid-cols-N utility class,
    // which only defines 2/3/4. List layout ignores this.
    public int Columns { get; init; } = 3;
}

public enum InformationCardLayout
{
    List,
    Grid
}

public class InformationCardItem
{
    // Raw <svg> inner markup (path/rect/circle elements using
    // currentColor) — same hand-authored-icon convention as
    // SocialMediaViewComponent.IconMarkup, kept to the same single icon
    // library rather than pulling in an icon font/package.
    public required string IconMarkup { get; init; }

    public required string Label { get; init; }
    public required string Value { get; init; }

    // Optional — renders Value as a link (tel:/mailto:) when present,
    // plain text otherwise (e.g. Working Hours has no meaningful href).
    public string? Href { get; init; }

    // Grid layout only — data-reveal direction for this card ("left"/
    // "right"). Empty renders the default fade-up, staggered via the
    // wrapping data-reveal-group (site.css Section 18). List layout
    // ignores this.
    public string Reveal { get; init; } = "";
}
