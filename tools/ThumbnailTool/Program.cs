using AncinInsaat.ImagePipeline;

// One-command thumbnail generation for the project image architecture
// (docs/07_AssetStructure.md). Walks every "originals" folder under
// wwwroot/images/projects and creates a matching WebP thumbnail in its
// sibling "thumbnails" folder.
//
// Behavior:
//   - A missing thumbnail is always generated.
//   - An existing thumbnail is regenerated automatically if its original
//     has a newer last-write time (i.e. the original was replaced with a
//     different photo at the same path) — no flag needed for this case.
//   - An up-to-date thumbnail is left alone, so re-running this tool after
//     adding a handful of new photos to a project only costs work for those
//     new photos, not the whole tree.
//   - Every generated/regenerated thumbnail has its EXIF metadata (camera
//     info, capture time, GPS coordinates) stripped by
//     AncinInsaat.ImagePipeline.ThumbnailGenerator — see that class for
//     details.
//
// Usage:
//   dotnet run --project tools/ThumbnailTool -- [path] [--force]
//   dotnet run --project tools/ThumbnailTool -- --single <source> <dest> [--width N] [--quality N]
//
//   [path]     Optional. Defaults to the repo's own wwwroot/images/projects.
//   --force    Rebuild every thumbnail, ignoring both existence and
//              timestamp checks. Use this after changing DefaultWidth/
//              DefaultQuality in ThumbnailGenerator, or if a thumbnail was
//              hand-edited and needs to be regenerated from its original.
//   --single   Generates exactly one thumbnail at the given source/
//              destination paths, bypassing the originals/thumbnails
//              folder-walk entirely — for a one-off asset (e.g. a project's
//              dedicated cover.webp) that wants a different width/quality
//              than DefaultWidth/DefaultQuality without touching those
//              (and therefore every other thumbnail on the site).
//              --width/--quality default to DefaultWidth/DefaultQuality
//              when omitted.
//
// Examples:
//   dotnet run --project tools/ThumbnailTool
//   dotnet run --project tools/ThumbnailTool -- --force
//   dotnet run --project tools/ThumbnailTool -- /path/to/wwwroot/images/projects --force
//   dotnet run --project tools/ThumbnailTool -- --single wwwroot/images/projects/x/gallery/originals/photo.jpg wwwroot/images/projects/x/cover.webp --width 1000 --quality 88

if (args.Any(a => string.Equals(a, "--single", StringComparison.OrdinalIgnoreCase)))
{
    var singleArgs = args.Where(a => !string.Equals(a, "--single", StringComparison.OrdinalIgnoreCase)).ToArray();
    var singlePositional = new List<string>();
    var width = AncinInsaat.ImagePipeline.ThumbnailGenerator.DefaultWidth;
    var quality = AncinInsaat.ImagePipeline.ThumbnailGenerator.DefaultQuality;

    for (var i = 0; i < singleArgs.Length; i++)
    {
        if (string.Equals(singleArgs[i], "--width", StringComparison.OrdinalIgnoreCase) && i + 1 < singleArgs.Length)
        {
            width = int.Parse(singleArgs[++i]);
        }
        else if (string.Equals(singleArgs[i], "--quality", StringComparison.OrdinalIgnoreCase) && i + 1 < singleArgs.Length)
        {
            quality = int.Parse(singleArgs[++i]);
        }
        else
        {
            singlePositional.Add(singleArgs[i]);
        }
    }

    if (singlePositional.Count != 2)
    {
        Console.Error.WriteLine("--single requires exactly <source> <dest> (optionally followed by --width N / --quality N).");
        return 1;
    }

    var singleSource = Path.GetFullPath(singlePositional[0]);
    var singleDest = Path.GetFullPath(singlePositional[1]);

    try
    {
        await AncinInsaat.ImagePipeline.ThumbnailGenerator.GenerateAsync(singleSource, singleDest, width, quality);
        Console.WriteLine($"  generated    {singleDest} ({width}w/{quality}q, from {singleSource})");
        return 0;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"  FAILED       {singleDest} — {ex.Message}");
        return 1;
    }
}

var force = args.Any(a => string.Equals(a, "--force", StringComparison.OrdinalIgnoreCase));
var positionalArgs = args.Where(a => !a.StartsWith("--", StringComparison.Ordinal)).ToArray();

var projectsRoot = positionalArgs.Length > 0
    ? positionalArgs[0]
    : Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "AncinInsaat", "wwwroot", "images", "projects");

projectsRoot = Path.GetFullPath(projectsRoot);

if (!Directory.Exists(projectsRoot))
{
    Console.Error.WriteLine($"Projects root not found: {projectsRoot}");
    return 1;
}

Console.WriteLine(force
    ? $"Scanning {projectsRoot} for originals/ folders (--force: rebuilding every thumbnail)..."
    : $"Scanning {projectsRoot} for originals/ folders...");

var generated = 0;
var regenerated = 0;
var skipped = 0;
var failed = 0;

var originalsFolders = Directory.EnumerateDirectories(projectsRoot, "originals", SearchOption.AllDirectories);

foreach (var originalsFolder in originalsFolders)
{
    var thumbnailsFolder = Path.Combine(originalsFolder, "..", "thumbnails");

    foreach (var sourcePath in Directory.EnumerateFiles(originalsFolder))
    {
        var thumbnailFileName = Path.ChangeExtension(Path.GetFileName(sourcePath), ".webp");
        var destinationPath = Path.Combine(thumbnailsFolder, thumbnailFileName);

        var thumbnailExists = File.Exists(destinationPath);
        var originalIsNewer = thumbnailExists
            && File.GetLastWriteTimeUtc(sourcePath) > File.GetLastWriteTimeUtc(destinationPath);

        if (thumbnailExists && !force && !originalIsNewer)
        {
            skipped++;
            continue;
        }

        try
        {
            await ThumbnailGenerator.GenerateAsync(sourcePath, destinationPath);

            if (!thumbnailExists)
            {
                generated++;
                Console.WriteLine($"  generated    {Path.GetRelativePath(projectsRoot, destinationPath)}");
            }
            else
            {
                regenerated++;
                var reason = force ? "--force" : "original is newer";
                Console.WriteLine($"  regenerated  {Path.GetRelativePath(projectsRoot, destinationPath)} ({reason})");
            }
        }
        catch (Exception ex)
        {
            failed++;
            Console.Error.WriteLine($"  FAILED       {Path.GetRelativePath(projectsRoot, sourcePath)} — {ex.Message}");
        }
    }
}

Console.WriteLine($"Done. {generated} generated, {regenerated} regenerated, {skipped} already up to date, {failed} failed.");
return failed > 0 ? 1 : 0;
