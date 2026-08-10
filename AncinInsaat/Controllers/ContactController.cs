using AncinInsaat.Data.Entities;
using AncinInsaat.Models;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

public class ContactController : Controller
{
    // Key for the PRG (Post-Redirect-Get) success flag — set in TempData
    // after a valid POST, read once by the following GET so a page
    // refresh after submitting doesn't resubmit the form or keep showing
    // the success state forever.
    private const string SuccessTempDataKey = "ContactFormSuccess";

    // Hand-authored line icons, same inline-SVG-with-currentColor
    // convention as SocialMediaViewComponent.IconMarkup — kept to the
    // site's single icon library rather than adding an icon package.
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

    private const string ClockIconMarkup = """
        <circle cx="12" cy="12" r="8.5" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M12 7.5V12l3.2 2" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round" />
        """;

    private readonly ISiteSettingsService _siteSettingsService;
    private readonly IContactMessageService _contactMessageService;
    private readonly ISeoService _seoService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(
        ISiteSettingsService siteSettingsService,
        IContactMessageService contactMessageService,
        ISeoService seoService,
        ILogger<ContactController> logger)
    {
        _siteSettingsService = siteSettingsService;
        _contactMessageService = contactMessageService;
        _seoService = seoService;
        _logger = logger;
    }

    [HttpGet("contact")]
    public async Task<IActionResult> Index()
    {
        var showSuccess = TempData[SuccessTempDataKey] is not null;

        var model = await BuildPageModelAsync(new ContactFormViewModel(), showSuccess);

        ViewData["Seo"] = await _seoService.GetPageSeoAsync("contact", Request);
        ViewData["SeoPageType"] = "ContactPage";

        return View(model);
    }

    [HttpPost("contact")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(ContactFormViewModel form)
    {
        // Honeypot: a real visitor never sees or fills this field (see
        // ContactFormViewModel.Website). A bot that blindly fills every
        // input trips it. Pretend success without persisting or logging
        // the submitted content, per docs/12_Security.md ("Never log
        // sensitive personal information") — only the fact that a
        // honeypot fired is worth a log line.
        if (!string.IsNullOrWhiteSpace(form.Website))
        {
            _logger.LogWarning("Contact form honeypot triggered — submission discarded.");
            TempData[SuccessTempDataKey] = true;
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            var invalidModel = await BuildPageModelAsync(form, showSuccess: false);

            ViewData["Seo"] = await _seoService.GetPageSeoAsync("contact", Request);
            ViewData["SeoPageType"] = "ContactPage";

            return View(nameof(Index), invalidModel);
        }

        await _contactMessageService.SaveAsync(new ContactMessage
        {
            FullName = form.FullName.Trim(),
            Email = form.Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(form.Phone) ? null : form.Phone.Trim(),
            Subject = string.IsNullOrWhiteSpace(form.Subject) ? null : form.Subject.Trim(),
            Message = form.Message.Trim()
        });

        TempData[SuccessTempDataKey] = true;
        return RedirectToAction(nameof(Index));
    }

    private async Task<ContactPageViewModel> BuildPageModelAsync(ContactFormViewModel form, bool showSuccess)
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

            if (!string.IsNullOrWhiteSpace(settings.WorkingHours))
            {
                items.Add(new InformationCardItem { IconMarkup = ClockIconMarkup, Label = "Çalışma Saatleri", Value = settings.WorkingHours });
            }
        }

        return new ContactPageViewModel
        {
            Form = form,
            InformationCard = new InformationCardModel { Items = items },
            // Intentionally always null for now — Google's keyless
            // "output=embed" trick does not reliably render (Google now
            // requires either a Maps Embed API key or a signed "Share →
            // Embed a map" URL, neither of which exist yet — see
            // docs/14_Decisions.md). MapViewComponent shows its static
            // placeholder illustration until a real embed URL is wired up
            // here; no other code changes when that happens.
            MapEmbedUrl = null,
            MapLinkUrl = settings?.GoogleMaps,
            ShowSuccess = showSuccess
        };
    }

    private static string NormalizePhoneHref(string phone)
    {
        var digits = phone.Where(c => char.IsDigit(c) || c == '+').ToArray();
        return new string(digits);
    }
}
