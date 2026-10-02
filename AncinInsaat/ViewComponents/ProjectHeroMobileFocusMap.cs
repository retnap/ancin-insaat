namespace AncinInsaat.ViewComponents;

// Mobile Performance & Responsive Pass (2026-10-02) — shared by
// HeroBannerProjectDetailViewComponent (every Project Detail Hero) and
// HeroBannerViewComponent (Home Hero, whenever the latest featured project
// is one of the slugs below) so both Heroes crop a project's banner
// identically on narrow viewports, same precedent as ProjectHeroFontMap.
//
// Every Hero banner is a CSS background-image with background-size: cover,
// background-position: center (site.css, .hero-bg) — correct for a banner
// whose subject already sits in the frame's center, but on a narrow/tall
// mobile viewport a plain center crop pushes off-screen whatever the
// subject actually sits closest to. Each entry here is this project's own
// banner image inspected directly (wwwroot/images/projects/{slug}/banner/),
// not a guess: a value only exists where the building/development in the
// photo sits meaningfully off-center, so the mobile crop can be re-aimed at
// it without touching the image itself or its desktop presentation
// (site.css's 768px+ override always forces plain center back, regardless
// of this map). A slug with no entry here — including every banner whose
// subject is already centered or spans evenly across the full frame (Nysa Gold,
// Tralles Gold, Alinda Gold, Hacıfeyzullah - Q-Latis, La Fiore Karabağ 1./2.
// Etap) — renders with the exact same plain "center" it always has.
internal static class ProjectHeroMobileFocusMap
{
    public static readonly IReadOnlyDictionary<string, string> MobileBackgroundPositions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        // Le Jardin (banner/le-jardin-yeni-banner.png) — the villa row runs
        // diagonally through the right ~55% of the frame; the left half is
        // empty olive grove. Plain center crops into the grove and the
        // buildings' own left edge on a narrow viewport; shifting right
        // keeps the row itself in frame.
        ["le-jardin"] = "72% 50%",

        // Davutlar D Latis (banner/dlatis banner deneme.png) — the building
        // sits center-right with a wide pool/terrace foreground band; a
        // taller mobile crop needs less of that foreground and more of the
        // building itself to read as the same photo.
        ["davutlar-d-latis"] = "64% 38%",

        // Magnesia Gold Residence (banner/magnesia banner deneme.png) — the
        // three blocks span the full width but sit in the frame's upper
        // half, with a deep landscaped-park foreground; centering vertically
        // crops into roofs on a narrow viewport, so this biases up to keep
        // the buildings themselves in frame.
        ["magnesia-gold"] = "50% 30%",

        // Nlatis (banner/n-latis-banner.jpeg) — the tower sits right of
        // center against open sky/sea on the left; shifting right keeps the
        // building (not the empty sky) centered on a narrow crop.
        ["nlatis"] = "62% 55%",

        // Ferhunde Hanım Apt. (banner/ferhunde-hanim-yeni-banner-2.jpeg) —
        // the building sits right of center behind a large foreground tree
        // on the left; shifting right keeps the building rather than the
        // tree canopy centered on a narrow crop.
        ["ferhunde-hanim-apt"] = "68% 48%",

        // Kuyulu La Via Villalar 1. Etap (banner/la-via-banner.png) — the
        // villa row's densest, closest cluster sits in the lower-right
        // portion of the frame; a mild right/down bias keeps it centered
        // without losing the row's own left end entirely on a narrow crop.
        ["kuyulu-la-via-villalar-birinci-etap"] = "58% 58%"
    };
}
