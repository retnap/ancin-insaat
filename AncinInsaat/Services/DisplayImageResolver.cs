namespace AncinInsaat.Services;

// Image performance pass (2026-10-26) — every lightbox image, banner and
// in-page image without a thumbnail is served as a screen-sized "display"
// WebP (AncinInsaat.ImagePipeline.DisplayImageGenerator: 1920px/q80, or
// 2560px/q85 for banners and full-width images) instead of its multi-MB
// original. Originals are never modified. No display file is kept where the
// WebP would be larger than the original (e.g. small pre-optimized JPEGs),
// so Resolve falls back to the original — same fail-closed contract as
// ProjectsController.ResolveThumbnail. Pure path derivation, no schema
// field, so the future Admin Panel only has to generate the file.
public class DisplayImageResolver : IDisplayImageResolver
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DisplayImageResolver(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Resolve(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return path;
        }

        var displayPath = ImagePathHelper.GetDisplayPath(path);
        return Exists(displayPath) ? displayPath : path;
    }

    public bool Exists(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        var relativePath = path.TrimStart('~').TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        return File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, relativePath));
    }
}
