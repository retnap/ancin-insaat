using AncinInsaat.Models;

namespace AncinInsaat.ViewComponents;

public class EmployeeDevelopmentViewModel
{
    public required SectionHeaderModel SectionHeader { get; init; }
    public required StatisticsCardModel Statistics { get; init; }
    public required IReadOnlyList<string> Paragraphs { get; init; }
}
