using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class ValuesIntroductionViewModel
{
    public required SectionHeaderModel SectionHeader { get; init; }
    public required IReadOnlyList<string> Paragraphs { get; init; }
}
