namespace AncinInsaat.Models;

// One entry per distinct category present in a project's gallery (Gallery
// Category Cards redesign, 2026-08-04). Value is the raw ProjectImage.Category
// value — used to match GalleryImageModel.Category and as the Media Viewer
// group key, so it must stay stable. Label is the Turkish text shown in the
// category dropdown and on every card in that category, kept separate from
// Value so a future English/localised category value never leaks into the
// UI unpolished (falls back to Value itself when no translation is known —
// see ProjectsController.GalleryCategoryLabels).
public class GalleryCategoryModel
{
    public required string Value { get; init; }
    public required string Label { get; init; }
}
