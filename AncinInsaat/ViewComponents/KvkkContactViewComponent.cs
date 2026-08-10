using AncinInsaat.Models;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// KVKK "Contact Information" section (docs/03_PageBlueprints.md — Page
// KVKK). Reuses the existing SiteSettings row via ISiteSettingsService —
// same source ContactController already reads — rather than introducing
// any new contact fields/entities, per the project owner's 2026-08-03
// decision. Icon markup is copied from ContactController's Address/Phone/
// Email constants (same hand-authored-inline-SVG-with-currentColor
// convention as SocialMediaViewComponent.IconMarkup) so the icons stay
// visually identical to the Contact page; no shared icon component exists
// yet in this codebase to import them from instead.
//
// Working Hours is intentionally left out here — 03_PageBlueprints.md's
// KVKK "Contact Information" section is about reaching the data controller
// for a rights request, not office visiting hours, so only Address/Phone/
// Email are shown.
public class KvkkContactViewComponent : ViewComponent
{
    private const string PhoneIconMarkup = """
        <path d="M7.5 3.5c.6 0 1.1.4 1.3 1l1 2.6c.2.5 0 1.1-.4 1.5L8.1 9.8c.9 2 2.5 3.6 4.5 4.5l1.2-1.3c.4-.4 1-.5 1.5-.4l2.6 1c.6.2 1 .7 1 1.3v2.2c0 .9-.8 1.6-1.7 1.5-6.9-.7-12.4-6.2-13.1-13.1C3.1 4.3 3.8 3.5 4.7 3.5h2.8z" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        """;

    private const string EmailIconMarkup = """
        <rect x="3" y="5.5" width="18" height="13" rx="2" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M4 7l8 6 8-6" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        """;

    private const string AddressIconMarkup = """
        <path d="M12 21s7-7.2 7-12a7 7 0 10-14 0c0 4.8 7 12 7 12z" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <circle cx="12" cy="9" r="2.4" fill="none" stroke="currentColor" stroke-width="1.6" />
        """;

    private readonly ISiteSettingsService _siteSettingsService;

    public KvkkContactViewComponent(ISiteSettingsService siteSettingsService)
    {
        _siteSettingsService = siteSettingsService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var settings = await _siteSettingsService.GetAsync();
        var items = new List<InformationCardItem>();

        if (settings is not null)
        {
            if (!string.IsNullOrWhiteSpace(settings.Address))
            {
                items.Add(new InformationCardItem { IconMarkup = AddressIconMarkup, Label = "Adres", Value = settings.Address });
            }

            if (!string.IsNullOrWhiteSpace(settings.Phone))
            {
                items.Add(new InformationCardItem
                {
                    IconMarkup = PhoneIconMarkup,
                    Label = "Telefon",
                    Value = settings.Phone,
                    Href = $"tel:{NormalizePhoneHref(settings.Phone)}"
                });
            }

            if (!string.IsNullOrWhiteSpace(settings.Email))
            {
                items.Add(new InformationCardItem
                {
                    IconMarkup = EmailIconMarkup,
                    Label = "E-posta",
                    Value = settings.Email,
                    Href = $"mailto:{settings.Email}"
                });
            }
        }

        var model = new KvkkContactViewModel
        {
            SectionHeader = new SectionHeaderModel
            {
                HeadingId = "kvkk-contact-heading",
                Eyebrow = "İLETİŞİM",
                Title = "Veri Sahibi Başvuruları",
                Description = "KVKK kapsamındaki haklarınızla ilgili taleplerinizi aşağıdaki kanallardan iletebilirsiniz.",
                CssClass = "section-header--center"
            },
            InformationCard = new InformationCardModel { Items = items }
        };

        return View(model);
    }

    private static string NormalizePhoneHref(string phone)
    {
        var digits = phone.Where(c => char.IsDigit(c) || c == '+').ToArray();
        return new string(digits);
    }
}
