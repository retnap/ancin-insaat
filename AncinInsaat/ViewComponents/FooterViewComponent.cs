using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    // Real company contact details (client-confirmed, 2026-10-08) — no
    // longer placeholder. Still hardcoded rather than DB-backed, same as
    // before, because Company Information is static content
    // (06_ContentStructure.md) and the Admin Panel is out of scope until
    // Milestone 3+; SiteSettings (see ContactController) is the actual
    // source of truth for the Contact page and carries the same values plus
    // the Mobile number this single-phone-line footer design has no slot
    // for.
    public IViewComponentResult Invoke()
    {
        var model = new FooterViewModel
        {
            AddressLine1 = "Zeybek Mahallesi 5 cd. No:7/A/31 Blok",
            AddressLine2 = "Magnesia Gold Residence Efeler / AYDIN",
            Phone = "0256 212 9444",
            PhoneHref = "02562129444",
            Email = "info@ancininsaat.com.tr",
            CopyrightYear = DateTime.UtcNow.Year
        };

        return View(model);
    }
}
