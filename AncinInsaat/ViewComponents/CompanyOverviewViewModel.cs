using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class CompanyOverviewViewModel
{
    public required SectionHeaderModel SectionHeader { get; init; }
    public required VideoShowcaseModel Video { get; init; }
    public required string CtaLabel { get; init; }
    public required string CtaUrl { get; init; }
}
