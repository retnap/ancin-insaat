using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class HrPhilosophyViewModel
{
    public required SectionHeaderModel SectionHeader { get; init; }
    public required ImageBlockModel Image { get; init; }
    public required IReadOnlyList<string> Paragraphs { get; init; }
}
