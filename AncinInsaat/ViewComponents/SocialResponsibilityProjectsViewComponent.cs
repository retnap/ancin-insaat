using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

// Sosyal Sorumluluk Projelerimiz — the two real projects (2026-10-02
// client request), replacing the earlier four generic placeholder pillars
// (SocialResponsibilityPillarsViewComponent, removed). The page must show
// exactly these two projects — no other content, no invented third item.
//
// Unlike the earlier pillars, both projects here are grounded directly in
// the client-supplied photos themselves rather than written from scratch:
// Project 2's own photo is an official event flyer whose printed text
// ("Ançın Geleceğin Mimarları Etkinliği", "23 Nisan Ulusal Egemenlik ve
// Çocuk Bayramı", "25 Nisan 2026 Cumartesi, Aydın OPSmall AVM", organized
// with "Neşeli Oyunlar Etkinlik & Oyun Atölyesi") is quoted/paraphrased
// below, not invented. Project 1's photos show an "ANÇIN"-branded youth
// performance team at an Aydın Gençlik ve Spor İl Müdürlüğü event and a
// costume/achievement recognition at the Ançın office — described only in
// general terms actually visible in the photos, with no specific club
// name, date or statistic invented beyond what the images show.
//
// Static/hardcoded content, same precedent as every other page-section
// ViewComponent (HrPhilosophy, CorePrinciples, etc.) — see
// docs/14_Decisions.md's "Database Strategy": no new entity was introduced
// since this page has exactly these two fixed projects and no admin/CMS
// editing is in scope for Version 1.
//
// Image assets: see the one-off optimizer the previous task ran
// (SixLabors.ImageSharp via the existing AncinInsaat.ImagePipeline
// ThumbnailGenerator — same tool tools/ThumbnailTool already uses for every
// other project gallery). Raw originals are untouched at
// wwwroot/images/sosyal-sorumluluk-projeleri/{birinci,ikinci}-proje/*;
// optimized WebP derivatives (two size tiers — a small on-page "featured"
// image and a larger "lightbox" version of every photo) live in each
// project's sibling web/ subfolder.
//
// Video files (2026-10-02, unified gallery task) — the same web/ subfolder
// already held each project's optimized MP4 + WebP poster from the previous
// task (video.mp4/video-poster.webp for Project 1; video-1/-2.mp4 +
// video-1/-2-poster.webp for Project 2); this task only wires those
// existing files into OtherMedia, appended after every photo, as
// VideoSrc-bearing SocialResponsibilityGalleryImage entries — no
// regeneration, no new asset files. They join each project's single
// GalleryGroupKey, so the existing, unmodified Media Viewer (site.js)
// already navigates seamlessly between a project's photos and videos; see
// SocialResponsibilityProjects/Default.cshtml and _MediaViewer.cshtml for
// the video-vs-photo branch.
public class SocialResponsibilityProjectsViewComponent : ViewComponent
{
    private const string Project1BasePath = "/images/sosyal-sorumluluk-projeleri/birinci-proje/web";
    private const string Project2BasePath = "/images/sosyal-sorumluluk-projeleri/ikinci-proje/web";

    public IViewComponentResult Invoke()
    {
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

        var model = new SocialResponsibilityProjectsViewModel
        {
            Projects = new List<SocialResponsibilityProjectItem>
            {
                BuildProject1(isEnglish),
                BuildProject2(isEnglish)
            }
        };

        return View(model);
    }

    // Project 1 — originals numbered 1.jpeg, 3.jpeg..37.jpeg (36 photos;
    // "2" is a video, not a gap). Featured file is "1.jpeg" per the
    // project owner's explicit instruction. Its one video (the original
    // "2.mp4", optimized to web/video.mp4 + web/video-poster.webp in the
    // previous task) is appended last, after all 35 other photos.
    //
    // English localization (2026-10-02) — heading/paragraphs/alt text are
    // natural corporate-English renderings, not literal translations, of
    // the Turkish copy below; grounded in the exact same photos/facts (see
    // the class-level comment), no new detail invented.
    private static SocialResponsibilityProjectItem BuildProject1(bool isEnglish)
    {
        var heading = isEnglish ? "Supporting Youth, Sport and Culture" : "Gençlik, Spor ve Kültüre Destek";
        var altSuffix = isEnglish ? "photo" : "fotoğraf";
        var videoAltSuffix = isEnglish ? "video" : "video";

        var otherMedia = Enumerable.Range(3, 35) // 3..37
            .Select(n => new SocialResponsibilityGalleryImage
            {
                Src = $"{Project1BasePath}/{n}.webp",
                Alt = $"{heading} — {altSuffix} {n}"
            })
            .ToList();

        otherMedia.Add(new SocialResponsibilityGalleryImage
        {
            Src = $"{Project1BasePath}/video-poster.webp",
            Alt = $"{heading} — {videoAltSuffix}",
            VideoSrc = $"{Project1BasePath}/video.mp4"
        });

        return new SocialResponsibilityProjectItem
        {
            SectionId = "social-responsibility-project-1",
            GalleryGroupKey = "sr-project-1",
            Heading = heading,
            Paragraphs = isEnglish
                ? new List<string>
                {
                    "As Ançın İnşaat, we support the young athletes and dancers who wear the Ançın " +
                        "jersey at youth sports events organized in cooperation with the Aydın " +
                        "Directorate of Youth and Sports.",
                    "Alongside their achievements on the field, we also welcome our young " +
                        "performers — who keep the region's cultural heritage alive — to our office " +
                        "to recognize their efforts."
                }
                : new List<string>
                {
                    "Ançın İnşaat olarak, Aydın Gençlik ve Spor İl Müdürlüğü iş birliğiyle düzenlenen " +
                        "gençlik spor etkinliklerinde Ançın forması taşıyan genç sporcu ve dansçılara destek " +
                        "veriyoruz.",
                    "Sahadaki başarılarının yanı sıra, bölgenin kültürel mirasını yaşatan genç " +
                        "performansçılarımızı da ofisimizde ağırlayarak emeklerini takdir ediyoruz."
                },
            FeaturedThumbnailSrc = $"{Project1BasePath}/featured.webp",
            FeaturedSrc = $"{Project1BasePath}/1.webp",
            FeaturedAlt = heading,
            OtherMedia = otherMedia,
            ImageOnRight = false
        };
    }

