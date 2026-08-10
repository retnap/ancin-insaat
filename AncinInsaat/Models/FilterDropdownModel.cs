namespace AncinInsaat.Models;

// Custom-styled single-select listbox (button + popup), not a native
// <select> — the Projects listing needs full control over the open/close
// animation and per-option hover state (see docs/04_ComponentLibrary.md's
// Filter Dropdown entry). Behaviour is wired in site.js, keyed off
// FilterKey via a data-filter-key attribute.
public class FilterDropdownModel
{
    // e.g. "type" | "location" — matches the data-project-type /
    // data-project-location attribute site.js filters the Projects Grid by.
    public required string FilterKey { get; init; }

    // Label shown on the trigger button before a selection is made, and the
    // permanent "no filter" option at the top of the menu.
    public required string Placeholder { get; init; }

    // Distinct, non-empty values already present in the published projects
    // for this field — never a static list, per the approved decision that
    // an option only appears once real data uses it.
    public required IReadOnlyList<string> Options { get; init; }
}
