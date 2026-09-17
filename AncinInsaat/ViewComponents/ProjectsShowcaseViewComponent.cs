using AncinInsaat.Data.Entities;
using AncinInsaat.Models;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Home page only, directly below Company History — added at the project
// owner's explicit 2026-07-28 request, using Inspirationals/Terzioglu's
// "TAMAMLANAN PROJELER" section for layout/hierarchy inspiration only (not
// its copy: "Hayallerinizi Yaşatır" is intentionally excluded per that
// request). Lists every published project via the same
// IProjectQueryService the Hero Banner already uses, so Project stays the
// single source of truth for project data site-wide rather than a second,
// parallel list — see DbSeeder for the 7 projects added to back this
// section's full carousel-derived list (docs/03_PageBlueprints.md's Home
// "Featured Projects" only anticipated a curated subset; this shows all
// published projects instead, per that request).
public class ProjectsShowcaseViewComponent : ViewComponent
{
    private const string FallbackCoverImage = "/images/projects/project-cover-placeholder.webp";

    // Card hover-title overrides (2026-08-20 request) — the client wants
    // these six exact strings on the Home carousel card only, verbatim
    // (including "VİLLALAR"'s Turkish dotted İ), independent of each
    // Project entity's own Name. Scoped to this component/dictionary only:
    // Project.Name (DB/seed data) and every other page that reads it —
    // Project Detail, /projects, admin — are untouched.
    private static readonly IReadOnlyDictionary<string, string> CardDisplayNameOverridesBySlug =
        new Dictionary<string, string>
        {
            ["nysa-gold"] = "NYSA GOLD",
            ["le-jardin"] = "LE JARDIN",
            ["la-fiore-karabag-2-etap"] = "LA FIORE 2. ETAP",
            ["kuyulu-la-via-villalar-birinci-etap"] = "LA VIA VİLLALAR 1. ETAP",
            ["davutlar-d-latis"] = "D-LATIS",
            ["q-latis"] = "Q-LATIS"
        };

    // Nysa Gold Home-carousel card branding refresh (2026-08-20 client
    // request) — swaps this one card's background image for the client's
    // new proje-karti asset and overlays the project's full logo, both
    // scoped to this Home carousel only via these slug-keyed dictionaries
    // (same pattern as CardDisplayNameOverridesBySlug above). Every other
    // slug falls through to the normal CoverImage-derived image below and
    // gets no LogoImageUrl, so Default.cshtml renders nothing extra for
    // them. Project.CoverImage itself, and every other page that reads it
    // (Projects listing, SEO OpenGraph image), stay untouched.
    // La Via Villalar 1. Etap and D-Latis (2026-08-20 client request) —
    // same card treatment extended to these two projects' newly supplied
    // assets, added as further entries rather than touching the Nysa
    // Gold/Le Jardin lines above. La Fiore Karabağ 1. Etap is Completed, so
    // it never appears in this Ongoing-only carousel and needs no entry
    // here (its Home-carousel display name above is likewise absent).
    // Tralles Gold Residence, Alinda Gold Residence, Magnesia Gold
    // Residence and Nlatis (2026-08-20 client request) — same treatment
    // extended again. All four are Completed, so like La Fiore Karabağ 1.
    // Etap above they never render in this Ongoing-only carousel today;
    // the entries are kept here anyway so the treatment applies
    // automatically if any of them is ever marked Ongoing, with no further
    // code change.
    // Ferhunde Hanım Apt. (2026-08-20 client request) — card image only, no
    // logo asset exists, so no CardLogoImageUrlsBySlug entry below. Also
    // Completed, so (like the four projects above) this is a no-op today
    // and only takes effect if the project is ever marked Ongoing.
    // La Fiore Karabağ 2. Etap (2026-08-20 client request) — full card
    // treatment (image + logo); this project is Ongoing, so both entries
    // render immediately in this carousel.
    // Tralles Gold, Alinda Gold, Le Jardin, La Fiore Karabağ 1. Etap, La
    // Fiore Karabağ 2. Etap and Ferhunde Hanım (2026-09-17 Devam Eden
    // Projeler banner sync request) — these six entries are repointed at
    // the exact same file each project's own Project Detail Hero Banner
    // currently uses (ProjectsController.HeroBannerImageOverridesBySlug,
    // or its Le Jardin ternary fallback), rather than each project's
    // separate proje-karti/card-background.webp asset, so this card and
    // that Banner never drift apart again. La Fiore Karabağ 1. Etap
    // (slug "la-fiore-karabag") gets a first entry here for the same
    // reason; it is Completed today so, like the other Completed slugs
    // above, this is a no-op until the project is ever marked Ongoing.
    private static readonly IReadOnlyDictionary<string, string> CardImageOverridesBySlug =
        new Dictionary<string, string>
        {
            ["nysa-gold"] = "/images/projects/nysa-gold/proje-karti/card-background.webp",
            ["le-jardin"] = "/images/projects/le-jardin/banner/le-jardin-yeni-banner.png",
            ["kuyulu-la-via-villalar-birinci-etap"] = "/images/projects/kuyulu-la-via-villalar-birinci-etap/proje-karti/card-background.webp",
            ["davutlar-d-latis"] = "/images/projects/davutlar-d-latis/proje-karti/card-background.webp",
            ["tralles-gold"] = "/images/projects/tralles-gold/banner/tralles-gold-yeni-banner.jpeg",
            ["alinda-gold"] = "/images/projects/alinda-gold/banner/alinda-gold-yeni-banner.jpeg",
            ["magnesia-gold"] = "/images/projects/magnesia-gold/proje-karti/card-background.webp",
            ["nlatis"] = "/images/projects/nlatis/proje-karti/card-background.webp",
            ["ferhunde-hanim-apt"] = "/images/projects/ferhunde-hanim-apt/banner/ferhunde-hanim-yeni-banner-2.jpeg",
            ["la-fiore-karabag"] = "/images/projects/la-fiore-karabag/banner/la-fiore-karabag-birinci-yeni-banner-2.jpeg",
            ["la-fiore-karabag-2-etap"] = "/images/projects/la-fiore-karabag-2-etap/banner/la-fiore-karabag-ikinci-yeni-banner-2.jpeg",
            // Hacıfeyzullah - Q-Latis (2026-08-20 client request) — same
            // raw-file treatment as ProjectsController's own override (no
            // logo overlay supplied, so it is intentionally absent from
            // CardLogoImageUrlsBySlug below and keeps its normal caption).
            ["q-latis"] = "/images/projects/q-latis/proje-karti/hacıfeyzullah proje kartı.png"
        };

