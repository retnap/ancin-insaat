namespace AncinInsaat.Models;

// One entry of Data/campaigns.json — see CampaignService.
public class Campaign
{
    public int Order { get; init; }
    public required string ProjectSlug { get; init; }

    // Card label. Kept here rather than read from Project.Name because a
    // campaign's name can differ from it ("Nysa Gold" vs "Nysa Gold
    // Residence") and project names are never translated, so one value
    // serves both languages.
    public required string Name { get; init; }

    public required string VideoPath { get; init; }
    public required string PosterPath { get; init; }
    public bool Active { get; init; }

    // Last day the campaign is shown (inclusive). Null means no end date.
    public DateOnly? EndDate { get; init; }
}
