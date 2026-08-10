using System.Text.Json;
using System.Text.Json.Serialization;
using AncinInsaat.Data.Entities;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Career page's JobPosting structured data (docs/03_PageBlueprints.md
// "Page — Career", SEO: "JobPosting Schema (when applicable)"). Deliberately
// separate from SeoStructuredDataViewComponent rather than extending its
// single PageSchema slot — that component renders at most one page-specific
// schema per page (CollectionPage/AboutPage/ContactPage/...), while Career
// needs zero-to-many JobPosting entries, one per published CareerPosition.
// Invoked directly from Views/Career/Index.cshtml alongside (not instead
// of) SeoStructuredData, so both keep rendering independently. Renders
// nothing when there are no published positions, per the approved
// requirement.
public class JobPostingStructuredDataViewComponent : ViewComponent
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly ISiteSettingsService _siteSettingsService;

    public JobPostingStructuredDataViewComponent(ISiteSettingsService siteSettingsService)
    {
        _siteSettingsService = siteSettingsService;
    }

    public async Task<IViewComponentResult> InvokeAsync(IReadOnlyList<CareerPosition> positions)
    {
        var settings = await _siteSettingsService.GetAsync();

        if (settings is null || positions.Count == 0)
        {
            return Content(string.Empty);
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var pageUrl = $"{baseUrl}/career";

        // A JobPosting with no description would fail Google's rich-result
        // validation — skipped defensively rather than emitting invalid
        // markup for a position record missing its copy.
        var jobPostingsJsonLd = positions
            .Where(p => !string.IsNullOrWhiteSpace(p.Description))
            .Select(p => JsonSerializer.Serialize(new JobPostingSchema
            {
                Title = p.Title,
                Description = p.Description,
                DatePosted = p.CreatedAt.ToString("yyyy-MM-dd"),
                Url = pageUrl,
                HiringOrganization = new OrganizationRefSchema
                {
                    Name = settings.CompanyName,
                    SameAs = baseUrl
                },
                JobLocation = new PlaceSchema
                {
                    Address = new PostalAddressSchema
                    {
                        AddressLocality = string.IsNullOrWhiteSpace(p.Location) ? "Aydın" : p.Location,
                        AddressCountry = "TR"
                    }
                }
            }, JsonOptions))
            .ToList();

        if (jobPostingsJsonLd.Count == 0)
        {
            return Content(string.Empty);
        }

        return View(new JobPostingStructuredDataViewModel { JobPostingsJsonLd = jobPostingsJsonLd });
    }

    private class JobPostingSchema
    {
        [JsonPropertyName("@context")]
        public string Context { get; } = "https://schema.org";

        [JsonPropertyName("@type")]
        public string Type { get; } = "JobPosting";

        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string DatePosted { get; init; }
        public required string Url { get; init; }
        public required OrganizationRefSchema HiringOrganization { get; init; }
        public required PlaceSchema JobLocation { get; init; }
    }

    private class OrganizationRefSchema
    {
        [JsonPropertyName("@type")]
        public string Type { get; } = "Organization";

        public required string Name { get; init; }
        public required string SameAs { get; init; }
    }

    private class PlaceSchema
    {
        [JsonPropertyName("@type")]
        public string Type { get; } = "Place";

        public required PostalAddressSchema Address { get; init; }
    }

    private class PostalAddressSchema
    {
        [JsonPropertyName("@type")]
        public string Type { get; } = "PostalAddress";

        public required string AddressLocality { get; init; }
        public required string AddressCountry { get; init; }
    }
}
