namespace AncinInsaat.Models;

// Views/Shared/_SuccessMessage.cshtml — a plain partial (not a
// ViewComponent), same rationale as _ProjectInformation: purely
// presentational, driven entirely by data the calling page already has.
// Introduced for the Contact form's success state; kept generic so the
// future Career form can reuse it (docs/04_ComponentLibrary.md's Success
// Message entry lists both).
public class SuccessMessageModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
}
