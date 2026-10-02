namespace AncinInsaat.ViewComponents;

public class NavbarViewModel
{
    public bool IsHomeActive { get; set; }
    public bool IsCorporateActive { get; set; }
    public bool IsProjectsActive { get; set; }
    public bool IsCareerActive { get; set; }
    public bool IsContactActive { get; set; }

    // English localization (2026-10-02) — the language switch always links
    // to the EN/TR equivalent of the page actually being viewed (never
    // home), computed from the current request by LanguageUrlService. See
    // Views/Shared/Components/Navbar/Default.cshtml.
    public bool IsEnglish { get; set; }
    public required string TurkishUrl { get; set; }
    public required string EnglishUrl { get; set; }
}
