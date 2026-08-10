namespace AncinInsaat.Models;

// Social Facilities (Project Detail redesign, 2026-07-31) — one card per
// line of Project.Amenities. ImageSrc is optional: ProjectsController pairs
// each amenity with one of the project's "Social Areas"-categoried gallery
// photos when any exist (cycling through them if there are fewer photos than
// amenity lines), otherwise the card renders text-only rather than a broken
// or unrelated image.
public class SocialFacilityModel
{
    public required string Name { get; init; }
    public string? ImageSrc { get; init; }
}
