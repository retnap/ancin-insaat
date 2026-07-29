namespace AncinInsaat.ViewComponents;

// JSON is serialized in the component, not the view, so the Razor view
// stays render-only (Html.Raw of an already-safe string) — see
// SeoStructuredDataViewComponent for the System.Text.Json call that
// produces these.
public class SeoStructuredDataViewModel
{
    public required string OrganizationJsonLd { get; init; }
    public required string WebSiteJsonLd { get; init; }
}
