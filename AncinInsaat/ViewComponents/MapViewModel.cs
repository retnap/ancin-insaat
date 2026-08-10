namespace AncinInsaat.ViewComponents;

public class MapViewModel
{
    // Null renders the static placeholder illustration (current state —
    // no Google Maps Embed API key/signed embed URL exists yet, see
    // docs/14_Decisions.md). A future real embed URL (API key-based, or a
    // signed "Share → Embed a map" URL from Google Maps) only needs to be
    // passed in here — the component itself does not change.
    public string? EmbedUrl { get; init; }

    public string? LinkUrl { get; init; }
}
