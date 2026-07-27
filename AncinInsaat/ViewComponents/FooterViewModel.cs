namespace AncinInsaat.ViewComponents;

public class FooterLinkItem
{
    public required string Label { get; init; }
    public required string Url { get; init; }
}

public class FooterViewModel
{
    public required string CompanyTagline { get; init; }
    public required string Address { get; init; }
    public required string Phone { get; init; }
    public required string PhoneHref { get; init; }
    public required string Email { get; init; }
    public required IReadOnlyList<FooterLinkItem> CorporateLinks { get; init; }
    public required IReadOnlyList<FooterLinkItem> PeopleFirstLinks { get; init; }
    public required IReadOnlyList<FooterLinkItem> ExploreLinks { get; init; }
    public int CopyrightYear { get; init; }
}
