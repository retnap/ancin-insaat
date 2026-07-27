namespace AncinInsaat.ViewComponents;

public class BreadcrumbItem
{
    public required string Label { get; init; }

    // Null marks the current page — rendered as plain text with
    // aria-current="page" instead of a link.
    public string? Url { get; init; }
}

public class BreadcrumbViewModel
{
    public required IReadOnlyList<BreadcrumbItem> Items { get; init; }
}
