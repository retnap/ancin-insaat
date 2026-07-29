namespace AncinInsaat.Models;

public class ScrollIndicatorModel
{
    // Id (without '#') of the section this indicator scrolls to.
    public required string TargetId { get; init; }
    public string Label { get; init; } = "Scroll to explore";
    public string? CssClass { get; init; }
}
