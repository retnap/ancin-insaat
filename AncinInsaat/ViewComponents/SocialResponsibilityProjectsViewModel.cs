namespace AncinInsaat.ViewComponents;

// One other (non-featured) photo OR video belonging to a Social
// Responsibility project's unified gallery. Only ever rendered as a hidden
// Media Viewer trigger (see SocialResponsibilityProjects/Default.cshtml) —
// never displayed in the page itself, so it carries no ThumbnailSrc, unlike
// GalleryImageModel.
//
// VideoSrc null => a photo; Src is that photo's own lightbox-resolution
// image. VideoSrc set (2026-10-02, unified gallery task) => a video; Src is
// then that video's poster image instead (doubles as both the Media
// Viewer's "photo" src for a non-JS/fallback render and the <video>'s own
// poster — see site.js's render()), and VideoSrc is the optimized MP4 that
// is only ever assigned to the <video> element's src on an explicit Play
// click, never eagerly.
public class SocialResponsibilityGalleryImage
{
    public required string Src { get; init; }
    public required string Alt { get; init; }
    public string? VideoSrc { get; init; }
}

public class SocialResponsibilityProjectItem
{
    public required string SectionId { get; init; }

    // Shared by every data-media-viewer-trigger belonging to this project
    // (featured + hidden siblings) so Prev/Next cycles only within this
    // project's own photos — see site.js's existing, unmodified Media
    // Viewer module.
    public required string GalleryGroupKey { get; init; }

    public required string Heading { get; init; }
    public required IReadOnlyList<string> Paragraphs { get; init; }

    // On-page display image (Featured tier — see sr-optimize script notes
    // in the ViewComponent). Lighter than FeaturedSrc.
    public required string FeaturedThumbnailSrc { get; init; }

    // Full lightbox-resolution version of the same featured photo (Lightbox
    // tier) — what the Media Viewer opens to when the featured image itself
    // is clicked.
    public required string FeaturedSrc { get; init; }

    public required string FeaturedAlt { get; init; }

    // Every other photo, then every video (2026-10-02 — unified gallery:
    // videos are appended after all existing photos, preserving their own
    // original order, never interleaved or reordering the photos
    // themselves), in this project's gallery. Never rendered as a visible
    // <img>/<video> on the page itself — see the "remaining items must not
    // all be displayed as large images" requirement.
    public required IReadOnlyList<SocialResponsibilityGalleryImage> OtherMedia { get; init; }

    public bool ImageOnRight { get; init; }
}

public class SocialResponsibilityProjectsViewModel
{
    public required IReadOnlyList<SocialResponsibilityProjectItem> Projects { get; init; }
}
