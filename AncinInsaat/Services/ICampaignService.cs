using AncinInsaat.Models;

namespace AncinInsaat.Services;

public interface ICampaignService
{
    // Campaigns to show today, in display order. Empty when none are left.
    Task<IReadOnlyList<Campaign>> GetVisibleCampaignsAsync(CancellationToken cancellationToken = default);
}
