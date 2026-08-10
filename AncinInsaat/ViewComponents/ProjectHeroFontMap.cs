namespace AncinInsaat.ViewComponents;

// Shared by HeroBannerProjectDetailViewComponent (every Project Detail
// Hero) and HeroBannerViewComponent (Home Hero title typography match,
// 2026-08-10) so both Heroes resolve a project's heading font modifier
// from the same slug map — a project's title looks identical wherever its
// Hero appears, and a mapped slug only ever needs to be added/removed in
// one place. See HeroBannerProjectDetailViewComponent for the mapping
// rationale (client-specified, per project family).
internal static class ProjectHeroFontMap
{
    public static readonly IReadOnlyDictionary<string, string> HeadingFontModifierClasses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["nlatis"] = "hero-heading--font-montserrat",
        ["davutlar-d-latis"] = "hero-heading--font-montserrat",
        ["nysa-gold"] = "hero-heading--font-poiret",
        ["tralles-gold"] = "hero-heading--font-poiret",
        ["alinda-gold"] = "hero-heading--font-poiret",
        ["magnesia-gold"] = "hero-heading--font-poiret",
        ["la-fiore-karabag"] = "hero-heading--font-cormorant",
        ["la-fiore-karabag-2-etap"] = "hero-heading--font-cormorant",
        ["le-jardin"] = "hero-heading--font-cormorant",
        ["kuyulu-la-via-villalar-birinci-etap"] = "hero-heading--font-cormorant"
    };
}
