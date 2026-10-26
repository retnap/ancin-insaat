namespace AncinInsaat.Services;

public interface IDisplayImageResolver
{
    // The image's display WebP (ImagePathHelper.GetDisplayPath) when one has
    // been generated, otherwise the path unchanged.
    string Resolve(string path);

    bool Exists(string path);
}
