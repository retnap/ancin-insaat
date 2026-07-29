using AncinInsaat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AncinInsaat.Data.Seed;

// PLACEHOLDER DATA — realistic-but-fictional content per CLAUDE.md's
// Placeholder Content rules. Runtime seeder (not EF Core HasData) so this
// content can be edited or removed without creating a new migration, per
// the approved Milestone 3 decision.
//
// Project names/slugs (Nysa Gold, Le Jardin, Tralles Gold) are not invented
// here — they are the exact examples already used in 01_SiteMap.md's
// dynamic page routes. All other fields (descriptions, images, dates,
// partners) are placeholder and must be replaced with real client content
// before launch.
//
// 2026-07-28 (Projects Showcase section): extended to cover all 10 project
// wordmarks already confirmed real by the client via PartnerLogosViewComponent
// (Nlatis, Tralles Gold Residence, Alinda Gold Residence, Magnesia Gold
// Residence, La Fiore Karabağ, La Fiore Karabağ 2. Etap, Le Jardin, Lavia
// Kuyulu, Nysa Gold Residence, Dlatis Thermal Wellness Residence), so the
// Home page's new Projects Showcase can list every one of them via the same
// IProjectQueryService the Hero Banner already uses, rather than a second
// parallel list. Only Name/Slug/DisplayOrder/IsFeatured are informed by real
// confirmed facts (the wordmarks + which project the Hero already features);
// Status for the 7 projects newly added here is NOT a confirmed fact — it
// defaults to Ongoing and is clearly marked below for correction once the
// client confirms real status per project. Descriptions remain placeholder
// copy, same as the original 3.
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedSiteSettingsAsync(context);
        await SeedProjectsAsync(context);
        await SeedSeoMetadataAsync(context);
    }

    private static async Task SeedSiteSettingsAsync(AppDbContext context)
    {
        if (await context.SiteSettings.AnyAsync())
        {
            return;
        }

        context.SiteSettings.Add(new SiteSettings
        {
            CompanyName = "Ançın İnşaat",
            Address = "Kültür Mahallesi, İnşaat Caddesi No:12, Aydın, Türkiye",
            Phone = "+90 256 123 45 67",
            Email = "info@ancininsaat.com",
            WorkingHours = "Monday - Friday, 09:00 - 18:00",
            Facebook = "https://www.facebook.com/ancininsaat",
            Instagram = "https://www.instagram.com/ancininsaat",
            LinkedIn = "https://www.linkedin.com/company/ancininsaat",
            YouTube = "https://www.youtube.com/@ancininsaat",
            GoogleMaps = "https://maps.google.com/?q=Ancin+Insaat+Aydin",
            Logo = "/images/logos/ancin-logo.png",
            FooterText = "Building lasting value across Aydın, one project at a time."
        });

        await context.SaveChangesAsync();
    }

    // CanonicalUrl/OpenGraphImage are stored relative — ISeoService prefixes
    // them with the current request's scheme+host at read time, since no
    // production domain has been assigned yet (see 14_Decisions.md).
    private static async Task SeedSeoMetadataAsync(AppDbContext context)
    {
        if (await context.SeoMetadata.AnyAsync())
        {
            return;
        }

        context.SeoMetadata.Add(new SeoMetadata
        {
            Page = "home",
            MetaTitle = "Ançın İnşaat | Aydın'da Güvenilir Konut Projeleri",
            MetaDescription = "Ançın İnşaat, Aydın'da güven ve zanaatkârlıkla şekillenen konut projeleriyle yaşam alanlarını geleceğe taşıyor. Devam eden ve tamamlanan projelerimizi keşfedin.",
            CanonicalUrl = "/",
            OpenGraphImage = "/images/seo/og-home.webp"
        });

        await context.SaveChangesAsync();
    }

    private static async Task SeedProjectsAsync(AppDbContext context)
    {
        await ReconcileConfirmedProjectNamesAsync(context);

        var existingSlugs = new HashSet<string>(await context.Projects.Select(p => p.Slug).ToListAsync());

        var now = DateTime.UtcNow;

        // Asset paths below follow 07_AssetStructure.md's naming convention
        // but the files themselves do not exist yet (wwwroot/images/projects
        // only holds a .gitkeep) — expected to 404 until real media is added.
        var projects = new List<Project>
        {
            new()
            {
                Name = "Nysa Gold Residence",
                Slug = "nysa-gold",
                ShortDescription = "Placeholder short description for the Nysa Gold development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın, Türkiye",
                CompletionDate = null,
                CoverImage = "/images/projects/nysa-gold/cover.webp",
                DisplayOrder = 1,
                IsFeatured = true,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                Images = new List<ProjectImage>
                {
                    new() { ImagePath = "/images/projects/nysa-gold/gallery-01.webp", AltText = "Nysa Gold placeholder gallery image 1", DisplayOrder = 1 },
                    new() { ImagePath = "/images/projects/nysa-gold/gallery-02.webp", AltText = "Nysa Gold placeholder gallery image 2", DisplayOrder = 2 }
                },
                FloorPlans = new List<FloorPlan>
                {
                    new() { Title = "Type A", ImagePath = "/images/projects/nysa-gold/floorplan-a.webp", DisplayOrder = 1 }
                },
                Partners = new List<Partner>
                {
                    new() { Name = "Partner 01", Logo = "/images/partners/partner-01.webp", DisplayOrder = 1 }
                }
            },
            new()
            {
                Name = "Le Jardin",
                Slug = "le-jardin",
                ShortDescription = "Placeholder short description for the Le Jardin development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Completed,
                Location = "Aydın, Türkiye",
                CompletionDate = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                CoverImage = "/images/projects/le-jardin/cover.webp",
                DisplayOrder = 2,
                IsFeatured = true,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                Images = new List<ProjectImage>
                {
                    new() { ImagePath = "/images/projects/le-jardin/gallery-01.webp", AltText = "Le Jardin placeholder gallery image 1", DisplayOrder = 1 }
                },
                FloorPlans = new List<FloorPlan>
                {
                    new() { Title = "Type A", ImagePath = "/images/projects/le-jardin/floorplan-a.webp", DisplayOrder = 1 }
                },
                Partners = new List<Partner>
                {
                    new() { Name = "Partner 02", Logo = "/images/partners/partner-02.webp", DisplayOrder = 1 }
                }
            },
            new()
            {
                Name = "Tralles Gold Residence",
                Slug = "tralles-gold",
                ShortDescription = "Placeholder short description for the Tralles Gold development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın, Türkiye",
                CompletionDate = null,
                CoverImage = "/images/projects/tralles-gold/cover.webp",
                DisplayOrder = 3,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                Images = new List<ProjectImage>
                {
                    new() { ImagePath = "/images/projects/tralles-gold/gallery-01.webp", AltText = "Tralles Gold placeholder gallery image 1", DisplayOrder = 1 }
                },
                FloorPlans = new List<FloorPlan>
                {
                    new() { Title = "Type A", ImagePath = "/images/projects/tralles-gold/floorplan-a.webp", DisplayOrder = 1 }
                },
                Partners = new List<Partner>
                {
                    new() { Name = "Partner 03", Logo = "/images/partners/partner-03.webp", DisplayOrder = 1 }
                }
            },

            // 2026-07-28 — added for the Home page Projects Showcase section.
            // Names/order below follow PartnerLogosViewComponent's confirmed
            // real wordmarks; Status is NOT a confirmed fact for any of these
            // 7 (defaults to Ongoing per the project owner's instruction) and
            // Images/FloorPlans/Partners are intentionally left empty — no
            // detail-page content has been requested or supplied yet.
            new()
            {
                Name = "Nlatis",
                Slug = "nlatis",
                ShortDescription = "Placeholder short description for the Nlatis development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın, Türkiye",
                CompletionDate = null,
                CoverImage = "/images/projects/nlatis/cover.webp",
                DisplayOrder = 4,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "Alinda Gold Residence",
                Slug = "alinda-gold",
                ShortDescription = "Placeholder short description for the Alinda Gold Residence development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın, Türkiye",
                CompletionDate = null,
                CoverImage = "/images/projects/alinda-gold/cover.webp",
                DisplayOrder = 5,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "Magnesia Gold Residence",
                Slug = "magnesia-gold",
                ShortDescription = "Placeholder short description for the Magnesia Gold Residence development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın, Türkiye",
                CompletionDate = null,
                CoverImage = "/images/projects/magnesia-gold/cover.webp",
                DisplayOrder = 6,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "La Fiore Karabağ",
                Slug = "la-fiore-karabag",
                ShortDescription = "Placeholder short description for the La Fiore Karabağ development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın, Türkiye",
                CompletionDate = null,
                CoverImage = "/images/projects/la-fiore-karabag/cover.webp",
                DisplayOrder = 7,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "La Fiore Karabağ 2. Etap",
                Slug = "la-fiore-karabag-2-etap",
                ShortDescription = "Placeholder short description for the La Fiore Karabağ 2. Etap development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın, Türkiye",
                CompletionDate = null,
                CoverImage = "/images/projects/la-fiore-karabag-2-etap/cover.webp",
                DisplayOrder = 8,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "Lavia Kuyulu",
                Slug = "lavia-kuyulu",
                ShortDescription = "Placeholder short description for the Lavia Kuyulu development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın, Türkiye",
                CompletionDate = null,
                CoverImage = "/images/projects/lavia-kuyulu/cover.webp",
                DisplayOrder = 9,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new()
            {
                Name = "Dlatis Thermal Wellness Residence",
                Slug = "dlatis-thermal-wellness",
                ShortDescription = "Placeholder short description for the Dlatis Thermal Wellness Residence development.",
                Description = "Placeholder full description. Replace with approved project copy before launch.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın, Türkiye",
                CompletionDate = null,
                CoverImage = "/images/projects/dlatis-thermal-wellness/cover.webp",
                DisplayOrder = 10,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now
            }
        };

        var missingProjects = projects.Where(p => !existingSlugs.Contains(p.Slug)).ToList();
        if (missingProjects.Count == 0)
        {
            return;
        }

        context.Projects.AddRange(missingProjects);
        await context.SaveChangesAsync();
    }

    // Not a seed — a one-time reconciliation for 2 rows seeded before the
    // client confirmed (2026-07-28, see PartnerLogosViewComponent) their
    // full real names include "Residence". Safe to run on every startup:
    // nothing reads Project.Name publicly yet (Hero Banner's heading is a
    // separate hardcoded string), so this is the first place it reaches a
    // page, and the check is a no-op once names already match.
    private static async Task ReconcileConfirmedProjectNamesAsync(AppDbContext context)
    {
        var confirmedNames = new Dictionary<string, string>
        {
            ["nysa-gold"] = "Nysa Gold Residence",
            ["tralles-gold"] = "Tralles Gold Residence"
        };

        var projectsToCheck = await context.Projects
            .Where(p => confirmedNames.Keys.Contains(p.Slug))
            .ToListAsync();

        var changed = false;
        foreach (var project in projectsToCheck)
        {
            var confirmedName = confirmedNames[project.Slug];
            if (project.Name != confirmedName)
            {
                project.Name = confirmedName;
                changed = true;
            }
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }
}
