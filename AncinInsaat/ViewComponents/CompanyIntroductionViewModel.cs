namespace AncinInsaat.ViewComponents;

public class CompanyIntroductionViewModel
{
    // "about-intro" (before Mission & Vision) or "about-closing" (after it)
    // — see CompanyIntroductionViewComponent. Set explicitly rather than
    // derived in the view since both calls render into the same page and
    // need distinct ids.
    public required string SectionId { get; init; }
    public required IReadOnlyList<string> Paragraphs { get; init; }
}
