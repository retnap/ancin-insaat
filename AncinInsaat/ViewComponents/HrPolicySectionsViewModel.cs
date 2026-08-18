namespace AncinInsaat.ViewComponents;

public class HrPolicySectionItem
{
    public required string Heading { get; init; }
    public required IReadOnlyList<string> Bullets { get; init; }
}

public class HrPolicySectionsViewModel
{
    public required IReadOnlyList<HrPolicySectionItem> Sections { get; init; }
}
