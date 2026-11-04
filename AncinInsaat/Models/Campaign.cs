namespace AncinInsaat.Models;

// One entry of Data/campaigns.json — see CampaignService.
public class Campaign
{
    public int Order { get; init; }

    // The project page the campaign belongs to. Null (or left out) for a
    // campaign with no project page on the site yet (Alabanda Gold,
    // 2026-11-03): its video still opens, but the viewer's "View Project"
    // button shows a "Details coming soon" message instead of navigating,
    // and closing the viewer keeps the visitor on the Home page.
    public string? ProjectSlug { get; init; }

    // Card label. Kept here rather than read from Project.Name because a
    // campaign's name can differ from it ("Nysa Gold" vs "Nysa Gold
    // Residence") and a campaign may have no project at all. Written in
    // Turkish; English pages translate only a phase suffix ("1. Etap" →
    // "Phase 1", see ProjectDisplayText).
    public required string Name { get; init; }

    public required string VideoPath { get; init; }
    public required string PosterPath { get; init; }
    public bool Active { get; init; }

    // Last day the campaign is shown (inclusive). Null means no end date.
    public DateOnly? EndDate { get; init; }
}
