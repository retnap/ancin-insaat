using AncinInsaat.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AncinInsaat.Models;

// View model for Views/Career/Index.cshtml — assembled entirely in
// CareerController, same "controller shapes entities into a view model"
// convention as ContactPageViewModel.
public class CareerPageViewModel
{
    public required CareerFormViewModel Form { get; init; }

    // Published positions as-fetched — passed straight through to the
    // JobPostingStructuredData component (Component.InvokeAsync("JobPostingStructuredData",
    // new { positions = Model.Positions })) since that component needs the
    // full entities (Description, CreatedAt, ...), not the flattened
    // Information Card/select shapes below.
    public required IReadOnlyList<CareerPosition> Positions { get; init; }

    // Open Positions section — Information Card, Grid layout (reused as-is,
    // same as AreasOfExpertise/MissionVision). Items is empty when there
    // are no published positions; the view renders an empty-state message
    // in that case instead of an empty grid.
    public required InformationCardModel OpenPositions { get; init; }

    // The Application Form's position <select> — built from the same
    // published CareerPosition rows as OpenPositions, so a candidate can
    // only ever submit against a position that is actually open.
    public required IReadOnlyList<SelectListItem> PositionOptions { get; init; }

    // Set from TempData after a successful POST-redirect-GET — the view
    // renders _SuccessMessage in place of the form when true.
    public bool ShowSuccess { get; init; }
}
