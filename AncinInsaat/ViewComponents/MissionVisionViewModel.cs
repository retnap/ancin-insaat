namespace AncinInsaat.ViewComponents;

public class MissionVisionViewModel
{
    // Swapping in the real photo later is a one-line change to this path —
    // see MissionVisionViewComponent's PlaceholderImageUrl comment. The
    // panel's layout (Default.cshtml/site.css) reads this the same way
    // Hero Banner reads BackgroundImageUrl, so no markup change is needed
    // when it's replaced.
    public required string BackgroundImageUrl { get; init; }
    public required string MissionLabel { get; init; }
    public required string MissionText { get; init; }
    public required string VisionLabel { get; init; }
    public required string VisionText { get; init; }
}
