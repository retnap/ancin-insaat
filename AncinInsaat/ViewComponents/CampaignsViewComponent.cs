using System.Globalization;
using AncinInsaat.Models;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Home page only, between the Hero Banner and Company Overview (2026-11-01
// request). Renders nothing at all when ICampaignService has no campaign to
// show, so the whole section disappears by editing Data/campaigns.json alone.
public class CampaignsViewComponent : ViewComponent
{
    private readonly ICampaignService _campaignService;
    private readonly IProjectQueryService _projectQueryService;

    public CampaignsViewComponent(ICampaignService campaignService, IProjectQueryService projectQueryService)
    {
        _campaignService = campaignService;
        _projectQueryService = projectQueryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var campaigns = await _campaignService.GetVisibleCampaignsAsync();

        if (campaigns.Count == 0)
        {
            return Content(string.Empty);
        }

        var projects = await _projectQueryService.GetPublishedProjectsAsync();
        var locationsBySlug = projects.ToDictionary(project => project.Slug, ProjectDetailAvailability.GetPublicLocation);

        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);
        var pathPrefix = isEnglish ? "/en" : "";

        var model = new CampaignsViewModel
        {
            // Same slots as Company Overview's header: the red-line label on
            // top, the large heading right under it.
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "campaigns-heading",
                Eyebrow = isEnglish ? "CAMPAIGNS" : "KAMPANYALAR",
                Title = isEnglish ? "Our 50th Anniversary Campaigns" : "50. Yılımıza Özel Kampanyalar"
            },
            Cards = campaigns.Select(campaign => new CampaignCardModel
            {
                Eyebrow = isEnglish ? "50TH ANNIVERSARY OFFER" : "50. YIL KAMPANYASI",
                Name = campaign.Name,
                Location = locationsBySlug.GetValueOrDefault(campaign.ProjectSlug),
                WatchLabel = isEnglish ? "Watch →" : "İzle →",
                Slug = campaign.ProjectSlug,
                VideoSrc = campaign.VideoPath,
                PosterSrc = campaign.PosterPath,
                ProjectUrl = $"{pathPrefix}/projects/{campaign.ProjectSlug}",
                PlayLabel = isEnglish
                    ? $"Watch the {campaign.Name} campaign video"
                    : $"{campaign.Name} kampanya videosunu izle",
                VideoAlt = isEnglish
                    ? $"{campaign.Name} campaign video"
                    : $"{campaign.Name} kampanya videosu"
            }).ToList()
        };

        return View(model);
    }
}
