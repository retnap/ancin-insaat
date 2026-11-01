using System.Text.Json;
using AncinInsaat.Models;

namespace AncinInsaat.Services;

// Home "Kampanyalar" list (2026-11-01 request). Lives in Data/campaigns.json
// rather than the database because the live server keeps its own database,
// which a deploy never overwrites; the JSON file ships with every publish.
// Read on each call so an edit to the file takes effect without a restart.
public class CampaignService : ICampaignService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    // Turkey has a fixed UTC+3 offset; an endDate means the end of that day
    // for visitors here, whatever time zone the server itself runs in.
    private static readonly TimeSpan TurkeyUtcOffset = TimeSpan.FromHours(3);

    private readonly string _filePath;
    private readonly ILogger<CampaignService> _logger;

    public CampaignService(IWebHostEnvironment webHostEnvironment, ILogger<CampaignService> logger)
    {
        _filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Data", "campaigns.json");
        _logger = logger;
    }

    public async Task<IReadOnlyList<Campaign>> GetVisibleCampaignsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow + TurkeyUtcOffset);
        var campaigns = await ReadCampaignsAsync(cancellationToken);

        // ProjectDetailAvailability — closing a campaign video navigates to
        // the project page, so never show one whose page is switched off.
        return campaigns
            .Where(campaign => campaign.Active
                && (campaign.EndDate is null || campaign.EndDate >= today)
                && ProjectDetailAvailability.IsDetailPageAvailable(campaign.ProjectSlug))
            .OrderBy(campaign => campaign.Order)
            .ToList();
    }

    // A missing or malformed file hides the section rather than taking the
    // whole Home page down with it.
    private async Task<IReadOnlyList<Campaign>> ReadCampaignsAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var stream = File.OpenRead(_filePath);
            return await JsonSerializer.DeserializeAsync<List<Campaign>>(stream, SerializerOptions, cancellationToken) ?? [];
        }
        catch (Exception exception) when (exception is IOException or JsonException)
        {
            _logger.LogError(exception, "Could not read the campaign list from {FilePath}", _filePath);
            return [];
        }
    }
}
