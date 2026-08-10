using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class CompanyIntroductionViewModel
{
    public required SectionHeaderModel SectionHeader { get; init; }
    public required ImageBlockModel Image { get; init; }
    public required IReadOnlyList<string> Paragraphs { get; init; }

    // Drives the .company-introduction--text-first modifier (see
    // Default.cshtml/site.css). Default (false) renders Image / Text
    // left-to-right on desktop — About Us's own arrangement, mirrored from
    // Home's Company Overview (text / image) to give the page its own
    // identity while staying visually related. A future request to swap
    // which side each block sits on is a one-line flag flip here, not a
    // markup rewrite — both _ImageBlock and the text block stay exactly
    // as reusable as Company Overview's.
    public bool TextFirst { get; init; }
}
