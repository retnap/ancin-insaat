namespace AncinInsaat.Models;

// Project Gallery (docs/04_ComponentLibrary.md's Image Gallery entry) — one
// entry per project image that actually exists on disk. ProjectsController
// filters Project.Images down to this shape so the view never has to check
// file existence itself (see ProjectsController.FileExistsInWebRoot).
public class GalleryImageModel
{
    // Full-resolution original — used only by the Media Viewer/Lightbox,
    // fetched solely on click (see _MediaViewer.cshtml / site.js).
    public required string Src { get; init; }

    // Lightweight WebP thumbnail (docs/07_AssetStructure.md) — what every
    // gallery card's <img> actually renders. Falls back to Src when no
    // thumbnail has been generated yet (ProjectsController.Details), so a
    // project never shows a broken image while its thumbnails are pending.
    public required string ThumbnailSrc { get; init; }

    public required string Alt { get; init; }

    // Nullable — mirrors ProjectImage.Category. Null means the image never
    // matches a category filter option, only "Tüm Görseller" (Gallery
    // redesign, 2026-07-31).
    public string? Category { get; init; }

    // Nullable — mirror ProjectImage.Block/ApartmentType (La Fiore Karabağ
    // 2. Etap Gallery pilot, 2026-08-06). Null on every other project's
    // images, which is what keeps _ProjectGallery.cshtml's Block/Apartment
    // chip rows from ever rendering elsewhere — see ProjectImage.cs.
    public string? Block { get; init; }
    public string? ApartmentType { get; init; }

    // Nullable — mirrors ProjectImage.VideoPath (Gallery video support,
    // 2026-08-09). When set, this card renders as a video: Src/ThumbnailSrc
    // still point at the poster image (existing pipeline, unchanged), and
    // VideoUrl is only read by the shared video modal once Play is pressed
    // — see _ProjectGallery.cshtml / site.js.
    public string? VideoUrl { get; init; }
}
