namespace AncinInsaat.Models;

// View model for Views/Contact/Index.cshtml — assembled entirely in
// ContactController (GET and the invalid-submission POST path both build
// one), same "controller shapes entities into a view model" convention as
// ProjectDetailViewModel/ProjectsIndexViewModel.
public class ContactPageViewModel
{
    public required ContactFormViewModel Form { get; init; }
    public required InformationCardModel InformationCard { get; init; }

    // Null when SiteSettings has no GoogleMaps URL yet — Views/Contact/Index
    // skips the Map section entirely rather than rendering an empty iframe.
    public string? MapEmbedUrl { get; init; }
    public string? MapLinkUrl { get; init; }

    // Set from TempData after a successful POST-redirect-GET — the view
    // renders _SuccessMessage in place of the form when true.
    public bool ShowSuccess { get; init; }
}
