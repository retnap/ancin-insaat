using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    // Fallback only — used if the SiteSettings row is missing or has no
    // Email, so the footer never renders an empty mailto: link.
    private const string FallbackEmail = "info@ancin.com.tr";

    private readonly ISiteSettingsService _siteSettingsService;

    public FooterViewComponent(ISiteSettingsService siteSettingsService)
    {
        _siteSettingsService = siteSettingsService;
    }

    // Real company contact details (client-confirmed, 2026-10-08) — no
    // longer placeholder. Address/Phone are still hardcoded rather than
    // DB-backed because Company Information is static content
    // (06_ContentStructure.md) and the Admin Panel is out of scope until
    // Milestone 3+. Email is read from SiteSettings (the same source as the
    // Contact page, KVKK block and Organization JSON-LD) so a correction to
    // the address only ever has to be made in one place.
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var settings = await _siteSettingsService.GetAsync();
        var email = string.IsNullOrWhiteSpace(settings?.Email) ? FallbackEmail : settings.Email;

        var model = new FooterViewModel
        {
            AddressLine1 = "Zeybek Mahallesi 5 cd. No:7/A/31 Blok",
            AddressLine2 = "Magnesia Gold Residence Efeler / AYDIN",
            Phone = "0256 212 9444",
            PhoneHref = "02562129444",
            Email = email,
            CopyrightYear = DateTime.UtcNow.Year
        };

        return View(model);
    }
}
