using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace AncinInsaat.ImagePipeline;

// Single source of truth for turning an original project photo into a
// gallery-card thumbnail — used today by the ThumbnailTool console app
// (one-off migration + "drop new originals in, run one command") and
// intended for the future Admin Panel's upload endpoint to call directly, so
// upload-time thumbnail generation never drifts from this batch tool's
// output (07_AssetStructure.md's "Future Admin Compatibility" requirement).
public static class ThumbnailGenerator
{
    // Raised from 480/75 (2026-08-06) — project cards read visibly soft at
    // the 4-column desktop breakpoint (.project-card-media, ~360-400px
    // wide) on 2x displays, since a 480px-wide source can't fill that
    // without upscaling. 800/82 gives enough headroom for retina cards
    // while staying a fraction of a full original's size.
    public const int DefaultWidth = 800;
    public const int DefaultQuality = 82;

    // Resizes to DefaultWidth wide, preserving aspect ratio (never upscales
    // a smaller original), and encodes as WebP at DefaultQuality — "visually
    // identical, lightweight" per the image architecture brief. Width-only
    // resize (no fixed height) because gallery cards crop via CSS
    // object-fit, not by the thumbnail's own aspect ratio.
    //
    // EXIF metadata (camera make/model, capture timestamp, GPS coordinates,
    // etc.) is always stripped from the output — it's dead weight on a
    // gallery-card thumbnail and, for GPS in particular, not something a
    // photo taken at a client's site should carry onto a public web page.
    // The ICC colour profile is left untouched so thumbnails stay visually
    // identical to their originals.
    public static Task GenerateAsync(string sourcePath, string destinationPath, CancellationToken cancellationToken = default)
        => GenerateAsync(sourcePath, destinationPath, DefaultWidth, DefaultQuality, cancellationToken);

    // Overload taking explicit width/quality (ThumbnailTool's --single mode,
    // 2026-08-06) — for a one-off asset that intentionally wants a different
    // setting than the bulk originals/thumbnails default above, e.g. a
    // project's dedicated cover.webp generated at a higher quality than the
    // gallery-thumbnail default without changing DefaultWidth/DefaultQuality
    // (and therefore every other thumbnail) for everyone.
    public static async Task GenerateAsync(string sourcePath, string destinationPath, int width, int quality, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);

        using var image = await Image.LoadAsync(sourcePath, cancellationToken);

        image.Metadata.ExifProfile = null;

        var targetWidth = Math.Min(width, image.Width);
        image.Mutate(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(targetWidth, 0),
            Mode = ResizeMode.Max
        }));

        var encoder = new WebpEncoder { Quality = quality };
        await image.SaveAsync(destinationPath, encoder, cancellationToken);
    }
}
