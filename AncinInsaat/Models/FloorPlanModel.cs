namespace AncinInsaat.Models;

// Floor Plans section (Project Detail Floor Plans redesign, 2026-07-31) —
// one entry per apartment type. Src/ThumbnailSrc are null whenever
// FloorPlan.ImagePath doesn't exist on disk yet (ProjectsController.Details
// existence-checks it, same as GalleryImageModel) — _FloorPlans.cshtml falls
// back to its static premium placeholder in that case, so every project
// without real architectural drawings keeps behaving exactly as before
// (La Fiore Karabağ 2. Etap Floor Plans pilot, 2026-08-06).
public class FloorPlanModel
{
    public required string ApartmentType { get; init; }

    // Null when FloorPlan.NetAreaM2/GrossAreaM2/SalesGrossAreaM2 are all
    // still at their 0 decimal default — i.e. no real area data has been
    // supplied yet (La Via AVM update, 2026-08-09: its 3 floor drawings show
    // dozens of individual unit areas rather than one Net/Gross/Sales-Gross
    // figure per floor, and the client asked not to invent one).
    // _FloorPlans.cshtml omits the stats row entirely in that case. Every
    // other project already has real non-zero figures, so this is a no-op
    // for them.
    public decimal? NetAreaM2 { get; init; }
    public decimal? GrossAreaM2 { get; init; }
    public decimal? SalesGrossAreaM2 { get; init; }
    public required IReadOnlyList<FloorPlanRoomModel> Rooms { get; init; }

    // Full-resolution original — used only by the Media Viewer, same
    // contract as GalleryImageModel.Src.
    public string? Src { get; init; }

    // Lightweight WebP thumbnail — what the panel's own visual actually
    // renders. Same fallback-to-Src contract as GalleryImageModel.ThumbnailSrc.
    public string? ThumbnailSrc { get; init; }
}
