using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace AncinInsaat.ImagePipeline;

// Turns an original photo into its "display" version: the WebP the browser
// actually loads in the lightbox, for in-page images without a thumbnail and
// for banners. Originals stay untouched as the archival source; this is the
// web-sized copy (2026-10-26 image performance pass). Sits next to
// ThumbnailGenerator so the future Admin Panel upload endpoint can produce
// both versions from the same code.
public static class DisplayImageGenerator
{
    // Lightbox / in-page images.
    public const int StandardLongEdge = 1920;
    public const int StandardQuality = 80;

    // Banners, hero images and any full-width image — these span the whole
    // viewport on large and 2x screens, so they get more pixels and a
    // slightly higher quality than the standard display version.
    public const int WideLongEdge = 2560;
    public const int WideQuality = 85;

    // Fits the image inside a longEdge x longEdge square (ResizeMode.Max), so
    // the long edge is capped whether the photo is landscape or portrait.
    // Never upscales a smaller original. EXIF is stripped for the same
    // reasons as ThumbnailGenerator; the ICC profile is kept.
    public static async Task GenerateAsync(string sourcePath, string destinationPath, int longEdge, int quality, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);

        using var image = await Image.LoadAsync(sourcePath, cancellationToken);

        image.Metadata.ExifProfile = null;

        if (image.Width > longEdge || image.Height > longEdge)
        {
            image.Mutate(ctx => ctx.Resize(new ResizeOptions
            {
                Size = new Size(longEdge, longEdge),
                Mode = ResizeMode.Max
            }));
        }

        // FileFormat must be explicit: left unset, ImageSharp carries a PNG
        // source's lossless format over and the "WebP" ends up several MB.
        var encoder = new WebpEncoder { FileFormat = WebpFileFormatType.Lossy, Quality = quality };
        await image.SaveAsync(destinationPath, encoder, cancellationToken);
    }
}
