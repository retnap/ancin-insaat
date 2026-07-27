namespace AncinInsaat.ViewComponents;

public class SocialMediaLink
{
    public required string Platform { get; init; }
    public required string Url { get; init; }

    // Static, developer-authored SVG shape markup (24x24 viewBox) rendered
    // with @Html.Raw in the view. Never populate this from user input, a
    // database value or any external source — it is trusted only because
    // it is a hardcoded literal below.
    public required string IconMarkup { get; init; }
}

public class SocialMediaViewModel
{
    public required IReadOnlyList<SocialMediaLink> Links { get; init; }
    public bool OnLight { get; init; }
}