    private static readonly IReadOnlyDictionary<string, string> CardLogoImageUrlsBySlug =
        new Dictionary<string, string>
        {
            ["nysa-gold"] = "/images/projects/nysa-gold/proje-karti/logo.png",
            ["le-jardin"] = "/images/projects/le-jardin/proje-karti/logo.png",
            ["kuyulu-la-via-villalar-birinci-etap"] = "/images/projects/kuyulu-la-via-villalar-birinci-etap/proje-karti/logo.png",
            ["davutlar-d-latis"] = "/images/projects/davutlar-d-latis/proje-karti/logo.png",
            ["tralles-gold"] = "/images/projects/tralles-gold/proje-karti/logo.png",
            ["alinda-gold"] = "/images/projects/alinda-gold/proje-karti/logo.png",
            ["magnesia-gold"] = "/images/projects/magnesia-gold/proje-karti/logo.png",
            ["nlatis"] = "/images/projects/nlatis/proje-karti/logo.png",
            ["la-fiore-karabag-2-etap"] = "/images/projects/la-fiore-karabag-2-etap/proje-karti/la fiore.png"
        };

    private readonly IProjectQueryService _projectQueryService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProjectsShowcaseViewComponent(IProjectQueryService projectQueryService, IWebHostEnvironment webHostEnvironment)
    {
        _projectQueryService = projectQueryService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var projects = await _projectQueryService.GetPublishedProjectsAsync();

        // Home page shows ongoing projects only (2026-08-03 request) — this
        // filters the shared published list locally rather than adding a
        // new IProjectQueryService method, so /projects and its own
        // filtering stay untouched.
        var ongoingProjects = projects.Where(project => project.Status == ProjectStatus.Ongoing);

        var cards = ongoingProjects.Select(project => new ProjectShowcaseCardModel
        {
            Name = CardDisplayNameOverridesBySlug.TryGetValue(project.Slug, out var displayNameOverride)
                ? displayNameOverride
                : project.Name,
            CoverImageSrc = CardImageOverridesBySlug.TryGetValue(project.Slug, out var cardImageOverride)
                ? cardImageOverride
                : ResolveCoverImage(project.CoverImage),
            StatusLabel = project.Status == ProjectStatus.Completed ? "Tamamlandı" : "Devam Ediyor",
            StatusModifierClass = project.Status == ProjectStatus.Completed
                ? "project-status-badge--completed"
                : "project-status-badge--ongoing",
            DetailUrl = $"/projects/{project.Slug}",
            LogoImageUrl = CardLogoImageUrlsBySlug.GetValueOrDefault(project.Slug)
        }).ToList();

        var model = new ProjectsShowcaseViewModel
        {
            Header = new SectionHeaderModel
            {
                // 2026-08-16 revision: heading swapped to match
                // Inspirationals/Terzioglu/terzioglu-projeler-referans.png's
                // slogan/title pairing (styling only, via .projects-showcase-header
                // in site.css Section 21 — see comment there for why the eyebrow's
                // shared decorative line is hidden and why the title's font
                // diverges from the default h2 treatment).
                Eyebrow = "İlklerin Mimarı - Ancın İnşaat",
                Title = "Devam Eden Projeler",
                HeadingId = "projects-showcase-heading"
            },
            Cards = cards
        };

        return View(model);
    }

    // Falls back to the shared placeholder cover whenever a project's real
    // photo has not been supplied yet, rather than hardcoding which slugs
    // have real media — once a real CoverImage is dropped in, this starts
    // rendering it automatically with no code change. Prefers the generated
    // WebP thumbnail (docs/07_AssetStructure.md) over the full original,
    // same derivation as ProjectsController.ResolveCoverImage — duplicated
    // rather than shared, per this component's existing precedent of not
    // touching the completed Home milestone's architecture.
    private string ResolveCoverImage(string coverImage)
    {
        if (string.IsNullOrWhiteSpace(coverImage) || !FileExists(coverImage))
        {
            return FallbackCoverImage;
        }

        var thumbnailPath = ImagePathHelper.GetThumbnailPath(coverImage);
        return FileExists(thumbnailPath) ? thumbnailPath : coverImage;
    }

    private bool FileExists(string relativePath)
    {
        var normalizedPath = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, normalizedPath);
        return File.Exists(absolutePath);
    }
}
