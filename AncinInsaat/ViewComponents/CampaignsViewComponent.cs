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
                Title = isEnglish ? "Celebrating 50 Years with Exclusive Offers" : "50. Yılımıza Özel Kampanyalar"
            },
            Cards = campaigns.Select(campaign =>
            {
                var name = ProjectDisplayText.Name(campaign.Name, isEnglish);

                return new CampaignCardModel
                {
                    Eyebrow = isEnglish ? "EXCLUSIVE 50TH ANNIVERSARY OFFER" : "50. YIL KAMPANYASI",
                    Name = name,
                    // A campaign without a project page has no location line.
                    Location = campaign.ProjectSlug is null
                        ? null
                        : ProjectDisplayText.Location(locationsBySlug.GetValueOrDefault(campaign.ProjectSlug), isEnglish),
                    WatchLabel = isEnglish ? "Watch →" : "İzle →",
                    Key = campaign.ProjectSlug ?? $"campaign-{campaign.Order}",
                    VideoSrc = campaign.VideoPath,
                    PosterSrc = campaign.PosterPath,
                    ProjectUrl = campaign.ProjectSlug is null ? null : $"{pathPrefix}/projects/{campaign.ProjectSlug}",
                    ComingSoonMessage = campaign.ProjectSlug is null
                        ? (isEnglish ? "Details coming soon." : "Detaylar çok yakında.")
                        : null,
                    PlayLabel = isEnglish
                        ? $"Watch the {name} offer video"
                        : $"{name} kampanya videosunu izle",
                    VideoAlt = isEnglish
                        ? $"{name} offer video"
                        : $"{name} kampanya videosu"
                };
            }).ToList(),
            PreviousLabel = isEnglish ? "Previous offers" : "Önceki kampanyalar",
            NextLabel = isEnglish ? "Next offers" : "Sonraki kampanyalar",
            ListLabel = isEnglish ? "Offers, scrollable" : "Kampanyalar, kaydırılabilir"
        };

        return View(model);
    }
}
