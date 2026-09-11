using AncinInsaat.Data.Entities;
using AncinInsaat.Models;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.Controllers;

public class ProjectsController : Controller
{
    // Mirrors ProjectsShowcaseViewComponent's fallback cover image constant.
    private const string FallbackCoverImage = "/images/projects/project-cover-placeholder.webp";

    // Reference order for the Project Type filter dropdown (08_DatabasePlan.md's
    // approved taxonomy). Any real value outside this list still surfaces —
    // it just sorts after these five, alphabetically — so the filter never
    // silently drops a project type the taxonomy hasn't caught up with yet.
    private static readonly string[] ProjectTypeSortOrder =
    {
        "Residence", "Villa", "Commercial", "Office", "Mixed Use"
    };

    // Reference order for the Gallery's category dropdown (Project Detail
    // redesign, 2026-07-31) — mirrors ProjectTypeSortOrder's approach. Any
    // real category outside this list still surfaces, sorted after these
    // three alphabetically, so a new ProjectImage.Category value never
    // silently disappears from the filter.
    private static readonly string[] GalleryCategorySortOrder =
    {
        "Exterior", "All Exterior", "Interior", "Standard Interior", "Optional Interior", "Sales Office", "Social Areas"
    };

    // Turkish display label for each known ProjectImage.Category value
    // (Gallery Category Cards redesign, 2026-08-04) — shown in the Gallery's
    // category dropdown and on every card in that category. A category
    // outside this map still surfaces (GalleryCategoryModel.Label falls back
    // to the raw value below), it just renders untranslated rather than
    // disappearing. "Standard Interior"/"Optional Interior" (La Via Villalar
    // 1. Etap Gallery integration, 2026-08-06) are this project's own two
    // interior categories — see BuildKuyuluLaViaVillalarImages.
    private static readonly Dictionary<string, string> GalleryCategoryLabels = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Exterior"] = "Dış Mekan Görselleri",
        // "All Exterior" (La Fiore Karabağ 2. Etap Vaziyet Planı/Concept/
        // Gallery phase, 2026-08-09) — a second, broader exterior category
        // distinct from the per-block "Exterior" photos above; today only
        // this project has both.
        ["All Exterior"] = "Tüm Dış Mekan Görselleri",
        ["Interior"] = "İç Mekan Görselleri",
        ["Standard Interior"] = "Standart İç Mekan Görselleri",
        ["Optional Interior"] = "Opsiyonel İç Mekan Görselleri",
        // "Sales Office" (Nysa Gold revision, 2026-08-09) — a project's sales
        // office/showroom photos, its own named category same as every
        // other value here; no special-casing anywhere else in the Gallery.
        ["Sales Office"] = "Satış Ofisi Görselleri",
        ["Social Areas"] = "Sosyal Alan Görselleri"
    };

    // "Social Areas" is the one ProjectImage.Category value Social
    // Facilities cards borrow photos from — kept as a single named constant
    // rather than a magic string repeated at each call site.
    private const string SocialAreasCategory = "Social Areas";

    // Projects catalogue Nysa Gold card treatment (2026-08-20 client
    // request) — same slug-keyed override pattern as
    // ProjectsShowcaseViewComponent's Home-carousel card, reusing the exact
    // same assets. Every other slug falls through to the normal
    // CoverImage-derived image below and gets no LogoImageUrl, so
    // _ProjectCard.cshtml renders nothing extra for them. Project.CoverImage
    // itself, and every other page that reads it (Home carousel, SEO
    // OpenGraph image), stay untouched.
    // La Fiore Karabağ 1. Etap, La Via Villalar 1. Etap and D-Latis
    // (2026-08-20 client request) — same card treatment extended to these
    // three projects' newly supplied assets, added as further entries
    // rather than touching the Nysa Gold/Le Jardin lines above.
    // Tralles Gold Residence, Alinda Gold Residence, Magnesia Gold
    // Residence and Nlatis (2026-08-20 client request) — same treatment
    // again, each project's own proje-karti/card-background.webp derived
    // the same way (client's "X proje kartı.png" resized 1080x1350 →
    // 1000x1250 and re-encoded as webp, same scale factor as every prior
    // project above).
    // Ferhunde Hanım Apt. (2026-08-20 client request) — card image only, no
    // logo image asset exists for this project, so it is intentionally
    // absent from CardLogoImageUrlsBySlug below; it instead gets a
    // text-based title overlay (CardTitleOverlayTextBySlug below). Its
    // card-background.webp was derived from the client's "ferhunde proje
    // kartı.jpeg" (a different source aspect ratio than the 1080x1350 files
    // above — resized to 1000px wide, height following proportionally,
    // rather than forced to 1000x1250) since .project-card-standard-media
    // crops via object-fit: cover regardless of the source's exact ratio.
    // La Fiore Karabağ 2. Etap (2026-08-20 client request) — full treatment
    // (card image + logo), same derivation as the 1080x1350 group above.
    // Hacıfeyzullah - Q-Latis (2026-08-20 client request) — card image
    // only, no logo image asset, same text-title-overlay treatment as
    // Ferhunde Hanım Apt. above (its own raw proje-karti file points
    // directly at the client's PNG rather than a pre-derived webp, since no
    // image-processing step was requested here).
    private static readonly IReadOnlyDictionary<string, string> CardImageOverridesBySlug =
        new Dictionary<string, string>
        {
            ["nysa-gold"] = "/images/projects/nysa-gold/proje-karti/card-background.webp",
            ["le-jardin"] = "/images/projects/le-jardin/proje-karti/card-background.webp",
            ["la-fiore-karabag"] = "/images/projects/la-fiore-karabag/proje-karti/card-background.webp",
            ["kuyulu-la-via-villalar-birinci-etap"] = "/images/projects/kuyulu-la-via-villalar-birinci-etap/proje-karti/card-background.webp",
            ["davutlar-d-latis"] = "/images/projects/davutlar-d-latis/proje-karti/card-background.webp",
            ["tralles-gold"] = "/images/projects/tralles-gold/proje-karti/card-background.webp",
            ["alinda-gold"] = "/images/projects/alinda-gold/proje-karti/card-background.webp",
            ["magnesia-gold"] = "/images/projects/magnesia-gold/proje-karti/card-background.webp",
            ["nlatis"] = "/images/projects/nlatis/proje-karti/card-background.webp",
            ["ferhunde-hanim-apt"] = "/images/projects/ferhunde-hanim-apt/proje-karti/card-background.webp",
            ["la-fiore-karabag-2-etap"] = "/images/projects/la-fiore-karabag-2-etap/proje-karti/card-background.webp",
            ["q-latis"] = "/images/projects/q-latis/proje-karti/hacıfeyzullah proje kartı.png"
        };

    private static readonly IReadOnlyDictionary<string, string> CardLogoImageUrlsBySlug =
        new Dictionary<string, string>
        {
            ["nysa-gold"] = "/images/projects/nysa-gold/proje-karti/logo.png",
            ["le-jardin"] = "/images/projects/le-jardin/proje-karti/logo.png",
            ["la-fiore-karabag"] = "/images/projects/la-fiore-karabag/proje-karti/logo.png",
            ["kuyulu-la-via-villalar-birinci-etap"] = "/images/projects/kuyulu-la-via-villalar-birinci-etap/proje-karti/logo.png",
            ["davutlar-d-latis"] = "/images/projects/davutlar-d-latis/proje-karti/logo.png",
            ["tralles-gold"] = "/images/projects/tralles-gold/proje-karti/logo.png",
            ["alinda-gold"] = "/images/projects/alinda-gold/proje-karti/logo.png",
            ["magnesia-gold"] = "/images/projects/magnesia-gold/proje-karti/logo.png",
            ["nlatis"] = "/images/projects/nlatis/proje-karti/logo.png",
            ["la-fiore-karabag-2-etap"] = "/images/projects/la-fiore-karabag-2-etap/proje-karti/la fiore.png"
        };

    // Ferhunde Hanım Apt. and Q-Latis card treatment (2026-08-20 client
    // request) — neither has a logo asset, so the card's centered overlay
    // is real HTML text instead of an image (_ProjectCard.cshtml,
    // ProjectCardModel.TitleOverlayText). Q-Latis uses the same short
    // brand-facing title as its Hero heading override above rather than the
    // project's full record Name ("Hacıfeyzullah - Q-Latis"); Ferhunde
    // Hanım Apt.'s own Name already is that short form.
    private static readonly IReadOnlyDictionary<string, string> CardTitleOverlayTextBySlug =
        new Dictionary<string, string>
        {
            ["ferhunde-hanim-apt"] = "Ferhunde Hanım Apt.",
            ["q-latis"] = "Q-Latis"
        };

    // Projects catalogue caption removal (2026-08-20 client request) — the
    // full logo or title overlay above already carries each of these
    // projects' branding, so the standard name/location caption underneath
    // is redundant for their cards only. Every other slug keeps its normal
    // caption, unaffected. La Fiore Karabağ 2. Etap is added alongside its
    // new logo overlay above; Ferhunde Hanım Apt. and Q-Latis are added
    // alongside their new title overlays above.
    private static readonly IReadOnlySet<string> CardsWithCaptionHidden =
        new HashSet<string>
        {
            "nysa-gold", "le-jardin",
            "la-fiore-karabag", "kuyulu-la-via-villalar-birinci-etap", "davutlar-d-latis",
            "tralles-gold", "alinda-gold", "magnesia-gold", "nlatis",
            "la-fiore-karabag-2-etap", "ferhunde-hanim-apt", "q-latis"
        };

    // Project Detail Hero background overrides for La Via Villalar 1. Etap
    // and D-Latis (2026-08-20 client request) — same "freshly uploaded, not
    // yet converted to banner.webp" treatment as Nysa Gold/Le Jardin below,
    // kept as its own dictionary so those two projects' existing ternary is
    // left untouched. La Fiore Karabağ 1. Etap had the same entry until the
    // Konsept performance fix (2026-09-11): its Hero PNG was an 8.3MB RGBA
    // original (wwwroot/images/projects/la-fiore-karabag/banner/la fiore
    // banner.png) rendered full-bleed as this page's very first paint, a
    // real contributor to the reported initial-load stutter. Converted to a
    // proper banner.webp via the existing pipeline (tools/ThumbnailTool
    // --single ... --width 1920 --quality 88 — the exact command this
    // project's own DbSeeder.cs comment already prescribed once a real
    // source was supplied) and removed from this override, so Details() now
    // falls through to the default `{slug}/banner.webp` path below like
    // every non-overridden project. Same source photo, unchanged crop/
    // framing — only the delivery format changed.
    // Tralles Gold Residence, Alinda Gold Residence, Magnesia Gold
    // Residence and Nlatis (2026-08-20 client request) — same treatment
    // again; these four projects' own banner.webp/cover.webp were removed
    // in favor of their new banner/ folder assets, so an override here is
    // required (not optional) — without it Details() would fall through to
    // the now-deleted `{slug}/banner.webp` path.
    // Ferhunde Hanım Apt. and La Fiore Karabağ 2. Etap (2026-08-20 client
    // request) — same required override, same reason: both projects' own
    // banner.webp/cover.webp were removed in favor of their new banner/
    // folder assets.
    // Hacıfeyzullah - Q-Latis (2026-08-20 client request) — same required
    // override: this project never had a top-level banner.webp, only the
    // freshly uploaded banner/ folder asset.
    private static readonly IReadOnlyDictionary<string, string> HeroBannerImageOverridesBySlug =
        new Dictionary<string, string>
        {
            // Nysa Gold Residence (2026-09-11 client asset addition) — a new
            // banner photo was dropped into this project's own banner/
            // folder; overriding here points Details() at it instead of the
            // default `{slug}/banner.webp` fallback below.
            ["nysa-gold"] = "/images/projects/nysa-gold/banner/nysa-gold-banner.jpeg",
            ["kuyulu-la-via-villalar-birinci-etap"] = "/images/projects/kuyulu-la-via-villalar-birinci-etap/banner/la-via-banner.png",
            ["davutlar-d-latis"] = "/images/projects/davutlar-d-latis/banner/dlatis banner deneme.png",
            ["tralles-gold"] = "/images/projects/tralles-gold/banner/tralles banner deneme.png",
            ["alinda-gold"] = "/images/projects/alinda-gold/banner/alinda banner deneme.png",
            ["magnesia-gold"] = "/images/projects/magnesia-gold/banner/magnesia banner deneme.png",
            ["nlatis"] = "/images/projects/nlatis/banner/n-latis-banner.jpeg",
            ["ferhunde-hanim-apt"] = "/images/projects/ferhunde-hanim-apt/banner/ferhunde banner deneme.png",
            ["la-fiore-karabag-2-etap"] = "/images/projects/la-fiore-karabag-2-etap/banner/lafiore 2.etap banner deneme.png",
            ["q-latis"] = "/images/projects/q-latis/banner/hacıfeyzullah banner deneme.png"
        };

    // Turkish month names for CompletionDate's display label — the site has
    // no other date formatting yet to be consistent with, and every other
    // user-facing string on the page is already a hardcoded Turkish literal
    // (status labels, filter placeholders), so this follows the same
    // approach rather than introducing a CultureInfo("tr-TR") dependency.
    private static readonly string[] TurkishMonths =
    {
        "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
        "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
    };

    private readonly IProjectQueryService _projectQueryService;
    private readonly ISeoService _seoService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProjectsController(
        IProjectQueryService projectQueryService,
        ISeoService seoService,
        IWebHostEnvironment webHostEnvironment)
    {
        _projectQueryService = projectQueryService;
        _seoService = seoService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("projects")]
    public async Task<IActionResult> Index()
    {
        var projects = await _projectQueryService.GetPublishedProjectsAsync();

        var cards = projects.Select(project => new ProjectCardModel
        {
            Name = project.Name,
            CoverImageSrc = CardImageOverridesBySlug.TryGetValue(project.Slug, out var cardImageOverride)
                ? cardImageOverride
                : ResolveCoverImage(project.CoverImage),
            StatusLabel = project.Status == ProjectStatus.Completed ? "Tamamlandı" : "Devam Ediyor",
            StatusModifierClass = project.Status == ProjectStatus.Completed
                ? "project-status-badge--completed"
                : "project-status-badge--ongoing",
            StatusFilterValue = project.Status == ProjectStatus.Completed ? "completed" : "ongoing",
            DetailUrl = $"/projects/{project.Slug}",
            Location = project.Location,
            ProjectType = project.ProjectType,
            LogoImageUrl = CardLogoImageUrlsBySlug.GetValueOrDefault(project.Slug),
            TitleOverlayText = CardTitleOverlayTextBySlug.GetValueOrDefault(project.Slug),
            HideCaption = CardsWithCaptionHidden.Contains(project.Slug)
        }).ToList();

        var locationOptions = cards
            .Select(c => c.Location)
            .Where(location => !string.IsNullOrWhiteSpace(location))
            .Select(location => location!)
            .Distinct()
            .OrderBy(location => location, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var projectTypeOptions = cards
            .Select(c => c.ProjectType)
            .Where(type => !string.IsNullOrWhiteSpace(type))
            .Select(type => type!)
            .Distinct()
            .OrderBy(type =>
            {
                var index = Array.IndexOf(ProjectTypeSortOrder, type);
                return index == -1 ? ProjectTypeSortOrder.Length : index;
            })
            .ThenBy(type => type, StringComparer.OrdinalIgnoreCase)
            .ToList();

        ViewData["Seo"] = await _seoService.GetPageSeoAsync("projects", Request);
        ViewData["SeoPageType"] = "CollectionPage";

        return View(new ProjectsIndexViewModel
        {
            Cards = cards,
            LocationOptions = locationOptions,
            ProjectTypeOptions = projectTypeOptions
        });
    }

    [HttpGet("projects/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var project = await _projectQueryService.GetPublishedProjectBySlugAsync(slug);

        if (project is null)
        {
            return NotFound();
        }

        var statusLabel = project.Status == ProjectStatus.Completed ? "Tamamlandı" : "Devam Ediyor";
        var statusModifierClass = project.Status == ProjectStatus.Completed
            ? "project-status-badge--completed"
            : "project-status-badge--ongoing";

        var galleryImages = project.Images
            .Where(image => FileExistsInWebRoot(image.ImagePath))
            // A video row's VideoPath must also exist on disk or it falls
            // back to a plain photo card (poster only) rather than a Play
            // button with a dead link — same fail-closed contract as every
            // other media field on this page (Gallery video support,
            // 2026-08-09).
            .Select(image => new GalleryImageModel
            {
                Src = image.ImagePath,
                ThumbnailSrc = ResolveThumbnail(image.ImagePath),
                Alt = image.AltText,
                Category = string.IsNullOrWhiteSpace(image.Category) ? null : image.Category,
                Block = string.IsNullOrWhiteSpace(image.Block) ? null : image.Block,
                ApartmentType = string.IsNullOrWhiteSpace(image.ApartmentType) ? null : image.ApartmentType,
                VideoUrl = !string.IsNullOrWhiteSpace(image.VideoPath) && FileExistsInWebRoot(image.VideoPath)
                    ? image.VideoPath
                    : null
            })
            .ToList();

        // Derived from project.Images (pre file-existence filter), not
        // galleryImages, so a category can be offered in the dropdown/
        // category-card picker before any of its photos exist on disk yet —
        // e.g. Tralles Gold Residence / Magnesia Gold Residence's "Social
        // Areas" row (DbSeeder.cs), added structurally ahead of the client's
        // photos with a not-yet-uploaded ImagePath. Its own images stay
        // correctly absent from galleryImages/the merged "Tüm Görseller"
        // grid until a real file exists; only the picker/dropdown entry
        // shows early (2026-08-28 Gallery Category Picker revision).
        var galleryCategories = project.Images
            .Select(image => image.Category)
            .Where(category => !string.IsNullOrWhiteSpace(category))
            .Select(category => category!)
            .Distinct()
            .OrderBy(category =>
            {
                var index = Array.IndexOf(GalleryCategorySortOrder, category);
                return index == -1 ? GalleryCategorySortOrder.Length : index;
            })
            .ThenBy(category => category, StringComparer.OrdinalIgnoreCase)
            .Select(category => new GalleryCategoryModel
            {
                Value = category,
                Label = GalleryCategoryLabels.TryGetValue(category, out var label) ? label : category
            })
            .ToList();

        // Social Facilities cards borrow "Social Areas" gallery photos,
        // cycling through them when there are fewer photos than Amenities
        // lines; a project with no "Social Areas" photos gets text-only
        // cards rather than an unrelated or missing image.
        var socialAreaImages = galleryImages
            .Where(image => string.Equals(image.Category, SocialAreasCategory, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var amenities = SplitLines(project.Amenities);
        var socialFacilities = amenities
            .Select((name, index) => new SocialFacilityModel
            {
                Name = name,
                ImageSrc = socialAreaImages.Count > 0 ? socialAreaImages[index % socialAreaImages.Count].ThumbnailSrc : null
            })
            .ToList();

        var model = new ProjectDetailViewModel
        {
            Name = project.Name,
            Slug = project.Slug,
            StatusLabel = statusLabel,
            StatusModifierClass = statusModifierClass,
            // Nysa Gold temporarily points at the client's freshly uploaded
            // banner asset (still its original PNG, not yet converted/renamed
            // into the banner.webp slot every other project uses) per the
            // 2026-08-20 request to preview it as-is before optimization.
            // Le Jardin follows the same approach (2026-08-20 request) with
            // its own freshly uploaded banner asset.
            HeroBackgroundImageUrl = HeroBannerImageOverridesBySlug.TryGetValue(project.Slug, out var heroBannerOverride)
                ? heroBannerOverride
                : project.Slug == "nysa-gold"
                    ? "/images/projects/nysa-gold/banner/nysa gold 4k.png"
                    : project.Slug == "le-jardin"
                        ? "/images/projects/le-jardin/banner/le jardin banner.png"
                        : $"/images/projects/{project.Slug}/banner.webp",
            ShortDescription = string.IsNullOrWhiteSpace(project.ShortDescription) ? null : project.ShortDescription,
            DescriptionParagraphs = SplitDescription(project.Description),
            Information = new ProjectInformationModel
            {
                Name = project.Name,
                Location = string.IsNullOrWhiteSpace(project.Location) ? null : project.Location,
                StatusLabel = statusLabel,
                CompletionDateLabel = project.CompletionDate is { } completionDate ? FormatCompletionDate(completionDate) : null
            },
            ConceptImageUrl = galleryImages.Count > 0 ? galleryImages[0].Src : FallbackCoverImage,
            ConceptVideoUrl = !string.IsNullOrWhiteSpace(project.ConceptVideoPath) && FileExistsInWebRoot(project.ConceptVideoPath)
                ? project.ConceptVideoPath
                : null,
            ConceptVideoPosterUrl = !string.IsNullOrWhiteSpace(project.ConceptVideoPosterPath) && FileExistsInWebRoot(project.ConceptVideoPosterPath)
                ? project.ConceptVideoPosterPath
                : null,
            // Video and image slides merged into one carousel, ordered by
            // DisplayOrder across both (Le Jardin Concept carousel
            // generalization, 2026-08-09) — see ConceptSlideModel. Relies on
            // each project's seed data assigning non-overlapping DisplayOrder
            // values across its ConceptVideos/ConceptImages rows so the two
            // sequences interleave correctly once merged.
            ConceptSlides = project.ConceptVideos
                .Where(video => FileExistsInWebRoot(video.VideoPath) && FileExistsInWebRoot(video.PosterPath))
                .Select(video => new { video.DisplayOrder, Slide = new ConceptSlideModel
                {
                    MediaType = ConceptMediaType.Video,
                    PosterUrl = video.PosterPath,
                    LightboxUrl = video.PosterPath,
                    VideoUrl = video.VideoPath,
                    Eyebrow = video.Eyebrow,
                    Title = video.Title,
                    Description = video.Description
                } })
                .Concat(project.ConceptImages
                    .Where(image => FileExistsInWebRoot(image.ImagePath))
                    .Select(image => new { image.DisplayOrder, Slide = new ConceptSlideModel
                    {
                        MediaType = ConceptMediaType.Image,
                        PosterUrl = ResolveConceptCardImage(project.Slug, image.ImagePath),
                        LightboxUrl = image.ImagePath,
                        VideoUrl = null,
                        Eyebrow = image.Eyebrow,
                        Title = image.Title,
                        Description = image.Description
                    } }))
                .OrderBy(entry => entry.DisplayOrder)
                .Select(entry => entry.Slide)
                .ToList(),
            ConceptDescription = string.IsNullOrWhiteSpace(project.ConceptDescription) ? null : project.ConceptDescription,
            NearbyPlaces = project.NearbyPlaces
                .Select(place => new NearbyPlaceModel { Name = place.Name, Distance = place.Distance })
                .ToList(),
            GalleryImages = galleryImages,
            GalleryCategories = galleryCategories,
            CatalogueUrl = !string.IsNullOrWhiteSpace(project.CataloguePath) && FileExistsInWebRoot(project.CataloguePath)
                ? project.CataloguePath
                : null,
            CatalogueComingSoon = project.CatalogueComingSoon,
            // ANDed defensively so a project can never show the Hero toast
            // without CatalogueComingSoon also being true — see
            // Project.CatalogueComingSoonHeroToast.
            CatalogueComingSoonHeroToast = project.CatalogueComingSoon && project.CatalogueComingSoonHeroToast,
            SitePlanComingSoon = project.SitePlanComingSoon,
            SitePlanImageUrls = project.SitePlanImages
                .OrderBy(sitePlan => sitePlan.DisplayOrder)
                .Where(sitePlan => FileExistsInWebRoot(sitePlan.ImagePath))
                .Select(sitePlan => sitePlan.ImagePath)
                .ToList(),
            LocationImageUrl = !string.IsNullOrWhiteSpace(project.LocationImagePath) && FileExistsInWebRoot(project.LocationImagePath)
                ? project.LocationImagePath
                : null,
            SocialFacilities = socialFacilities,
            // Not existence-filtered like GalleryImages — every apartment
            // type's stats (Net/Gross/Sales Alan, room list) always display,
            // real or placeholder. Only the visual (Src/ThumbnailSrc) is
            // existence-checked, same as GalleryImages: a project with real
            // architectural drawings (e.g. La Fiore Karabağ 2. Etap) gets its
            // own photo per panel, everything else falls back to
            // _FloorPlans.cshtml's static placeholder exactly as before.
            FloorPlans = project.FloorPlans
                .Select(floorPlan =>
                {
                    var hasAreaStats = floorPlan.NetAreaM2 > 0 || floorPlan.GrossAreaM2 > 0 || floorPlan.SalesGrossAreaM2 > 0;
                    return new FloorPlanModel
                    {
                        ApartmentType = floorPlan.ApartmentType,
                        NetAreaM2 = hasAreaStats ? floorPlan.NetAreaM2 : null,
                        GrossAreaM2 = hasAreaStats ? floorPlan.GrossAreaM2 : null,
                        // Independent of hasAreaStats (Le Jardin Net/Brüt box
                        // restore, 2026-08-20) — Le Jardin's drawings print a
                        // real Net/Brüt total per floor but no "Satışa Esas
                        // Brüt Alan" figure anywhere, so its FloorPlan rows
                        // keep SalesGrossAreaM2 at its 0 default while
                        // NetAreaM2/GrossAreaM2 hold real values. Every other
                        // project already keeps all three fields at 0 or all
                        // three real together, so this is a no-op for them.
                        SalesGrossAreaM2 = floorPlan.SalesGrossAreaM2 > 0 ? floorPlan.SalesGrossAreaM2 : null,
                        Rooms = floorPlan.Rooms
                            .OrderBy(room => room.DisplayOrder)
                            .Select(room => new FloorPlanRoomModel { Name = room.Name, AreaM2 = room.AreaM2 })
                            .ToList(),
                        Src = FileExistsInWebRoot(floorPlan.ImagePath) ? floorPlan.ImagePath : null,
                        ThumbnailSrc = FileExistsInWebRoot(floorPlan.ImagePath) ? ResolveThumbnail(floorPlan.ImagePath) : null
                    };
                })
                .ToList()
        };

        // "RealEstateListing" per 03_PageBlueprints.md's Project Detail SEO
        // section — the pageKey is the project's own slug, so per-project
        // rows sit in the same SeoMetadata table as "home"/"projects" rather
        // than a second mechanism (see DbSeeder.SeedSeoMetadataAsync).
        ViewData["Seo"] = await _seoService.GetPageSeoAsync(project.Slug, Request);
        ViewData["SeoPageType"] = "RealEstateListing";

        // Read by _Layout.cshtml to stamp <body data-page-slug="...">, the
        // one bridge site.js/CSS need to scope the Nysa Gold reference
        // redesign's transparent-navbar behavior (see site.js) without
        // threading Project Detail's ViewModel through NavbarViewComponent,
        // which has no notion of "current project" today. Unset (and
        // therefore a no-op) on every other action/page.
        ViewData["ProjectSlug"] = project.Slug;

        return View(model);
    }

    private static IReadOnlyList<string> SplitDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return Array.Empty<string>();
        }

        return description
            .Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(paragraph => paragraph.Trim())
            .Where(paragraph => paragraph.Length > 0)
            .ToList();
    }

    // Splits Amenities (one facility per line) into a trimmed, blank-free
    // list — same "generic freeform text" contract as SplitDescription, but
    // line-by-line rather than blank-line-separated paragraphs, matching how
    // Project.Amenities is documented in docs/08_DatabasePlan.md.
    private static IReadOnlyList<string> SplitLines(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Array.Empty<string>();
        }

        return text
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => line.Length > 0)
            .ToList();
    }

    private static string FormatCompletionDate(DateTime completionDate)
    {
        return $"{TurkishMonths[completionDate.Month - 1]} {completionDate.Year}";
    }

    // Falls back to the shared placeholder cover whenever a project's real
    // photo has not been supplied yet, exactly like
    // ProjectsShowcaseViewComponent.ResolveCoverImage. Duplicated rather
    // than extracted into a shared service: that component belongs to the
    // completed Home milestone, which the project's working rules say not
    // to modify outside an explicit request.
    private string ResolveCoverImage(string coverImage)
    {
        if (string.IsNullOrWhiteSpace(coverImage) || !FileExistsInWebRoot(coverImage))
        {
            return FallbackCoverImage;
        }

        return ResolveThumbnail(coverImage);
    }

    // Prefers the generated WebP thumbnail (docs/07_AssetStructure.md) for
    // whatever a gallery/cover image actually renders as; falls back to the
    // full original when no thumbnail has been generated for it yet (e.g. a
    // photo just dropped into an "originals" folder before ThumbnailTool has
    // been run), so nothing 404s while thumbnails are pending.
    private string ResolveThumbnail(string originalPath)
    {
        var thumbnailPath = ImagePathHelper.GetThumbnailPath(originalPath);
        return FileExistsInWebRoot(thumbnailPath) ? thumbnailPath : originalPath;
    }

    // La Fiore Karabağ (1. Etap) Konsept performance fix, 2026-09-11 — its 3
    // Konsept slides reuse full Gallery originals (10-35MB camera JPEGs,
    // gallery/exterior/originals/1.jpg/3.jpg/13.jpg) as their card image, the
    // exact files ResolveThumbnail already swaps for a ~100KB WebP thumbnail
    // everywhere else on this page (Gallery grid, Cover). The Concept
    // carousel never called ResolveThumbnail for any project (PosterUrl above
    // always used the raw ImagePath) — every other project's Konsept images
    // happen to already be small pre-optimized files (a dedicated banner/
    // concept asset, not a raw camera original), so nothing there ever
    // surfaced this. Scoped to this one slug — rather than resolving a
    // thumbnail for every project's ConceptImages — so no other project's
    // rendered Konsept output changes; ConceptSlideModel.LightboxUrl still
    // carries the untouched original for the Media Viewer/lightbox.
    private static readonly HashSet<string> ConceptCardThumbnailSlugs = new(StringComparer.OrdinalIgnoreCase)
    {
        "la-fiore-karabag"
    };

    private string ResolveConceptCardImage(string slug, string originalImagePath)
    {
        return ConceptCardThumbnailSlugs.Contains(slug) ? ResolveThumbnail(originalImagePath) : originalImagePath;
    }

    // Shared by ResolveCoverImage and Details' Gallery/Floor Plans filtering
    // (2026-07-31, Project Detail Media phase) — every seeded image path
    // follows 07_AssetStructure.md's convention but the underlying file may
    // not exist yet (see DbSeeder.SeedProjectsAsync), so every project-image
    // reference on this controller is existence-checked before it reaches a
    // view, never left to 404 as a broken `<img>`.
    private bool FileExistsInWebRoot(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return false;
        }

        var normalizedPath = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var absolutePath = Path.Combine(_webHostEnvironment.WebRootPath, normalizedPath);

        return System.IO.File.Exists(absolutePath);
    }
}
