namespace AncinInsaat.Models;

// Location & Distances (Project Detail redesign, 2026-07-31) — one row per
// ProjectNearbyPlace, ordered by DisplayOrder. The section itself only
// renders when a project has at least one of these.
public class NearbyPlaceModel
{
    public required string Name { get; init; }
    public required string Distance { get; init; }
}
