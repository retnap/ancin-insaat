using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    // PLACEHOLDER — company details are not yet confirmed by the client.
    // Realistic-but-fictional values per CLAUDE.md Placeholder Content
    // rules; replace before launch. Hardcoded rather than DB-backed
    // because Company Information is static content (06_ContentStructure.md)
    // and the Admin Panel is out of scope until Milestone 3+.
    public IViewComponentResult Invoke()
    {
        var model = new FooterViewModel
        {
            AddressLine1 = "Kültür Mahallesi, İnşaat Caddesi No:12",
            AddressLine2 = "Aydın, Türkiye",
            Phone = "+90 256 123 45 67",
            PhoneHref = "+902561234567",
            Email = "info@ancininsaat.com",
            CopyrightYear = DateTime.UtcNow.Year
        };

        return View(model);
    }
}
