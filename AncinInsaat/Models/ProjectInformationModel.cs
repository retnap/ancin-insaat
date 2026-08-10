namespace AncinInsaat.Models;

// Project Information (docs/04_ComponentLibrary.md) — Project Detail's
// summary panel. Only Name and StatusLabel are guaranteed (every project has
// a name and a status); Location and CompletionDateLabel are optional and
// the rendering partial skips their row entirely when absent, per
// 03_PageBlueprints.md's "Only display values that exist."
public class ProjectInformationModel
{
    public required string Name { get; init; }
    public string? Location { get; init; }
    public required string StatusLabel { get; init; }
    public string? CompletionDateLabel { get; init; }
}
