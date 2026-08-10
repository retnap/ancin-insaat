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
            CoverImageSrc = ResolveCoverImage(project.CoverImage),
            StatusLabel = project.Status == ProjectStatus.Completed ? "Tamamlandı" : "Devam Ediyor",
            StatusModifierClass = project.Status == ProjectStatus.Completed
                ? "project-status-badge--completed"
                : "project-status-badge--ongoing",
            StatusFilterValue = project.Status == ProjectStatus.Completed ? "completed" : "ongoing",
            DetailUrl = $"/projects/{project.Slug}",
            Location = project.Location,
            ProjectType = project.ProjectType
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

        var galleryCategories = galleryImages
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
            HeroBackgroundImageUrl = $"/images/projects/{project.Slug}/banner.webp",
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
                        PosterUrl = image.ImagePath,
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
                        SalesGrossAreaM2 = hasAreaStats ? floorPlan.SalesGrossAreaM2 : null,
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
