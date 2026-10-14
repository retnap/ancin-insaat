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

    // Optional override for the category picker card's <img> (Tralles Gold
    // Residence client request, 2026-10-08 — "Dış Mekan Görselleri" shows
    // its 13th photo instead of the category's first). Null for every other
    // project/category, in which case _ProjectGallery.cshtml falls back to
    // its existing "first image in this category" lookup exactly as before
    // — see ProjectsController.GalleryCategoryCardImageOverridesBySlug. The
    // category's own underlying image set/order is never touched by this;
    // only which one renders on this one picker card changes.
    public GalleryImageModel? CardImage { get; init; }
}
