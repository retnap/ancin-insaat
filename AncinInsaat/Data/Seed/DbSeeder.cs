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
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedSiteSettingsAsync(context);
        await SeedProjectsAsync(context);
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

    private static async Task SeedProjectsAsync(AppDbContext context)
    {
        if (await context.Projects.AnyAsync())
        {
            return;
        }

        var now = DateTime.UtcNow;

        // Asset paths below follow 07_AssetStructure.md's naming convention
        // but the files themselves do not exist yet (wwwroot/images/projects
        // only holds a .gitkeep) — expected to 404 until real media is added.
        var projects = new List<Project>
        {
            new()
            {
                Name = "Nysa Gold",
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
                Name = "Tralles Gold",
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
            }
        };

        context.Projects.AddRange(projects);
        await context.SaveChangesAsync();
    }
}
