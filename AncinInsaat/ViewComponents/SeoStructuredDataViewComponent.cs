using System.Text.Json;
using System.Text.Json.Serialization;
using AncinInsaat.Models;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Site-wide Organization + WebSite JSON-LD, invoked once from _Layout so
// every page gets it without repeating the wiring — unlike SeoModel's
// per-page title/description/canonical, this data (company identity) is
// the same on every page, sourced from SiteSettings (docs/14_Decisions.md).
//
// Optionally also renders one page-specific schema (CollectionPage,
// RealEstateListing, ...) when the calling page passes pageSeo +
// pageSchemaType — _Layout reads both from ViewData (the same ViewData a
// controller action already sets for SeoModel) and forwards them here, so
// a new page opts in without this component needing to know about that
// page's own data source. Keeps JSON-LD authoring in this one component
// rather than a second, parallel place per page.
public class SeoStructuredDataViewComponent : ViewComponent
{
    // Default encoder (not UnsafeRelaxedJsonEscaping) so any HTML-sensitive
    // character from a DB field is \u-escaped — this JSON is embedded
    // directly into a <script> tag via Html.Raw.
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        // schema.org properties are camelCase (name, url, streetAddress, ...);
        // explicit [JsonPropertyName] on @context/@type still wins over this.
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly ISiteSettingsService _siteSettingsService;

    public SeoStructuredDataViewComponent(ISiteSettingsService siteSettingsService)
    {
        _siteSettingsService = siteSettingsService;
    }

    public async Task<IViewComponentResult> InvokeAsync(SeoModel? pageSeo = null, string? pageSchemaType = null)
    {
        var settings = await _siteSettingsService.GetAsync();
        if (settings is null)
        {
            return Content(string.Empty);
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var siteUrl = baseUrl + "/";
        var logoUrl = settings.Logo.StartsWith("http", StringComparison.OrdinalIgnoreCase)
            ? settings.Logo
            : $"{baseUrl}/{settings.Logo.TrimStart('/')}";

        var sameAs = new[] { settings.Facebook, settings.Instagram, settings.LinkedIn, settings.YouTube }
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .Select(url => url!)
            .ToArray();

        var organization = new OrganizationSchema
        {
            Name = settings.CompanyName,
            Url = siteUrl,
            Logo = logoUrl,
            Telephone = settings.Phone,
            Email = settings.Email,
            Address = new PostalAddressSchema { StreetAddress = settings.Address },
            SameAs = sameAs
        };

        var website = new WebSiteSchema
        {
            Name = settings.CompanyName,
            Url = siteUrl
        };

        string? pageJsonLd = null;
        if (pageSeo is not null && !string.IsNullOrWhiteSpace(pageSchemaType))
        {
            var pageSchema = new PageSchema
            {
                Type = pageSchemaType,
                Name = pageSeo.Title,
                Description = pageSeo.Description,
                Url = pageSeo.CanonicalUrl
            };

            pageJsonLd = JsonSerializer.Serialize(pageSchema, JsonOptions);
        }

        var model = new SeoStructuredDataViewModel
        {
            OrganizationJsonLd = JsonSerializer.Serialize(organization, JsonOptions),
            WebSiteJsonLd = JsonSerializer.Serialize(website, JsonOptions),
            PageJsonLd = pageJsonLd
        };

        return View(model);
    }

    private class OrganizationSchema
    {
        [JsonPropertyName("@context")]
        public string Context { get; } = "https://schema.org";

        [JsonPropertyName("@type")]
        public string Type { get; } = "Organization";

        public required string Name { get; init; }
        public required string Url { get; init; }
        public required string Logo { get; init; }
        public required string Telephone { get; init; }
        public required string Email { get; init; }
        public required PostalAddressSchema Address { get; init; }
        public required string[] SameAs { get; init; }
    }

    private class PostalAddressSchema
    {
        [JsonPropertyName("@type")]
        public string Type { get; } = "PostalAddress";

        public required string StreetAddress { get; init; }
    }

    private class WebSiteSchema
    {
        [JsonPropertyName("@context")]
        public string Context { get; } = "https://schema.org";

        [JsonPropertyName("@type")]
        public string Type { get; } = "WebSite";

        public required string Name { get; init; }
        public required string Url { get; init; }
    }

    // Deliberately generic (name/description/url only) rather than one
    // strongly-typed class per schema.org type — CollectionPage today,
    // RealEstateListing/ContactPage/JobPosting later all reduce to this
    // same shape from what SeoModel already resolves, with no page-specific
    // data fetching required here.
    private class PageSchema
    {
        [JsonPropertyName("@context")]
        public string Context { get; } = "https://schema.org";

        [JsonPropertyName("@type")]
        public required string Type { get; init; }

        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string Url { get; init; }
    }
}