    // Project 2 — originals are WhatsApp exports; sorted chronologically by
    // their embedded capture timestamp and renumbered 01..56 for the
    // optimized web/ assets (see the sr-optimize script). Featured file is
    // chronologically first — "WhatsApp Image 2026-10-02 at 16.44.54.jpeg"
    // — per the project owner's explicit instruction. Its two videos
    // (optimized to web/video-1.mp4 + web/video-2.mp4, each with its own
    // WebP poster, in the previous task — video-1 is the original
    // "...16.44.54.mp4", video-2 is "...16.44.56.mp4") are appended last,
    // in that order, after all 55 other photos.
    private static SocialResponsibilityProjectItem BuildProject2(bool isEnglish)
    {
        var heading = isEnglish ? "Ançın \"Architects of the Future\" Event" : "Ançın Geleceğin Mimarları Etkinliği";
        var altSuffix = isEnglish ? "photo" : "fotoğraf";
        var videoAltSuffix = isEnglish ? "video" : "video";

        var otherMedia = Enumerable.Range(2, 55) // 02..56
            .Select(n => new SocialResponsibilityGalleryImage
            {
                Src = $"{Project2BasePath}/{n:D2}.webp",
                Alt = $"{heading} — {altSuffix} {n}"
            })
            .ToList();

        otherMedia.Add(new SocialResponsibilityGalleryImage
        {
            Src = $"{Project2BasePath}/video-1-poster.webp",
            Alt = $"{heading} — {videoAltSuffix} 1",
            VideoSrc = $"{Project2BasePath}/video-1.mp4"
        });
        otherMedia.Add(new SocialResponsibilityGalleryImage
        {
            Src = $"{Project2BasePath}/video-2-poster.webp",
            Alt = $"{heading} — {videoAltSuffix} 2",
            VideoSrc = $"{Project2BasePath}/video-2.mp4"
        });

        return new SocialResponsibilityProjectItem
        {
            SectionId = "social-responsibility-project-2",
            GalleryGroupKey = "sr-project-2",
            Heading = heading,
            Paragraphs = isEnglish
                ? new List<string>
                {
                    "To mark April 23rd National Sovereignty and Children's Day, we held the " +
                        "\"Ançın Architects of the Future\" event at Aydın OPSmall Mall. Wearing their " +
                        "own Ançın hard hats and vests, our youngest engineers experienced " +
                        "construction firsthand.",
                    "Held in partnership with Neşeli Oyunlar Etkinlik & Oyun Atölyesi, the event " +
                        "gave children an enjoyable time at a mini construction site, a junior " +
                        "engineer's workshop and a folk dance performance."
                }
                : new List<string>
                {
                    "23 Nisan Ulusal Egemenlik ve Çocuk Bayramı kapsamında, Aydın OPSmall AVM'de " +
                        "\"Ançın Geleceğin Mimarları Etkinliği\"ni düzenledik. Ançın bareti ve yeleğiyle " +
                        "minik mühendisler, inşaatı kendi elleriyle deneyimledi.",
                    "Neşeli Oyunlar Etkinlik & Oyun Atölyesi iş birliğiyle hayata geçirdiğimiz etkinlikte " +
                        "çocuklar; küçük inşaat alanında, minik mühendis atölyesinde ve halk oyunları " +
                        "gösterisinde keyifli saatler geçirdi."
                },
            // On-page card image intentionally reuses the full-resolution
            // 01.webp (same lightbox-tier asset as FeaturedSrc below)
            // rather than the separate, smaller "featured" tier every other
            // project uses — 2026-10-02 fix, project owner-requested: the
            // flyer's printed title sits in its top third, and the card's
            // default center object-position cropped it away. See
            // site.css's #social-responsibility-project-2 object-position
            // override, which depends on this being the same source image.
            FeaturedThumbnailSrc = $"{Project2BasePath}/01.webp",
            FeaturedSrc = $"{Project2BasePath}/01.webp",
            FeaturedAlt = heading,
            OtherMedia = otherMedia,
            ImageOnRight = true
        };
    }
}
