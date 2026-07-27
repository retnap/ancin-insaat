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
            CompanyTagline = "Building lasting value across Aydın, one project at a time.",
            Address = "Kültür Mahallesi, İnşaat Caddesi No:12, Aydın, Türkiye",
            Phone = "+90 256 123 45 67",
            PhoneHref = "+902561234567",
            Email = "info@ancininsaat.com",
            CorporateLinks = new List<FooterLinkItem>
            {
                new() { Label = "About Us", Url = "/about" },
                new() { Label = "Our Values", Url = "/values" },
                new() { Label = "KVKK", Url = "/kvkk" }
            },
            PeopleFirstLinks = new List<FooterLinkItem>
            {
                new() { Label = "Career", Url = "/career" },
                new() { Label = "HR Policy", Url = "/hr-policy" }
            },
            ExploreLinks = new List<FooterLinkItem>
            {
                new() { Label = "Home", Url = "/" },
                new() { Label = "Projects", Url = "/projects" },
                new() { Label = "Contact", Url = "/contact" }
            },
            CopyrightYear = DateTime.UtcNow.Year
        };

        return View(model);
    }
}
