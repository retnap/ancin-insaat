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
// ProjectType (added for the Projects listing's Project Type filter) is set
// to "Residence" for all 10 seeded projects below — the one classification
// their names actually support (…Residence/…Gold housing developments) — as
// a placeholder default pending the client's real per-project
// classification (Villa / Commercial / Office / Mixed Use are also valid
// per 08_DatabasePlan.md's taxonomy but would be invented facts if applied
// here without confirmation).
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
        await SeedCareerPositionsAsync(context);
        await ReconcileCareerPositionPlaceholderPrefixAsync(context);
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
            CompanyName = "Ancın İnşaat",
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

    // PLACEHOLDER DATA (Career page, 2026-08-03) — the client has not yet
    // supplied real open positions. Titles/departments are realistic for a
    // construction company but must be replaced with real listings before
    // launch, same rule as every other placeholder in this file. Guarded
    // per-Title (CareerPosition has no slug/unique key) rather than a
    // single blanket "table is empty" check, matching SeedProjectsAsync's
    // per-slug pattern.
    private static async Task SeedCareerPositionsAsync(AppDbContext context)
    {
        var existingTitles = new HashSet<string>(await context.CareerPositions.Select(c => c.Title).ToListAsync());

        var now = DateTime.UtcNow;

        var positions = new List<CareerPosition>
        {
            new()
            {
                Title = "Şantiye Şefi",
                Department = "İnşaat",
                Location = "Aydın, Türkiye",
                Description = "Devam eden konut projelerimizde saha " +
                    "operasyonlarını yönetecek, iş güvenliği ve kalite standartlarını denetleyecek deneyimli " +
                    "bir Şantiye Şefi arıyoruz.",
                IsPublished = true,
                CreatedAt = now
            },
            new()
            {
                Title = "İnşaat Mühendisi",
                Department = "Teknik Ofis",
                Location = "Aydın, Türkiye",
                Description = "Proje uygulama süreçlerinde teknik ofis " +
                    "ekibimize katılacak, metraj ve keşif çalışmalarında görev alacak bir İnşaat Mühendisi " +
                    "arıyoruz.",
                IsPublished = true,
                CreatedAt = now
            },
            new()
            {
                Title = "Muhasebe Uzmanı",
                Department = "Finans",
                Location = "Aydın, Türkiye",
                Description = "Şirketimizin finans ve muhasebe " +
                    "süreçlerini yürütecek, detay odaklı bir Muhasebe Uzmanı arıyoruz.",
                IsPublished = true,
                CreatedAt = now
            }
        };

        var missingPositions = positions.Where(p => !existingTitles.Contains(p.Title)).ToList();
        if (missingPositions.Count == 0)
        {
            return;
        }

        context.CareerPositions.AddRange(missingPositions);
        await context.SaveChangesAsync();
    }

    // Not a seed — strips the literal "Placeholder position description. "
    // prefix that used to lead all 3 CareerPosition.Description values above
    // (Placeholder Content sweep, 2026-08-20 client request) off any
    // already-seeded row that still has it; the sentence following the
    // prefix was already real copy, so only the prefix itself is removed.
    // Guarded per-row (StartsWith check) rather than a full-string equality
    // match, same idempotent "no-op once already fixed" shape as every other
    // Reconcile* method in this file.
    private static async Task ReconcileCareerPositionPlaceholderPrefixAsync(AppDbContext context)
    {
        const string placeholderPrefix = "Placeholder position description. ";

        var positions = await context.CareerPositions
            .Where(p => p.Description.StartsWith(placeholderPrefix))
            .ToListAsync();

        if (positions.Count == 0)
        {
            return;
        }

        foreach (var position in positions)
        {
            position.Description = position.Description[placeholderPrefix.Length..];
        }

        await context.SaveChangesAsync();
    }

    // CanonicalUrl/OpenGraphImage are stored relative — ISeoService prefixes
    // them with the current request's scheme+host at read time, since no
    // production domain has been assigned yet (see 14_Decisions.md).
    //
    // Seeded per-page (like SeedProjectsAsync's per-slug check) rather than
    // behind a single blanket "table is empty" guard, so a page added after
    // the first deploy (e.g. "projects" here) still gets its row inserted
    // on a database that already seeded "home".
    //
    // 2026-07-31 (Project Detail Foundation): extended with one row per
    // project, keyed by the project's own Slug — ProjectsController.Details
    // calls the exact same ISeoService.GetPageSeoAsync(pageKey, ...) that
    // "home"/"projects" already use, just with a per-project pageKey, so
    // Project Detail needs no second SEO mechanism. MetaTitle/MetaDescription
    // are placeholder copy per CLAUDE.md's Placeholder Content rules;
    // OpenGraphImage reuses each project's own cover image rather than the
    // shared Home OG image, since 10_SEO.md calls for a real per-project image.
    private static async Task SeedSeoMetadataAsync(AppDbContext context)
    {
        var existingPages = new HashSet<string>(await context.SeoMetadata.Select(s => s.Page).ToListAsync());

        var pages = new List<SeoMetadata>
        {
            new()
            {
                Page = "home",
                MetaTitle = "Ancın İnşaat | Aydın'da Güvenilir Konut Projeleri",
                MetaDescription = "Ancın İnşaat, Aydın'da güven ve zanaatkârlıkla şekillenen konut projeleriyle yaşam alanlarını geleceğe taşıyor. Devam eden ve tamamlanan projelerimizi keşfedin.",
                CanonicalUrl = "/",
                OpenGraphImage = "/images/seo/og-home.webp"
            },

            // OpenGraphImage placeholder-reuses the Home OG image — no
            // Projects-specific one has been supplied yet; replace before
            // launch, same as MetaTitle/MetaDescription below.
            new()
            {
                Page = "projects",
                MetaTitle = "Projelerimiz | Ancın İnşaat",
                MetaDescription = "Ancın İnşaat'ın Aydın'da tamamladığı ve devam eden tüm konut projelerini keşfedin.",
                CanonicalUrl = "/projects",
                OpenGraphImage = "/images/seo/og-home.webp"
            },

            new()
            {
                Page = "contact",
                MetaTitle = "İletişim | Ancın İnşaat",
                MetaDescription = "Ancın İnşaat ile iletişime geçin. Adres, telefon, e-posta bilgilerimiz ve iletişim formumuz.",
                CanonicalUrl = "/contact",
                OpenGraphImage = "/images/seo/og-home.webp"
            },

            // About Us Foundation phase (2026-08-01). OpenGraphImage
            // placeholder-reuses the Home OG image, same as Projects/Contact
            // above — no About-specific one has been supplied yet.
            new()
            {
                Page = "about-us",
                MetaTitle = "Hakkımızda | Ancın İnşaat",
                MetaDescription = "Ancın İnşaat'ın 1973'ten bugüne uzanan hikayesini, değerlerini ve Aydın'daki yolculuğunu keşfedin.",
                CanonicalUrl = "/about-us",
                OpenGraphImage = "/images/seo/og-home.webp"
            },

            // Our Values (2026-08-03). OpenGraphImage placeholder-reuses the
            // Home OG image, same as About Us above — no Values-specific one
            // has been supplied yet.
            new()
            {
                Page = "values",
                MetaTitle = "Değerlerimiz | Ancın İnşaat",
                MetaDescription = "Ancın İnşaat'ın kurumsal kültürünü ve inşaat anlayışını şekillendiren temel değerleri keşfedin.",
                CanonicalUrl = "/values",
                OpenGraphImage = "/images/seo/og-home.webp"
            },

            // Career (2026-08-03). OpenGraphImage placeholder-reuses the Home
            // OG image, same as every other corporate page above — no
            // Career-specific one has been supplied yet.
            new()
            {
                Page = "career",
                MetaTitle = "Kariyer | Ancın İnşaat",
                MetaDescription = "Ancın İnşaat'ta kariyer fırsatlarını keşfedin, açık pozisyonlarımıza CV'nizle başvurun.",
                CanonicalUrl = "/career",
                OpenGraphImage = "/images/seo/og-home.webp"
            },

            // HR Policy (2026-08-03). OpenGraphImage placeholder-reuses the
            // Home OG image, same as every other corporate page above — no
            // HR-Policy-specific one has been supplied yet.
            new()
            {
                Page = "hr-policy",
                MetaTitle = "İnsan Kaynakları Politikası | Ancın İnşaat",
                MetaDescription = "Ancın İnşaat'ın çalışan gelişimi, kurum kültürü ve insan kaynakları ilkelerini keşfedin.",
                CanonicalUrl = "/hr-policy",
                OpenGraphImage = "/images/seo/og-home.webp"
            },

            // KVKK (2026-08-03). OpenGraphImage placeholder-reuses the Home
            // OG image, same as every other corporate page above — no
            // KVKK-specific one has been supplied yet.
            new()
            {
                Page = "kvkk",
                MetaTitle = "KVKK Aydınlatma Metni | Ancın İnşaat",
                MetaDescription = "Ancın İnşaat'ın 6698 sayılı Kişisel Verilerin Korunması Kanunu (KVKK) kapsamında kişisel verilerin işlenmesine ilişkin aydınlatma metni.",
                CanonicalUrl = "/kvkk",
                OpenGraphImage = "/images/seo/og-home.webp"
            }
        };

        pages.AddRange(BuildProjectSeoMetadata());

        var missingPages = pages.Where(p => !existingPages.Contains(p.Page)).ToList();
        if (missingPages.Count == 0)
        {
            return;
        }

        context.SeoMetadata.AddRange(missingPages);
        await context.SaveChangesAsync();
    }

    // Names/slugs mirror SeedProjectsAsync's project list exactly — kept as
    // a second, explicit list (rather than deriving it from the Project
    // seed data) so this method has no ordering dependency on
    // SeedProjectsAsync having already run in the same call.
    private static IEnumerable<SeoMetadata> BuildProjectSeoMetadata()
    {
        var projectNames = new (string Slug, string Name)[]
        {
            ("nysa-gold", "Nysa Gold Residence"),
            ("le-jardin", "Le Jardin"),
            ("tralles-gold", "Tralles Gold Residence"),
            ("nlatis", "Nlatis"),
            ("alinda-gold", "Alinda Gold Residence"),
            ("magnesia-gold", "Magnesia Gold Residence"),
            ("la-fiore-karabag", "La Fiore Karabağ"),
            ("la-fiore-karabag-2-etap", "La Fiore Karabağ 2. Etap"),
            ("kuyulu-la-via-villalar-birinci-etap", "La Via Villalar 1. Etap"),
            ("davutlar-d-latis", "Davutlar D Latis"),
            ("ferhunde-hanim-apt", "Ferhunde Hanım Apt."),
            ("q-latis", "Hacıfeyzullah - Q-Latis")
        };

        return projectNames.Select(p => new SeoMetadata
        {
            Page = p.Slug,
            MetaTitle = $"{p.Name} | Ancın İnşaat",
            MetaDescription = $"Ancın İnşaat'ın Aydın'daki {p.Name} projesini keşfedin. Konum, durum ve proje detayları.",
            CanonicalUrl = $"/projects/{p.Slug}",
            OpenGraphImage = $"/images/projects/{p.Slug}/cover.webp"
        });
    }

    // Shared placeholder Floor Plans entry (Floor Plans Availability Audit,
    // 2026-08-01) — a single "2+1" apartment type with the same figures Le
    // Jardin's "2+1" entry already used, applied to every project that has
    // no real floor plan data yet, so 03_PageBlueprints.md's Daire Planları
    // section renders consistently across the whole catalogue rather than
    // only for the 2 projects it happened to be seeded for originally.
    // ImagePath still points at a file that does not exist per project
    // (_FloorPlans.cshtml shows its placeholder graphic instead, never this
    // path) — see docs/14_Decisions.md.
    private static List<FloorPlan> BuildPlaceholderFloorPlans(string slug)
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "2+1",
                ImagePath = $"/images/projects/{slug}/floorplan-2-1.webp",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            }
        };
    }

    // Real gallery photos for Davutlar D Latis (2026-08-05) — every file the
    // client placed under wwwroot/images/projects/davutlar-d-latis, except
    // 3A.jpeg: dis-mekan-gorselleri → "Exterior" (45), ic-mekan-gorselleri →
    // "Interior" (65, including the 4 per-unit-type subfolders — the client
    // asked for every image in that folder to count as Interior for now,
    // to be recategorized later), sosyal-olanaklar → "Social Areas" (17).
    // 3A.jpeg is deliberately excluded (2026-08-05 client fix) — it's the
    // source photo behind banner.webp, so it already appears once, full-
    // bleed, as this project's Hero Banner; keeping it as a Gallery card
    // too duplicated that exact photo right below the Hero. 3B.jpeg's own
    // card (right next to where 3A's used to sit) stands in as the
    // Exterior category's representative shot instead.
    // DisplayOrder is continuous across the 3 categories so "Tüm Görseller"
    // reads Exterior, then Interior, then Social Areas — though
    // _ProjectGallery.cshtml actually enforces that ordering itself via
    // GalleryCategorySortOrder, grouping by category regardless of
    // DisplayOrder (the gap left at 7 by 3A.jpeg's removal is harmless).
    // Extracted into its own method (rather than inlined like every other
    // project's much shorter Images list) purely because of its size.
    private static List<ProjectImage> BuildDavutlarDLatisImages()
    {
        return new List<ProjectImage>
        {
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-01.jpg", AltText = "Davutlar D Latis dış cephe görünümü 1", DisplayOrder = 1, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-02.jpg", AltText = "Davutlar D Latis dış cephe görünümü 2", DisplayOrder = 2, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-03.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 3", DisplayOrder = 3, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-04.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 4", DisplayOrder = 4, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-05.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 5", DisplayOrder = 5, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-06.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 6", DisplayOrder = 6, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-07.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 8", DisplayOrder = 8, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-08.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 9", DisplayOrder = 9, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-09.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 10", DisplayOrder = 10, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-10.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 11", DisplayOrder = 11, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-11.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 12", DisplayOrder = 12, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-12.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 13", DisplayOrder = 13, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-13.jpg", AltText = "Davutlar D Latis dış cephe görünümü 14", DisplayOrder = 14, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-14.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 15", DisplayOrder = 15, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-15.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 16", DisplayOrder = 16, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-16.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 17", DisplayOrder = 17, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-17.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 18", DisplayOrder = 18, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-18.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 19", DisplayOrder = 19, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-19.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 20", DisplayOrder = 20, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-20.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 21", DisplayOrder = 21, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-21.jpg", AltText = "Davutlar D Latis dış cephe görünümü 22", DisplayOrder = 22, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-22.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 23", DisplayOrder = 23, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-23.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 24", DisplayOrder = 24, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-24.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 25", DisplayOrder = 25, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-25.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 26", DisplayOrder = 26, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-26.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 27", DisplayOrder = 27, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-27.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 28", DisplayOrder = 28, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-28.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 29", DisplayOrder = 29, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-29.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 30", DisplayOrder = 30, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-30.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 31", DisplayOrder = 31, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-31.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 32", DisplayOrder = 32, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-32.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 33", DisplayOrder = 33, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-33.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 34", DisplayOrder = 34, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-34.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 35", DisplayOrder = 35, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-35.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 36", DisplayOrder = 36, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-36.jpg", AltText = "Davutlar D Latis dış cephe görünümü 37", DisplayOrder = 37, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-37.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 38", DisplayOrder = 38, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-38.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 39", DisplayOrder = 39, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-39.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 40", DisplayOrder = 40, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-40.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 41", DisplayOrder = 41, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-41.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 42", DisplayOrder = 42, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-42.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 43", DisplayOrder = 43, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-43.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 44", DisplayOrder = 44, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-44.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 45", DisplayOrder = 45, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/exterior/originals/exterior-45.jpeg", AltText = "Davutlar D Latis dış cephe görünümü 46", DisplayOrder = 46, Category = "Exterior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-01.jpg", AltText = "Davutlar D Latis iç mekan görünümü 1", DisplayOrder = 47, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-02.jpeg", AltText = "Davutlar D Latis iç mekan görünümü 2", DisplayOrder = 48, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-03.jpg", AltText = "Davutlar D Latis iç mekan görünümü 3", DisplayOrder = 49, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-04.jpg", AltText = "Davutlar D Latis iç mekan görünümü 4", DisplayOrder = 50, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-05.jpg", AltText = "Davutlar D Latis iç mekan görünümü 5", DisplayOrder = 51, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-06.jpeg", AltText = "Davutlar D Latis iç mekan görünümü 6", DisplayOrder = 52, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-07.jpg", AltText = "Davutlar D Latis iç mekan görünümü 7", DisplayOrder = 53, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-08.jpeg", AltText = "Davutlar D Latis iç mekan görünümü 8", DisplayOrder = 54, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-09.jpeg", AltText = "Davutlar D Latis iç mekan görünümü 9", DisplayOrder = 55, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-10.jpg", AltText = "Davutlar D Latis iç mekan görünümü 10", DisplayOrder = 56, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-11.jpg", AltText = "Davutlar D Latis iç mekan görünümü 11", DisplayOrder = 57, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-12.jpg", AltText = "Davutlar D Latis iç mekan görünümü 12", DisplayOrder = 58, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-13.jpg", AltText = "Davutlar D Latis iç mekan görünümü 13", DisplayOrder = 59, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-14.jpg", AltText = "Davutlar D Latis iç mekan görünümü 14", DisplayOrder = 60, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-15.jpg", AltText = "Davutlar D Latis iç mekan görünümü 15", DisplayOrder = 61, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-16.jpg", AltText = "Davutlar D Latis iç mekan görünümü 16", DisplayOrder = 62, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-17.jpg", AltText = "Davutlar D Latis iç mekan görünümü 17", DisplayOrder = 63, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-18.jpg", AltText = "Davutlar D Latis iç mekan görünümü 18", DisplayOrder = 64, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-19.jpg", AltText = "Davutlar D Latis iç mekan görünümü 19", DisplayOrder = 65, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-20.jpg", AltText = "Davutlar D Latis iç mekan görünümü 20", DisplayOrder = 66, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-21.jpg", AltText = "Davutlar D Latis iç mekan görünümü 21", DisplayOrder = 67, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-22.jpg", AltText = "Davutlar D Latis iç mekan görünümü 22", DisplayOrder = 68, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-23.jpg", AltText = "Davutlar D Latis iç mekan görünümü 23", DisplayOrder = 69, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-24.jpg", AltText = "Davutlar D Latis iç mekan görünümü 24", DisplayOrder = 70, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-25.jpg", AltText = "Davutlar D Latis iç mekan görünümü 25", DisplayOrder = 71, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-26.jpg", AltText = "Davutlar D Latis iç mekan görünümü 26", DisplayOrder = 72, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-27.jpg", AltText = "Davutlar D Latis iç mekan görünümü 27", DisplayOrder = 73, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-28.jpg", AltText = "Davutlar D Latis iç mekan görünümü 28", DisplayOrder = 74, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-29.jpg", AltText = "Davutlar D Latis iç mekan görünümü 29", DisplayOrder = 75, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-30.jpg", AltText = "Davutlar D Latis iç mekan görünümü 30", DisplayOrder = 76, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-31.jpg", AltText = "Davutlar D Latis iç mekan görünümü 31", DisplayOrder = 77, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-32.jpg", AltText = "Davutlar D Latis iç mekan görünümü 32", DisplayOrder = 78, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-33.jpg", AltText = "Davutlar D Latis iç mekan görünümü 33", DisplayOrder = 79, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-34.jpg", AltText = "Davutlar D Latis iç mekan görünümü 34", DisplayOrder = 80, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-35.jpg", AltText = "Davutlar D Latis iç mekan görünümü 35", DisplayOrder = 81, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-36.jpg", AltText = "Davutlar D Latis iç mekan görünümü 36", DisplayOrder = 82, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-37.jpg", AltText = "Davutlar D Latis iç mekan görünümü 37", DisplayOrder = 83, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-38.jpg", AltText = "Davutlar D Latis iç mekan görünümü 38", DisplayOrder = 84, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-39.jpg", AltText = "Davutlar D Latis iç mekan görünümü 39", DisplayOrder = 85, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-40.jpg", AltText = "Davutlar D Latis iç mekan görünümü 40", DisplayOrder = 86, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-41.jpg", AltText = "Davutlar D Latis iç mekan görünümü 41", DisplayOrder = 87, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-42.jpg", AltText = "Davutlar D Latis iç mekan görünümü 42", DisplayOrder = 88, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-43.jpg", AltText = "Davutlar D Latis iç mekan görünümü 43", DisplayOrder = 89, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-44.jpg", AltText = "Davutlar D Latis iç mekan görünümü 44", DisplayOrder = 90, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-45.jpg", AltText = "Davutlar D Latis iç mekan görünümü 45", DisplayOrder = 91, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-46.jpg", AltText = "Davutlar D Latis iç mekan görünümü 46", DisplayOrder = 92, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-47.jpg", AltText = "Davutlar D Latis iç mekan görünümü 47", DisplayOrder = 93, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-48.jpg", AltText = "Davutlar D Latis iç mekan görünümü 48", DisplayOrder = 94, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-49.jpg", AltText = "Davutlar D Latis iç mekan görünümü 49", DisplayOrder = 95, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-50.jpg", AltText = "Davutlar D Latis iç mekan görünümü 50", DisplayOrder = 96, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-51.jpg", AltText = "Davutlar D Latis iç mekan görünümü 51", DisplayOrder = 97, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-52.jpg", AltText = "Davutlar D Latis iç mekan görünümü 52", DisplayOrder = 98, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-53.jpg", AltText = "Davutlar D Latis iç mekan görünümü 53", DisplayOrder = 99, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-54.jpg", AltText = "Davutlar D Latis iç mekan görünümü 54", DisplayOrder = 100, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-55.jpg", AltText = "Davutlar D Latis iç mekan görünümü 55", DisplayOrder = 101, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-56.jpg", AltText = "Davutlar D Latis iç mekan görünümü 56", DisplayOrder = 102, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-57.jpg", AltText = "Davutlar D Latis iç mekan görünümü 57", DisplayOrder = 103, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-58.jpg", AltText = "Davutlar D Latis iç mekan görünümü 58", DisplayOrder = 104, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-59.jpg", AltText = "Davutlar D Latis iç mekan görünümü 59", DisplayOrder = 105, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-60.jpg", AltText = "Davutlar D Latis iç mekan görünümü 60", DisplayOrder = 106, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-61.jpg", AltText = "Davutlar D Latis iç mekan görünümü 61", DisplayOrder = 107, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-62.jpg", AltText = "Davutlar D Latis iç mekan görünümü 62", DisplayOrder = 108, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-63.jpg", AltText = "Davutlar D Latis iç mekan görünümü 63", DisplayOrder = 109, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-64.jpg", AltText = "Davutlar D Latis iç mekan görünümü 64", DisplayOrder = 110, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/interior/originals/interior-65.jpg", AltText = "Davutlar D Latis iç mekan görünümü 65", DisplayOrder = 111, Category = "Interior" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-01.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 1", DisplayOrder = 112, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-02.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 2", DisplayOrder = 113, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-03.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 3", DisplayOrder = 114, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-04.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 4", DisplayOrder = 115, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-05.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 5", DisplayOrder = 116, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-06.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 6", DisplayOrder = 117, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-07.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 7", DisplayOrder = 118, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-08.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 8", DisplayOrder = 119, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-09.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 9", DisplayOrder = 120, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-10.png", AltText = "Davutlar D Latis sosyal alan görünümü 10", DisplayOrder = 121, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-11.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 11", DisplayOrder = 122, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-12.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 12", DisplayOrder = 123, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-13.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 13", DisplayOrder = 124, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-14.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 14", DisplayOrder = 125, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-15.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 15", DisplayOrder = 126, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-16.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 16", DisplayOrder = 127, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/davutlar-d-latis/gallery/social/originals/social-17.jpg", AltText = "Davutlar D Latis sosyal alan görünümü 17", DisplayOrder = 128, Category = "Social Areas" }
        };
    }

    // Real gallery photos for Ferhunde Hanım Apt. (2026-08-05, revised
    // 2026-08-10). dis-mekan-gorselleri → "Exterior" (20, incl. the
    // client's separately-named "GÖRSEL DIŞ CEPHE.jpg" hero shot, moved into
    // gallery/exterior/originals and renamed exterior-01..20 in the source
    // folder's numeric order). ic-mekan-gorselleri's 4 real subfolders → 47
    // "Interior" rows, each tagged with ProjectImage.Block so
    // _ProjectGallery.cshtml's block-chip row (the La Fiore Karabağ 2. Etap
    // pilot) surfaces "A Tip" / "B Tip" / "C Tip" / "Zemin Kat Daire" filter
    // buttons under İç Mekan Görselleri only — never under Tüm Görseller,
    // same shared JS rule, no code change. No sosyal-olanaklar folder was
    // supplied, so there is no "Social Areas" entry — the Gallery derives
    // its categories from whichever Category values actually exist per
    // project (ProjectsController.Details), so this project's dropdown
    // simply never surfaces that category. DisplayOrder is continuous across
    // categories so "Tüm Görseller" reads Exterior then Interior.
    private static List<ProjectImage> BuildFerhundeHanimAptImages()
    {
        var images = new List<ProjectImage>();
        var order = 1;

        for (var i = 1; i <= 20; i++)
        {
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/ferhunde-hanim-apt/gallery/exterior/originals/exterior-{i:00}.jpg",
                AltText = $"Ferhunde Hanım Apt. dış cephe görünümü {i}",
                DisplayOrder = order++,
                Category = "Exterior"
            });
        }

        var interiorGroups = new (string Folder, string Block, int Count)[]
        {
            ("a-tipi", "A Tip", 14),
            ("b-tipi", "B Tip", 14),
            ("c-tipi", "C Tip", 12),
            ("zemin-kat", "Zemin Kat Daire", 7)
        };

        foreach (var group in interiorGroups)
        {
            for (var i = 1; i <= group.Count; i++)
            {
                images.Add(new ProjectImage
                {
                    ImagePath = $"/images/projects/ferhunde-hanim-apt/gallery/interior/{group.Folder}/originals/{group.Folder}-{i:00}.jpg",
                    AltText = $"Ferhunde Hanım Apt. {group.Block} iç mekan görünümü {i}",
                    DisplayOrder = order++,
                    Category = "Interior",
                    Block = group.Block
                });
            }
        }

        // "Sosyal Alan" (Social Areas), client curation, 2026-08-20 — copies/
        // references 5 of the real Exterior photos above (same ImagePath, a
        // second ProjectImage row with a different Category, no file
        // duplication) so they also surface in the Gallery's Social Areas
        // filter and feed the Social Facilities cards.
        var socialAreaSourceIndexes = new[] { 13, 16, 17, 18, 19 };
        var socialAreaIndex = 0;
        foreach (var sourceIndex in socialAreaSourceIndexes)
        {
            socialAreaIndex++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/ferhunde-hanim-apt/gallery/exterior/originals/exterior-{sourceIndex:00}.jpg",
                AltText = $"Ferhunde Hanım Apt. sosyal alan görünümü {socialAreaIndex}",
                DisplayOrder = order++,
                Category = "Social Areas"
            });
        }

        return images;
    }

    // Real architectural drawings for Ferhunde Hanım Apt. (2026-08-10) — the
    // client supplied 2 files under kat-planlari/: "1.2.4. KAT PLANI.jpg"
    // (identical floor plan shared by the 1st, 2nd and 4th floors) and
    // "3. KAT PLANI.jpg" (the 3rd floor's own layout). The shared drawing is
    // moved into floorplans/originals once (1-2-4-kat.jpg) and referenced by
    // 3 rows rather than duplicated on disk. No real per-unit specs were
    // supplied for either floor, so every row carries the same placeholder
    // Net/Gross/Sales-Gross stats and room list every other real-drawing
    // project uses (see BuildPlaceholderFloorPlans).
    private static List<FloorPlan> BuildFerhundeHanimAptFloorPlans()
    {
        FloorPlanRoom[] PlaceholderRooms() => new[]
        {
            new FloorPlanRoom { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
            new FloorPlanRoom { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
            new FloorPlanRoom { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
            new FloorPlanRoom { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
            new FloorPlanRoom { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
            new FloorPlanRoom { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
        };

        const string sharedFloorImage = "/images/projects/ferhunde-hanim-apt/floorplans/originals/1-2-4-kat.jpg";

        return new List<FloorPlan>
        {
            new() { ApartmentType = "1. Kat", ImagePath = sharedFloorImage, NetAreaM2 = 68.00m, GrossAreaM2 = 95.00m, SalesGrossAreaM2 = 78.00m, DisplayOrder = 1, Rooms = PlaceholderRooms().ToList() },
            new() { ApartmentType = "2. Kat", ImagePath = sharedFloorImage, NetAreaM2 = 68.00m, GrossAreaM2 = 95.00m, SalesGrossAreaM2 = 78.00m, DisplayOrder = 2, Rooms = PlaceholderRooms().ToList() },
            new() { ApartmentType = "3. Kat", ImagePath = "/images/projects/ferhunde-hanim-apt/floorplans/originals/3-kat.jpg", NetAreaM2 = 68.00m, GrossAreaM2 = 95.00m, SalesGrossAreaM2 = 78.00m, DisplayOrder = 3, Rooms = PlaceholderRooms().ToList() },
            new() { ApartmentType = "4. Kat", ImagePath = sharedFloorImage, NetAreaM2 = 68.00m, GrossAreaM2 = 95.00m, SalesGrossAreaM2 = 78.00m, DisplayOrder = 4, Rooms = PlaceholderRooms().ToList() }
        };
    }

    // Vaziyet Planı (2026-08-10) — client supplied one site-context photo
    // (vaziyet/CUMHURİYET886-10.jpg, identical to konum/CUMHURİYET886-10.jpg
    // used for LocationImagePath below — same source photo serves both
    // purposes, converted to 2 differently-sized WebP derivatives via
    // ThumbnailTool --single rather than stored twice). Single entry still
    // renders through the Hero's shared Media Viewer group exactly like
    // every multi-image project (see HeroBannerProjectDetail/Default.cshtml).
    private static List<ProjectSitePlanImage> BuildFerhundeHanimAptSitePlanImages()
    {
        return new List<ProjectSitePlanImage>
        {
            new() { ImagePath = "/images/projects/ferhunde-hanim-apt/site-plan-1.webp", AltText = "Ferhunde Hanım Apt. vaziyet planı", DisplayOrder = 1 }
        };
    }

    // Concept section's 3-slide image carousel (2026-08-10) — this project
    // has no concept video, so it gets the image-only carousel variant (same
    // markup/CSS/JS as La Fiore Karabağ 2. Etap/Nysa Gold's image slides,
    // including the shared nav-arrow position). The 3 images are the most
    // visually representative shots picked from this project's own Exterior
    // gallery pool: the dusk establishing render, the private garden/terrace
    // lounge, and the daytime corner architecture shot. Copy is
    // project-specific and only describes what each photo actually shows —
    // no invented facts (unit counts, amenities, delivery dates, etc. are
    // not asserted anywhere here).
    private static List<ProjectConceptImage> BuildFerhundeHanimAptConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/ferhunde-hanim-apt/gallery/exterior/originals/exterior-01.jpg",
                Eyebrow = "Aydın'ın Kalbinde",
                Title = "Zarif Bir Cepheyle Yükselen Kimlik",
                Description = "Ferhunde Hanım Apt., yumuşak hatlı balkonları ve özenle seçilmiş cephe dokusuyla Aydın Efeler'de dikkat çeken bir mimari kimlik sunuyor. Geniş camlar ve ferah balkonlar, her katta doğal ışığı içeri taşıyor.",
                DisplayOrder = 1
            },
            new()
            {
                ImagePath = "/images/projects/ferhunde-hanim-apt/gallery/exterior/originals/exterior-16.jpg",
                Eyebrow = "Kendi Yeşil Alanınızda",
                Title = "Şehrin İçinde Sakin Bir Bahçe",
                Description = "Yüksek çitlerle çevrili özel bahçe alanı, sakinlerine şehrin gürültüsünden uzak, güvenli ve huzurlu bir dış mekan yaşam alanı sunuyor. Palmiye ağaçları ve oturma gruplarıyla günün her saatinde keyifli bir mola noktası.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/ferhunde-hanim-apt/gallery/exterior/originals/exterior-20.jpg",
                Eyebrow = "Detaylarda Kalite",
                Title = "Güvenli ve Düzenli Bir Yerleşim",
                Description = "Çevre duvarları, peyzajlı ön bahçesi ve düzenli site içi düzeniyle Ferhunde Hanım Apt., günlük yaşamı kolaylaştıran ve değerini koruyan bir yatırım fırsatı sunuyor.",
                DisplayOrder = 3
            }
        };
    }

    // Real gallery photos for Kuyulu AVM (2026-08-06) — every file supplied
    // under wwwroot/images/projects/kuyulu-avm, renamed kuyulu-avm-01..15 in
    // numeric order and moved into gallery/originals, none skipped. Category
    // left null throughout: the client supplied only one grouping (no
    // dış mekan/iç mekan/sosyal alan split), so these never populate
    // GalleryCategories and the Gallery falls back to its plain "Tüm
    // Görseller" grid — see ProjectsController.Details and
    // _ProjectGallery.cshtml.
    // A 2026-08-09 change briefly tagged all 15 with Category = "Exterior".
    // Reverted (client revision, 2026-08-09) — a single named category still
    // makes GalleryCategories.Count == 1, which routes _ProjectGallery.cshtml
    // to its oldest plain-grid branch (no dropdown at all) instead of the
    // "Tüm Görseller"-only dropdown the client wants, so Category stays null
    // as originally supplied.
    private static List<ProjectImage> BuildKuyuluAvmImages()
    {
        return new List<ProjectImage>
        {
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-01.jpg", AltText = "La Via AVM görünümü 1", DisplayOrder = 1 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-02.jpg", AltText = "La Via AVM görünümü 2", DisplayOrder = 2 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-03.jpg", AltText = "La Via AVM görünümü 3", DisplayOrder = 3 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-04.jpg", AltText = "La Via AVM görünümü 4", DisplayOrder = 4 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-05.jpg", AltText = "La Via AVM görünümü 5", DisplayOrder = 5 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-06.jpg", AltText = "La Via AVM görünümü 6", DisplayOrder = 6 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-07.jpg", AltText = "La Via AVM görünümü 7", DisplayOrder = 7 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-08.jpg", AltText = "La Via AVM görünümü 8", DisplayOrder = 8 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-09.jpg", AltText = "La Via AVM görünümü 9", DisplayOrder = 9 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-10.jpg", AltText = "La Via AVM görünümü 10", DisplayOrder = 10 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-11.jpg", AltText = "La Via AVM görünümü 11", DisplayOrder = 11 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-12.jpg", AltText = "La Via AVM görünümü 12", DisplayOrder = 12 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-13.jpg", AltText = "La Via AVM görünümü 13", DisplayOrder = 13 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-14.jpg", AltText = "La Via AVM görünümü 14", DisplayOrder = 14 },
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-15.jpg", AltText = "La Via AVM görünümü 15", DisplayOrder = 15 }
        };
    }

    // Site plan image(s) for La Via AVM's Hero "Vaziyet Planı" button (La Via
    // AVM update, 2026-08-09) — same shape as
    // BuildLaFioreKarabag2EtapSitePlanImages, one entry today (the client's
    // single supplied master-plan render), opened in the shared Media Viewer.
    private static List<ProjectSitePlanImage> BuildKuyuluAvmSitePlanImages()
    {
        return new List<ProjectSitePlanImage>
        {
            new() { ImagePath = "/images/projects/kuyulu-avm/gallery/vaziyet-plani/1.VAZİYET.webp", AltText = "La Via AVM vaziyet planı", DisplayOrder = 1 }
        };
    }

    // Floor Plans ("PLANLAR") for La Via AVM (La Via AVM update, 2026-08-09)
    // — 3 real building-floor drawings (Bodrum/Zemin/1. Kat), not per-
    // apartment-type unit plans like every other project's FloorPlan rows:
    // this is a commercial AVM leased out by dozens of individually sized
    // units per floor, so ApartmentType is reused as a free-text floor-level
    // label, same precedent as BuildLaFioreKarabag2EtapFloorPlans reusing it
    // as a block/floor label. NetAreaM2/GrossAreaM2/SalesGrossAreaM2 briefly
    // sat at 0 (the decimal default) rather than a placeholder, since the
    // client hadn't confirmed real per-floor figures yet and the drawings
    // show dozens of per-unit areas rather than one Net/Gross/Sales-Gross
    // figure per floor. Reverted (client revision, 2026-08-09): the stats
    // row disappearing entirely read as broken, so these now carry the same
    // 68/95/78 placeholder every other project's FloorPlan rows use (see the
    // many other Build...FloorPlans methods below), to be replaced with the
    // client's real figures once confirmed. Rooms stays empty — no per-room
    // breakdown applies to a commercial floor.
    private static List<FloorPlan> BuildKuyuluAvmFloorPlans()
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "Bodrum Kat",
                ImagePath = "/images/projects/kuyulu-avm/gallery/kat-planlari/1-bodrum.webp",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>()
            },
            new()
            {
                ApartmentType = "Zemin Kat",
                ImagePath = "/images/projects/kuyulu-avm/gallery/kat-planlari/2-ZEMİN.webp",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 2,
                Rooms = new List<FloorPlanRoom>()
            },
            new()
            {
                ApartmentType = "1. Kat",
                ImagePath = "/images/projects/kuyulu-avm/gallery/kat-planlari/3-1.KAT.webp",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 3,
                Rooms = new List<FloorPlanRoom>()
            }
        };
    }

    // Concept section's 3-slide image carousel for La Via AVM (La Via AVM
    // update, 2026-08-09) — same image-only-sibling pattern as
    // BuildLaFioreKarabag2EtapConceptImages (this project has no concept
    // video). 3 shots hand-picked from the gallery photos above (aerial
    // establishing view, ground-level retail promenade, corner storefront
    // massing) rather than truly randomized at request time, since seed data
    // is static — matches how La Fiore's own "random" selection was an
    // authorial choice, not a runtime Random call. Copy is written fresh for
    // La Via AVM (not reused from Davutlar/La Fiore) and deliberately avoids
    // naming the real fashion brands visible on the storefront renders
    // (Prada/Boss/Calvin Klein/etc.) — those are the artist's staging, not
    // confirmed tenants, so implying them as signed brands would be an
    // invented business claim.
    private static List<ProjectConceptImage> BuildKuyuluAvmConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-01.jpg",
                Eyebrow = "Kuyulu'nun Yeni Merkezi",
                Title = "Zeytinliklerin Eşiğinde Bir Yaşam Merkezi",
                Description = "Aydın Efeler'de, geniş bir zeytinlik dokusunun kenarında yükselen La Via AVM, alışverişi gündelik bir ihtiyaçtan çok bir yaşam deneyimine dönüştürüyor. Gölgelikli teraslar, geniş yürüyüş alanları ve özenle tasarlanmış peyzajıyla proje, Ançın İnşaat'ın imza kalite anlayışını ticari ölçekte yeniden yorumluyor.",
                DisplayOrder = 1
            },
            new()
            {
                ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-05.jpg",
                Eyebrow = "Seçkin Bir Alışveriş Rotası",
                Title = "Yürüyüş Kadar Keyifli Bir Cephe",
                Description = "Zemin kattaki mağaza cepheleri, gölgelikli yürüyüş yolu boyunca kesintisiz bir vitrin deneyimi sunacak şekilde kurgulandı. Palmiye ağaçlarıyla çevrili geniş kaldırımlar, ziyaretçileri mağazadan mağazaya rahatça dolaşmaya davet ediyor.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/kuyulu-avm/gallery/originals/kuyulu-avm-09.jpg",
                Eyebrow = "Zarif Bir Mimari İmza",
                Title = "Taş, Ahşap ve Işığın Buluştuğu Cephe",
                Description = "Doğal taş kaplamalar, ahşap lamel dokular ve gece boyunca yumuşak bir çizgiyle beliren aydınlatma, La Via AVM'nin cephesine kalıcı ve zarif bir karakter kazandırıyor. Her köşe, projenin bütününde hissedilen premium mimari anlayışını yansıtıyor.",
                DisplayOrder = 3
            }
        };
    }

    // Real gallery photos for Hacıfeyzullah - Q-Latis (2026-08-10) — every
    // file the client supplied under wwwroot/images/projects/q-latis,
    // reorganized into gallery/{exterior,interior,social}/originals per
    // 07_AssetStructure.md's convention (raw dis-mekan-gorselleri/
    // ic-mekan-gorselleri/sosyal-olanaklar/konsept/katalog drops left in
    // place as an archival copy, same precedent as Davutlar D Latis):
    // dis-mekan-gorselleri → "Exterior" (11 renders, minus 2.png which is
    // reserved as the banner/cover source — see the Project block below,
    // same exclusion precedent as Davutlar D Latis's 3A.jpeg / La Fiore
    // Karabağ 2. Etap's 2a.jpeg), ic-mekan-gorselleri → "Interior" (1
    // render — no per-unit-type subfolders were supplied, so no Block/
    // ApartmentType chips), sosyal-olanaklar → "Social Areas" (3 renders).
    private static List<ProjectImage> BuildQLatisImages()
    {
        return new List<ProjectImage>
        {
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/1.png", AltText = "Hacıfeyzullah - Q-Latis sokak seviyesinden dış cephe görünümü", DisplayOrder = 1, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/3.png", AltText = "Hacıfeyzullah - Q-Latis kuşbakışı yerleşim görünümü", DisplayOrder = 2, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/4.png", AltText = "Hacıfeyzullah - Q-Latis çatı terası ve yüzme havuzu görünümü", DisplayOrder = 3, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/5.png", AltText = "Hacıfeyzullah - Q-Latis Kuşadası tepelerinden dış cephe görünümü", DisplayOrder = 4, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/6.png", AltText = "Hacıfeyzullah - Q-Latis ahşap cephe detayı", DisplayOrder = 5, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/7.png", AltText = "Hacıfeyzullah - Q-Latis gün batımında balkon ve deniz manzarası", DisplayOrder = 6, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/8.png", AltText = "Hacıfeyzullah - Q-Latis akşam ışığında dış cephe ve deniz manzarası", DisplayOrder = 7, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/9.png", AltText = "Hacıfeyzullah - Q-Latis gün batımında balkon detayı", DisplayOrder = 8, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/10.png", AltText = "Hacıfeyzullah - Q-Latis gece dış cephe görünümü", DisplayOrder = 9, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/exterior/originals/11.png", AltText = "Hacıfeyzullah - Q-Latis balkon ve cephe detayı", DisplayOrder = 10, Category = "Exterior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/interior/originals/1.png", AltText = "Hacıfeyzullah - Q-Latis salon iç mekan görünümü", DisplayOrder = 11, Category = "Interior" },
            new() { ImagePath = "/images/projects/q-latis/gallery/social/originals/1.png", AltText = "Hacıfeyzullah - Q-Latis açık teras ve fitness alanı", DisplayOrder = 12, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/q-latis/gallery/social/originals/2.png", AltText = "Hacıfeyzullah - Q-Latis aeroyoga stüdyosu", DisplayOrder = 13, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/q-latis/gallery/social/originals/3.png", AltText = "Hacıfeyzullah - Q-Latis fitness ekipmanları", DisplayOrder = 14, Category = "Social Areas" }
        };
    }

    // Real concept video (2026-08-10) — client-supplied konsept/
    // video1-web.mp4 (208MB, 1920x1080, 6.2Mbps, 252.97s) re-encoded via
    // ffmpeg (libx264 CRF 23, 2.2Mbps capped, AAC 128k, +faststart) down to
    // ~54MB at the same 1920x1080/253s, copied to concept/video.mp4. Poster
    // frame extracted at 01:45 (clean, watermark-free street-level render,
    // no massing-diagram/title-card frames used) via ffmpeg + ThumbnailTool
    // --single (1920w/88q) to concept/poster.webp.
    private static List<ProjectConceptVideo> BuildQLatisConceptVideos()
    {
        return new List<ProjectConceptVideo>
        {
            new()
            {
                VideoPath = "/images/projects/q-latis/concept/video.mp4",
                PosterPath = "/images/projects/q-latis/concept/poster.webp",
                Eyebrow = "Kuşadası'nda Yeni Bir Yaşam",
                Title = "Hacıfeyzullah - Q-Latis'e Hoş Geldiniz",
                Description = "Kuşadası'nın eşsiz doğasıyla iç içe, Ançın İnşaat güvencesiyle hayata geçen Hacıfeyzullah - Q-Latis, modern mimarisi ve deniz manzaralı yaşam alanlarıyla ayrıcalıklı bir yaşamı sizlere sunuyor.",
                DisplayOrder = 1
            }
        };
    }

    // 3 selected exterior renders — chosen for visual quality/relevance
    // (clean full elevation, dusk sea-view elevation, Kuşadası hillside
    // establishing shot with the site's own "KUŞADASI" signage), not the
    // same photo excluded from the Gallery grid as the banner/cover source.
    // Copy is grounded in what each image actually shows (materials, sea
    // view, hillside/olive-grove setting) and the video's own "Kuşadası"
    // title card — no invented facilities, distances or claims.
    private static List<ProjectConceptImage> BuildQLatisConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/q-latis/gallery/exterior/originals/2.png",
                Eyebrow = "Mimari Kimlik",
                Title = "Doğayla Uyumlu Modern Mimari",
                Description = "Ahşap dokulu cepheleri, geniş camekanları ve yeşille bütünleşen balkonlarıyla Hacıfeyzullah - Q-Latis, Kuşadası'nın karakterine saygılı, çağdaş bir mimari dil sunuyor.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/q-latis/gallery/exterior/originals/8.png",
                Eyebrow = "Ege Manzarası",
                Title = "Balkonunuzdan Ege'nin Eşsiz Manzarası",
                Description = "Üst kat dairelerin geniş balkonlarından izlenen Ege Denizi manzarası, Hacıfeyzullah - Q-Latis'te her günü özel kılıyor.",
                DisplayOrder = 3
            },
            new()
            {
                ImagePath = "/images/projects/q-latis/gallery/exterior/originals/5.png",
                Eyebrow = "Konum",
                Title = "Kuşadası'nın Kalbinde",
                Description = "Zeytinliklerle çevrili tepelik bir konumda yükselen Hacıfeyzullah - Q-Latis, Kuşadası'nın doğal dokusunu koruyan bir yerleşim anlayışıyla tasarlandı.",
                DisplayOrder = 4
            }
        };
    }

    // Real gallery photos for La Fiore Karabağ 2. Etap (Gallery pilot,
    // 2026-08-06) — every file supplied under wwwroot/images/projects/
    // la-fiore-karabag-2-etap/gallery (reorganized from the client's raw
    // karabag-la-fiore-ikinci-etap/ folder into this project's actual slug,
    // per docs/07_AssetStructure.md's originals/thumbnails convention):
    // dis-mekan-gorselleri → "Exterior" across 8 blocks (A–H), ic-mekan-
    // gorselleri → "Interior" across the 3 blocks that have interior photos
    // (A, B, C — D through H have none). Block/ApartmentType are this
    // project's pilot of the new Gallery hierarchy tier (see ProjectImage.cs)
    // — C Tipi Blok is the only block with two distinct ApartmentType values
    // ("Sağ Tip"/"Sol Tip"), so it's the only one whose apartment-type chip
    // row ever appears; A ("4+1") and B ("Sol Tip") each have exactly one
    // value, so their chip row stays hidden per _ProjectGallery.cshtml's
    // "only show when needed" rule. dis-mekan-gorselleri/a-tipi-blok/2a.jpeg
    // is deliberately excluded here — it's the source photo behind both
    // banner.webp (Hero) and cover.webp (Project Card), same reasoning as
    // Davutlar D Latis excluding its own Hero source photo from the Gallery
    // grid (see BuildDavutlarDLatisImages above).
    private static List<ProjectImage> BuildLaFioreKarabag2EtapImages()
    {
        return new List<ProjectImage>
        {
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 1", DisplayOrder = 1, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 2", DisplayOrder = 2, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/2b.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 3", DisplayOrder = 3, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/3.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 4", DisplayOrder = 4, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/4.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 5", DisplayOrder = 5, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/4a.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 6", DisplayOrder = 6, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/4b.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 7", DisplayOrder = 7, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/5.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 8", DisplayOrder = 8, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/6.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 9", DisplayOrder = 9, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/6a.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 10", DisplayOrder = 10, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/7.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 11", DisplayOrder = 11, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/a-tipi-blok/originals/8.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok dış cephe görünümü 12", DisplayOrder = 12, Category = "Exterior", Block = "A Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/b-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok dış cephe görünümü 1", DisplayOrder = 13, Category = "Exterior", Block = "B Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/b-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok dış cephe görünümü 2", DisplayOrder = 14, Category = "Exterior", Block = "B Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/b-tipi-blok/originals/2a.jpeg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok dış cephe görünümü 3", DisplayOrder = 15, Category = "Exterior", Block = "B Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/b-tipi-blok/originals/3.jpeg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok dış cephe görünümü 4", DisplayOrder = 16, Category = "Exterior", Block = "B Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/b-tipi-blok/originals/3a.jpeg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok dış cephe görünümü 5", DisplayOrder = 17, Category = "Exterior", Block = "B Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 1", DisplayOrder = 18, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 2", DisplayOrder = 19, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/2a.jpeg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 3", DisplayOrder = 20, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/3.jpeg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 4", DisplayOrder = 21, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/3a.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 5", DisplayOrder = 22, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/3b.jpeg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 6", DisplayOrder = 23, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/3c.jpeg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 7", DisplayOrder = 24, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/3d.jpeg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 8", DisplayOrder = 25, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/4.jpeg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 9", DisplayOrder = 26, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/4a.jpeg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok dış cephe görünümü 10", DisplayOrder = 27, Category = "Exterior", Block = "C Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/d-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap D Tipi Blok dış cephe görünümü 1", DisplayOrder = 28, Category = "Exterior", Block = "D Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/d-tipi-blok/originals/1a.jpeg", AltText = "La Fiore Karabağ 2. Etap D Tipi Blok dış cephe görünümü 2", DisplayOrder = 29, Category = "Exterior", Block = "D Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/d-tipi-blok/originals/1b.jpeg", AltText = "La Fiore Karabağ 2. Etap D Tipi Blok dış cephe görünümü 3", DisplayOrder = 30, Category = "Exterior", Block = "D Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/d-tipi-blok/originals/1c.jpeg", AltText = "La Fiore Karabağ 2. Etap D Tipi Blok dış cephe görünümü 4", DisplayOrder = 31, Category = "Exterior", Block = "D Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/d-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap D Tipi Blok dış cephe görünümü 5", DisplayOrder = 32, Category = "Exterior", Block = "D Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/d-tipi-blok/originals/2a.jpeg", AltText = "La Fiore Karabağ 2. Etap D Tipi Blok dış cephe görünümü 6", DisplayOrder = 33, Category = "Exterior", Block = "D Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/d-tipi-blok/originals/2b.jpeg", AltText = "La Fiore Karabağ 2. Etap D Tipi Blok dış cephe görünümü 7", DisplayOrder = 34, Category = "Exterior", Block = "D Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 1", DisplayOrder = 35, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/1a.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 2", DisplayOrder = 36, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/1b.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 3", DisplayOrder = 37, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 4", DisplayOrder = 38, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/2a.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 5", DisplayOrder = 39, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/2b.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 6", DisplayOrder = 40, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/3.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 7", DisplayOrder = 41, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/3a.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 8", DisplayOrder = 42, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/3b.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 9", DisplayOrder = 43, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/3c.jpeg", AltText = "La Fiore Karabağ 2. Etap E Tipi Blok dış cephe görünümü 10", DisplayOrder = 44, Category = "Exterior", Block = "E Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 1", DisplayOrder = 45, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/1a.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 2", DisplayOrder = 46, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 3", DisplayOrder = 47, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/2a.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 4", DisplayOrder = 48, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/3.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 5", DisplayOrder = 49, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/3a.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 6", DisplayOrder = 50, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/3b.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 7", DisplayOrder = 51, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/3c.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 8", DisplayOrder = 52, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/3d.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 9", DisplayOrder = 53, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/3e.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 10", DisplayOrder = 54, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/4.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 11", DisplayOrder = 55, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/5.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 12", DisplayOrder = 56, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/6.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 13", DisplayOrder = 57, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/6a.jpeg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 14", DisplayOrder = 58, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/b_21 - Foto.jpg", AltText = "La Fiore Karabağ 2. Etap F Tipi Blok dış cephe görünümü 15", DisplayOrder = 59, Category = "Exterior", Block = "F Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 1", DisplayOrder = 60, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/1a.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 2", DisplayOrder = 61, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/1b.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 3", DisplayOrder = 62, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/1c.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 4", DisplayOrder = 63, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 5", DisplayOrder = 64, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/2a.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 6", DisplayOrder = 65, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/2b.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 7", DisplayOrder = 66, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/2c.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 8", DisplayOrder = 67, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/3.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 9", DisplayOrder = 68, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/3a.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 10", DisplayOrder = 69, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/3b.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 11", DisplayOrder = 70, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/4.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 12", DisplayOrder = 71, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/5.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 13", DisplayOrder = 72, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/6.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 14", DisplayOrder = 73, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/7.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 15", DisplayOrder = 74, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/8.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 16", DisplayOrder = 75, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/9.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 17", DisplayOrder = 76, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/9a.jpeg", AltText = "La Fiore Karabağ 2. Etap G Tipi Blok dış cephe görünümü 18", DisplayOrder = 77, Category = "Exterior", Block = "G Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/h-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap H Tipi Blok dış cephe görünümü 1", DisplayOrder = 78, Category = "Exterior", Block = "H Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/h-tipi-blok/originals/1a.jpeg", AltText = "La Fiore Karabağ 2. Etap H Tipi Blok dış cephe görünümü 2", DisplayOrder = 79, Category = "Exterior", Block = "H Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/h-tipi-blok/originals/1b.jpeg", AltText = "La Fiore Karabağ 2. Etap H Tipi Blok dış cephe görünümü 3", DisplayOrder = 80, Category = "Exterior", Block = "H Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/h-tipi-blok/originals/1c.jpeg", AltText = "La Fiore Karabağ 2. Etap H Tipi Blok dış cephe görünümü 4", DisplayOrder = 81, Category = "Exterior", Block = "H Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/h-tipi-blok/originals/1d.jpeg", AltText = "La Fiore Karabağ 2. Etap H Tipi Blok dış cephe görünümü 5", DisplayOrder = 82, Category = "Exterior", Block = "H Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/h-tipi-blok/originals/1e.jpeg", AltText = "La Fiore Karabağ 2. Etap H Tipi Blok dış cephe görünümü 6", DisplayOrder = 83, Category = "Exterior", Block = "H Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/h-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap H Tipi Blok dış cephe görünümü 7", DisplayOrder = 84, Category = "Exterior", Block = "H Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/h-tipi-blok/originals/2a.jpeg", AltText = "La Fiore Karabağ 2. Etap H Tipi Blok dış cephe görünümü 8", DisplayOrder = 85, Category = "Exterior", Block = "H Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/h-tipi-blok/originals/2b.jpeg", AltText = "La Fiore Karabağ 2. Etap H Tipi Blok dış cephe görünümü 9", DisplayOrder = 86, Category = "Exterior", Block = "H Tipi Blok" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/1.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 1", DisplayOrder = 87, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/2.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 2", DisplayOrder = 88, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/3.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 3", DisplayOrder = 89, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/4.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 4", DisplayOrder = 90, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/5.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 5", DisplayOrder = 91, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/6.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 6", DisplayOrder = 92, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/7.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 7", DisplayOrder = 93, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/8.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 8", DisplayOrder = 94, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/9.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 9", DisplayOrder = 95, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/10.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 10", DisplayOrder = 96, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/11.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 11", DisplayOrder = 97, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/12.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 12", DisplayOrder = 98, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/13.jpeg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 13", DisplayOrder = 99, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/14.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 14", DisplayOrder = 100, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/15.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 15", DisplayOrder = 101, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/a-tipi-blok/originals/16.jpg", AltText = "La Fiore Karabağ 2. Etap A Tipi Blok iç mekan görünümü 16", DisplayOrder = 102, Category = "Interior", Block = "A Tipi Blok", ApartmentType = "4+1" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/1.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 1", DisplayOrder = 103, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/2.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 2", DisplayOrder = 104, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/3.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 3", DisplayOrder = 105, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/4.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 4", DisplayOrder = 106, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/5.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 5", DisplayOrder = 107, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/6.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 6", DisplayOrder = 108, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/7.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 7", DisplayOrder = 109, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/8.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 8", DisplayOrder = 110, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/9.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 9", DisplayOrder = 111, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/b-tipi-blok/originals/ebeveyn banyo.jpg", AltText = "La Fiore Karabağ 2. Etap B Tipi Blok iç mekan görünümü 10", DisplayOrder = 112, Category = "Interior", Block = "B Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/1.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 1", DisplayOrder = 113, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/2.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 2", DisplayOrder = 114, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/3.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 3", DisplayOrder = 115, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/4.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 4", DisplayOrder = 116, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/5.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 5", DisplayOrder = 117, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/6.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 6", DisplayOrder = 118, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/7.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 7", DisplayOrder = 119, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/8.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 8", DisplayOrder = 120, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/9.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 9", DisplayOrder = 121, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/10.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 10", DisplayOrder = 122, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/11.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 11", DisplayOrder = 123, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/12.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 12", DisplayOrder = 124, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/13.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 13", DisplayOrder = 125, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/14.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 14", DisplayOrder = 126, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sag-tip/originals/15.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sağ Tip iç mekan görünümü 15", DisplayOrder = 127, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sağ Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/1.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 1", DisplayOrder = 128, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/2.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 2", DisplayOrder = 129, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/3.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 3", DisplayOrder = 130, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/4.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 4", DisplayOrder = 131, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/5.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 5", DisplayOrder = 132, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/6.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 6", DisplayOrder = 133, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/7.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 7", DisplayOrder = 134, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/8.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 8", DisplayOrder = 135, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/9.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 9", DisplayOrder = 136, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/10.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 10", DisplayOrder = 137, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/11.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 11", DisplayOrder = 138, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/12.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 12", DisplayOrder = 139, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/13.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 13", DisplayOrder = 140, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/14.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 14", DisplayOrder = 141, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/interior/c-tipi-blok/sol-tip/originals/15.jpg", AltText = "La Fiore Karabağ 2. Etap C Tipi Blok Sol Tip iç mekan görünümü 15", DisplayOrder = 142, Category = "Interior", Block = "C Tipi Blok", ApartmentType = "Sol Tip" },

                    // "Tüm Dış Mekan Görselleri" gallery category (Vaziyet Planı/
                    // Concept/Gallery phase, 2026-08-09) — a broader, general
                    // exterior/render pool distinct from the per-block Exterior
                    // photos above, reorganized from the client's raw
                    // tum-dis-mekan-gorselleri/ drop into
                    // gallery/all-exterior/originals/, renamed sequentially.
                    // No Block/ApartmentType — this category is not tied to a
                    // specific building.
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/1.jpg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 1", DisplayOrder = 143, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 2", DisplayOrder = 144, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/3.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 3", DisplayOrder = 145, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/4.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 4", DisplayOrder = 146, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/5.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 5", DisplayOrder = 147, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/6.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 6", DisplayOrder = 148, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/7.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 7", DisplayOrder = 149, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/8.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 8", DisplayOrder = 150, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/9.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 9", DisplayOrder = 151, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/10.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 10", DisplayOrder = 152, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/11.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 11", DisplayOrder = 153, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/12.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 12", DisplayOrder = 154, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/13.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 13", DisplayOrder = 155, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/14.jpg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 14", DisplayOrder = 156, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/15.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 15", DisplayOrder = 157, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/16.jpg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 16", DisplayOrder = 158, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/17.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 17", DisplayOrder = 159, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/18.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 18", DisplayOrder = 160, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/19.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 19", DisplayOrder = 161, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/20.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 20", DisplayOrder = 162, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/21.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 21", DisplayOrder = 163, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/22.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 22", DisplayOrder = 164, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/23.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 23", DisplayOrder = 165, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/24.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 24", DisplayOrder = 166, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/25.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 25", DisplayOrder = 167, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/26.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 26", DisplayOrder = 168, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/27.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 27", DisplayOrder = 169, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/28.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 28", DisplayOrder = 170, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/29.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 29", DisplayOrder = 171, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/30.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 30", DisplayOrder = 172, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/31.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 31", DisplayOrder = 173, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/32.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 32", DisplayOrder = 174, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/33.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 33", DisplayOrder = 175, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/34.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 34", DisplayOrder = 176, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/35.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 35", DisplayOrder = 177, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/36.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 36", DisplayOrder = 178, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/37.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 37", DisplayOrder = 179, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/38.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 38", DisplayOrder = 180, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/39.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 39", DisplayOrder = 181, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/40.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 40", DisplayOrder = 182, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/41.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 41", DisplayOrder = 183, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/42.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 42", DisplayOrder = 184, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/43.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 43", DisplayOrder = 185, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/44.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 44", DisplayOrder = 186, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/45.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 45", DisplayOrder = 187, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/46.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 46", DisplayOrder = 188, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/47.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 47", DisplayOrder = 189, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/48.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 48", DisplayOrder = 190, Category = "All Exterior" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/49.jpeg", AltText = "La Fiore Karabağ 2. Etap dış mekan görünümü 49", DisplayOrder = 191, Category = "All Exterior" },

                    // "Sosyal Alan" (Social Areas), client curation, 2026-08-20 —
                    // copies/references 16 of the per-block Exterior photos above
                    // (C/D/E/F/G Tipi Blok) plus 3 of the "All Exterior" pool above
                    // (same ImagePath, a second ProjectImage row with a different
                    // Category, no file duplication) so they also surface in the
                    // Gallery's Social Areas filter and feed the Social Facilities
                    // cards. No Block on these rows — Social Areas is not tied to a
                    // specific building, same as All Exterior above.
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/3.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 1", DisplayOrder = 192, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/3a.jpg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 2", DisplayOrder = 193, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/c-tipi-blok/originals/3b.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 3", DisplayOrder = 194, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/d-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 4", DisplayOrder = 195, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 5", DisplayOrder = 196, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/1a.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 6", DisplayOrder = 197, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/3.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 7", DisplayOrder = 198, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/3a.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 8", DisplayOrder = 199, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/e-tipi-blok/originals/3b.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 9", DisplayOrder = 200, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 10", DisplayOrder = 201, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/1a.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 11", DisplayOrder = 202, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/f-tipi-blok/originals/2.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 12", DisplayOrder = 203, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/1.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 13", DisplayOrder = 204, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/1a.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 14", DisplayOrder = 205, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/9.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 15", DisplayOrder = 206, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/exterior/g-tipi-blok/originals/9a.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 16", DisplayOrder = 207, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/35.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 17", DisplayOrder = 208, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/36.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 18", DisplayOrder = 209, Category = "Social Areas" },
                    new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/37.jpeg", AltText = "La Fiore Karabağ 2. Etap sosyal alan görünümü 19", DisplayOrder = 210, Category = "Social Areas" }
        };
    }

    // Hero "Vaziyet Planı" site plan images (Vaziyet Planı/Concept/Gallery
    // phase, 2026-08-09) — 3 real master plan drawings supplied under
    // ProjectAssets/Projects/la-fiore-karabag-2-etap/vaziyet-planlari/,
    // converted to WebP via ThumbnailTool --single (1600w/88q, same recipe
    // as Davutlar D Latis's single site-plan.webp above). Multiple entries
    // give the Hero's Media Viewer group Prev/Next across all 3 — see
    // ProjectSitePlanImage.
    private static List<ProjectSitePlanImage> BuildLaFioreKarabag2EtapSitePlanImages()
    {
        return new List<ProjectSitePlanImage>
        {
            new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/site-plan-1.webp", AltText = "La Fiore Karabağ 2. Etap vaziyet planı 1", DisplayOrder = 1 },
            new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/site-plan-2.webp", AltText = "La Fiore Karabağ 2. Etap vaziyet planı 2", DisplayOrder = 2 },
            new() { ImagePath = "/images/projects/la-fiore-karabag-2-etap/site-plan-3.webp", AltText = "La Fiore Karabağ 2. Etap vaziyet planı 3", DisplayOrder = 3 }
        };
    }

    // Concept section's 3-slide image carousel (Vaziyet Planı/Concept/
    // Gallery phase, 2026-08-09) — this project has no concept video, so it
    // gets the image-only sibling of BuildDavutlarDLatisConceptVideos above,
    // reusing the exact same carousel markup/CSS/JS (see
    // _ProjectConcept.cshtml). Images are 3 selected shots from the new
    // "Tüm Dış Mekan Görselleri" pool (BuildLaFioreKarabag2EtapImages'
    // "All Exterior" entries above) — general establishing renders rather
    // than per-block photos, a better fit for a project-wide showcase. Copy
    // is written specifically for this project (architecture/identity,
    // social living, investment value), not reused from any other project.
    private static List<ProjectConceptImage> BuildLaFioreKarabag2EtapConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/1.jpg",
                Eyebrow = "Karabağ'ın Kalbinde",
                Title = "Zeytinliklerin İçinde Yükselen Mimari",
                Description = "Aydın Karabağ'ın yüzlerce yıllık zeytin ağaçları arasında konumlanan La Fiore Karabağ 2. Etap, arazinin doğal eğimini ve dokusunu koruyan bir yerleşim planıyla hayat buluyor. Her blok, çevresindeki yeşille bütünleşecek şekilde konumlandırılarak Ançın İnşaat imzasının kalite anlayışını yansıtıyor.",
                DisplayOrder = 1
            },
            new()
            {
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/15.jpeg",
                Eyebrow = "Ortak Yaşam Alanları",
                Title = "Sosyal Yaşamın Kalbinde Bir Rota",
                Description = "Geniş yürüyüş yolları, peyzajlı meydanlar ve ortak sosyal alanlarla örülen La Fiore Karabağ 2. Etap, sakinlerini binalardan çok bir arada yaşama davet ediyor. Zeytinliklerin arasından geçen yollar, günün her saatinde huzurlu bir yürüyüş deneyimi sunuyor.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/30.jpeg",
                Eyebrow = "Bugünden Yarına Değer",
                Title = "Karabağ'da Ayrıcalıklı Bir Yatırım",
                Description = "Taş duvarları, zarif aydınlatması ve zeytin ağaçlarıyla çevrili villaları ile La Fiore Karabağ 2. Etap, hem huzurlu bir yaşamı hem de güçlü bir yatırım fırsatını bir araya getiriyor. Karabağ'ın gelişen değeriyle birlikte, bu proje geleceğe kalıcı bir yatırım vaadi taşıyor.",
                DisplayOrder = 3
            }
        };
    }

    // Real floor plan drawings for La Fiore Karabağ 2. Etap (Floor Plans
    // pilot, 2026-08-06) — every file supplied under wwwroot/images/projects/
    // la-fiore-karabag-2-etap/floorplans (reorganized from kat-planlari/,
    // renamed to plain slugs). These are block/floor architectural drawings,
    // not per-apartment-type unit plans like every other project's single
    // placeholder entry — ApartmentType is reused as a free-text panel label
    // here ("A Tipi Blok – Zemin Kat", "F Tipi Blok", "G Tipi Blok (1+1)",
    // one entry per real drawing, 14 total) since the field has never been
    // validated against a real taxonomy. Net/Gross/Sales-Gross m² and the
    // room list are NOT real client data — no per-block/floor specs were
    // supplied — so they intentionally reuse the same placeholder figures
    // every other project's "2+1" entry already shows (see
    // BuildPlaceholderFloorPlans), clearly a placeholder today exactly as it
    // was before this pilot, now just paired with the real drawing instead
    // of the static graphic. Replace with real specs once the client
    // supplies them.
    private static List<FloorPlan> BuildLaFioreKarabag2EtapFloorPlans()
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "A Tipi Blok – Zemin Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/a-tipi-blok-zemin-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "A Tipi Blok – 1. Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/a-tipi-blok-1-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 2,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "B Tipi Blok – Zemin Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/b-tipi-blok-zemin-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 3,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "B Tipi Blok – 1. Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/b-tipi-blok-1-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 4,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "C Tipi Blok – Zemin Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/c-tipi-blok-zemin-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 5,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "C Tipi Blok – 1. Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/c-tipi-blok-1-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 6,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "D Tipi Blok – Zemin Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/d-tipi-blok-zemin-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 7,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "D Tipi Blok – 1. Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/d-tipi-blok-1-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 8,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "E Tipi Blok – Zemin Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/e-tipi-blok-zemin-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 9,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "E Tipi Blok – 1. Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/e-tipi-blok-1-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 10,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "F Tipi Blok",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/f-tipi-blok.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 11,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "G Tipi Blok (1+1)",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/g-tipi-blok-1-1.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 12,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "H Tipi Blok – Zemin Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/h-tipi-blok-zemin-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 13,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "H Tipi Blok – 1. Kat",
                ImagePath = "/images/projects/la-fiore-karabag-2-etap/floorplans/originals/h-tipi-blok-1-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 14,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            }
        };
    }

    // La Via Villalar 1. Etap Gallery (client asset delivery, 2026-08-06) —
    // same premium originals/thumbnails architecture as
    // BuildLaFioreKarabag2EtapImages, adapted for THIS project only: three
    // flat Category values (Exterior, Standard Interior, Optional Interior),
    // no Block/ApartmentType tiers, since the client's folders never split
    // by building block. Source folders were wwwroot/images/projects/
    // kuyulu-la-via-villalar-birinci-etap/dis-mekan-gorselleri/,
    // ic-mekan-gorselleri/standart-ic-mekan-gorselleri/ and
    // .../opsiyonelli-ic-mekan-gorselleri/, reorganized into gallery/
    // {exterior,standard-interior,optional-interior}/originals/ (clean
    // ASCII filenames) with the raw folders left in place, untouched, as an
    // archival copy. AltText derives from each file's own room label where
    // the client's filename carried one (e.g. "13-SALON.jpeg"); a running
    // index is appended whenever a room repeats.
    private static List<ProjectImage> BuildKuyuluLaViaVillalarImages()
    {
        return new List<ProjectImage>
        {
            // Exterior — dis-mekan-gorselleri/ (client-supplied, 2026-08-06). 14.jpg is also
            // used as the dedicated Hero Banner/CoverImage source (see cover.webp/banner.webp
            // below) and is kept here too, matching Davutlar D Latis's precedent of a shared
            // source photo remaining a normal Gallery card as well.
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/01.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 1", DisplayOrder = 1, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/02.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 2", DisplayOrder = 2, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/03.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 3", DisplayOrder = 3, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/04.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 4", DisplayOrder = 4, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/07.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 7", DisplayOrder = 5, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/09.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 9", DisplayOrder = 6, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/10.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 10", DisplayOrder = 7, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/11.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 11", DisplayOrder = 8, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/12.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 12", DisplayOrder = 9, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/13.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 13", DisplayOrder = 10, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/14.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 14", DisplayOrder = 11, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/15.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 15", DisplayOrder = 12, Category = "Exterior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/16.jpg", AltText = "La Via Villalar 1. Etap dış cephe görünümü 16", DisplayOrder = 13, Category = "Exterior" },

            // Standard Interior — ic-mekan-gorselleri/standart-ic-mekan-gorselleri/
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/01-giris.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Giriş", DisplayOrder = 14, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/02-hol-2.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Hol 1", DisplayOrder = 15, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/03-hol.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Hol 2", DisplayOrder = 16, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/04-vestiyer-odasi-2.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Vestiyer Odası 1", DisplayOrder = 17, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/05-vestiyer-odasi-1.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Vestiyer Odası 2", DisplayOrder = 18, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/06-kiler-2.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Kiler 1", DisplayOrder = 19, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/07-kiler.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Kiler 2", DisplayOrder = 20, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/08-mutfak-2.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Mutfak 1", DisplayOrder = 21, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/09-mutfak.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Mutfak 2", DisplayOrder = 22, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/10-salon-2.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Salon 1", DisplayOrder = 23, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/11-salon-3.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Salon 2", DisplayOrder = 24, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/12-salon-4.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Salon 3", DisplayOrder = 25, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/13-salon.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Salon 4", DisplayOrder = 26, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/14-ebeveyn-yatak-odasi-2.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Ebeveyn Yatak Odası 1", DisplayOrder = 27, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/15-ebeveyn-yatak-odasi-3.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Ebeveyn Yatak Odası 2", DisplayOrder = 28, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/16-ebeveyn-yatak-odasi.jpeg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Ebeveyn Yatak Odası 3", DisplayOrder = 29, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/30-giyinme-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Giyinme Odası 1", DisplayOrder = 30, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/31-giyinme-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Giyinme Odası 2", DisplayOrder = 31, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/32-giyinme-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Giyinme Odası 3", DisplayOrder = 32, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/33-ebeveyn-banyosu.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Ebeveyn Banyosu", DisplayOrder = 33, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/34-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Ebeveyn Odası 1", DisplayOrder = 34, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/35-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Ebeveyn Odası 2", DisplayOrder = 35, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/36-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Ebeveyn Odası 3", DisplayOrder = 36, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/37-yatak-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Yatak Odası 1", DisplayOrder = 37, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/38-yatak-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Yatak Odası 2", DisplayOrder = 38, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/39-yatak-odasi-banyo.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Yatak Odası Banyo", DisplayOrder = 39, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/40-yatak-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Yatak Odası 3", DisplayOrder = 40, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/41-yatak-odasi.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Yatak Odası 4", DisplayOrder = 41, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/42-yatak-odasi-1.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Yatak Odası 5", DisplayOrder = 42, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/42-yatak-odasi-2.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Yatak Odası 6", DisplayOrder = 43, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/44-oda-1.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Oda 1", DisplayOrder = 44, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/44-wc-1.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – WC 1", DisplayOrder = 45, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/44-oda-2.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Oda 2", DisplayOrder = 46, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/44-wc-2.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – WC 2", DisplayOrder = 47, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/44-oda-3.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan – Oda 3", DisplayOrder = 48, Category = "Standard Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/standard-interior/originals/45-ic-mekan.jpg", AltText = "La Via Villalar 1. Etap Standart İç Mekan görünümü 36", DisplayOrder = 49, Category = "Standard Interior" },

            // Optional Interior — ic-mekan-gorselleri/opsiyonelli-ic-mekan-gorselleri/
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/01-giris.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Giriş", DisplayOrder = 50, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/02-hol.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Hol 1", DisplayOrder = 51, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/03-hol.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Hol 2", DisplayOrder = 52, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/04-hol.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Hol 3", DisplayOrder = 53, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/05-hol.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Hol 4", DisplayOrder = 54, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/06-vestiyer-odasi-1.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Vestiyer Odası 1", DisplayOrder = 55, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/07-vestiyer-odasi-2.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Vestiyer Odası 2", DisplayOrder = 56, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/08-kiler.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Kiler 1", DisplayOrder = 57, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/09-kiler.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Kiler 2", DisplayOrder = 58, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/10-mutfak.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Mutfak 1", DisplayOrder = 59, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/11-mutfak.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Mutfak 2", DisplayOrder = 60, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/12-salon.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Salon 1", DisplayOrder = 61, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/13-salon.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Salon 2", DisplayOrder = 62, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/14-salon.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Salon 3", DisplayOrder = 63, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/15-salon.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Salon 4", DisplayOrder = 64, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/16-salon.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Salon 5", DisplayOrder = 65, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/17-salon.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Salon 6", DisplayOrder = 66, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/18-salon.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Salon 7", DisplayOrder = 67, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/19-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Ebeveyn Odası 1", DisplayOrder = 68, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/20-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Ebeveyn Odası 2", DisplayOrder = 69, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/21-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Ebeveyn Odası 3", DisplayOrder = 70, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/22-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Ebeveyn Odası 4", DisplayOrder = 71, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/23-giyinme-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Giyinme Odası 1", DisplayOrder = 72, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/24-giyinme-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Giyinme Odası 2", DisplayOrder = 73, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/25-giyinme-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Giyinme Odası 3", DisplayOrder = 74, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/26-ebeveyn-banyosu.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Ebeveyn Banyosu", DisplayOrder = 75, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/27-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Ebeveyn Odası 5", DisplayOrder = 76, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/28-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Ebeveyn Odası 6", DisplayOrder = 77, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/29-ebeveyn-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Ebeveyn Odası 7", DisplayOrder = 78, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/30-yatak-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Yatak Odası 1", DisplayOrder = 79, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/31-yatak-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Yatak Odası 2", DisplayOrder = 80, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/32-yatak-odasi-banyo.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Yatak Odası Banyo", DisplayOrder = 81, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/33-yatak-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Yatak Odası 3", DisplayOrder = 82, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/34-yatak-odasi.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Yatak Odası 4", DisplayOrder = 83, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/35-yatak-odasi-1.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Yatak Odası 5", DisplayOrder = 84, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/36-yatak-odasi-2.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Yatak Odası 6", DisplayOrder = 85, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/37-oda-1.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Oda 1", DisplayOrder = 86, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/38-oda-2.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Oda 2", DisplayOrder = 87, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/39-oda-3.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – Oda 3", DisplayOrder = 88, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/40-wc-1.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – WC 1", DisplayOrder = 89, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/41-wc-2.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan – WC 2", DisplayOrder = 90, Category = "Optional Interior" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/optional-interior/originals/42-ic-mekan.jpg", AltText = "La Via Villalar 1. Etap Opsiyonel İç Mekan görünümü 42", DisplayOrder = 91, Category = "Optional Interior" },

            // "Sosyal Alan" (Social Areas), client curation, 2026-08-20 — copies/
            // references 5 of the real Exterior photos above (same ImagePath, a
            // second ProjectImage row with a different Category, no file
            // duplication) so they also surface in the Gallery's Social Areas
            // filter and feed the Social Facilities cards.
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/02.jpg", AltText = "La Via Villalar 1. Etap sosyal alan görünümü 1", DisplayOrder = 92, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/03.jpg", AltText = "La Via Villalar 1. Etap sosyal alan görünümü 2", DisplayOrder = 93, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/04.jpg", AltText = "La Via Villalar 1. Etap sosyal alan görünümü 3", DisplayOrder = 94, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/07.jpg", AltText = "La Via Villalar 1. Etap sosyal alan görünümü 4", DisplayOrder = 95, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/09.jpg", AltText = "La Via Villalar 1. Etap sosyal alan görünümü 5", DisplayOrder = 96, Category = "Social Areas" }
        };
    }

    // La Via Villalar 1. Etap's real architectural drawings (kat-planlari/
    // standart-ic-mekan-kat-plani/ and .../opsiyonelli-ic-mekan-kat-plani/,
    // 2 files each — zemin kat + 1. kat), copied to floorplans/originals/
    // with clean names. Two logical groups ("Standart Kat Planları" /
    // "Opsiyonel Kat Planları" per the client's request) are expressed the
    // same way La Fiore Karabağ 2. Etap expresses its block groups: each
    // FloorPlan row's ApartmentType is a flat "{Group} – {Kat}" label, so
    // the existing switcher/carousel needs no changes. Net/Gross/Sales
    // Gross figures and room breakdown are still placeholder — no real
    // per-unit specs were supplied for this project, same as
    // BuildLaFioreKarabag2EtapFloorPlans.
    private static List<FloorPlan> BuildKuyuluLaViaVillalarFloorPlans()
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "Standart Kat Planları – Zemin Kat",
                ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/floorplans/originals/standart-zemin-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "Standart Kat Planları – 1. Kat",
                ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/floorplans/originals/standart-1-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 2,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "Opsiyonel Kat Planları – Zemin Kat",
                ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/floorplans/originals/opsiyonel-zemin-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 3,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            new()
            {
                ApartmentType = "Opsiyonel Kat Planları – 1. Kat",
                ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/floorplans/originals/opsiyonel-1-kat.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 4,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            },
            // Çatı Katı (Vaziyet Planı/Çatı Katı/Katalog/Konsept revision,
            // 2026-08-09) — a new roof-floor drawing supplied under
            // kat-planlari/4-KUYULU ÇATI PLANI.jpeg, copied to
            // floorplans/originals/cati-kati.jpg. One shared entry (not
            // duplicated per Standart/Opsiyonel group) appended after the
            // existing 4, same placeholder stats/room shape since no real
            // per-unit specs were supplied for it either.
            new()
            {
                ApartmentType = "Çatı Katı",
                ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/floorplans/originals/cati-kati.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = 5,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            }
        };
    }

    // Hero "Vaziyet Planı" site plan image (Vaziyet Planı/Çatı Katı/Katalog/
    // Konsept revision, 2026-08-09) — 1 real master plan drawing supplied
    // under vaziyet-plani/1.VAZİYET.jpg, converted to WebP via ThumbnailTool
    // --single (1600w/88q, same recipe as La Fiore Karabağ 2. Etap's
    // site-plan-N.webp above). Only one drawing was supplied, so the Hero's
    // Media Viewer group has no Prev/Next here — see ProjectSitePlanImage.
    private static List<ProjectSitePlanImage> BuildKuyuluLaViaVillalarSitePlanImages()
    {
        return new List<ProjectSitePlanImage>
        {
            new() { ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/site-plan-1.webp", AltText = "La Via Villalar 1. Etap vaziyet planı", DisplayOrder = 1 }
        };
    }

    // Concept section's 3-slide image carousel (Vaziyet Planı/Çatı Katı/
    // Katalog/Konsept revision, 2026-08-09) — this project has no concept
    // video, so it gets the same image-only carousel as La Fiore Karabağ 2.
    // Etap (see BuildLaFioreKarabag2EtapConceptImages), reusing the exact
    // same carousel markup/CSS/JS. Images are 3 randomly selected shots from
    // the Exterior gallery pool (BuildKuyuluLaViaVillalarImages above):
    // 01.jpg (a single villa's facade), 09.jpg (a rooftop terrace/pool), and
    // 16.jpg (the full site's aerial masterplan) — an architecture/social
    // life/investment arc, same structure as La Fiore Karabağ 2. Etap's copy
    // but written specifically for this project, not reused from it.
    private static List<ProjectConceptImage> BuildKuyuluLaViaVillalarConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/01.jpg",
                Eyebrow = "Kuyulu'nun Sakin Dokusunda",
                Title = "Kendine Ait Bir Mimari Kimlik",
                Description = "La Via Villalar 1. Etap, her biri kendi bahçesi ve garajıyla bağımsız bir villa mimarisini Kuyulu'nun sakin dokusuyla buluşturuyor. Ahşap dokulu cepheler, zarif aydınlatmalar ve modern çizgiler, Ançın İnşaat imzasının kalite anlayışını her villada yeniden tanımlıyor.",
                DisplayOrder = 1
            },
            new()
            {
                ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/09.jpg",
                Eyebrow = "Çatıdan Bahçeye Ayrıcalık",
                Title = "Kendi Terasında Bir Yaşam",
                Description = "Her villanın kendi çatı terası, özel havuzu ve peyzajlı bahçesiyle tasarlanan La Via Villalar 1. Etap, sosyal yaşamı komşu villalarla paylaşılan ortak alanlara değil, ailenizin kendi mahremiyetine taşıyor. Gün batımını kendi terasınızdan izlemek artık bir ayrıcalık değil, günlük bir alışkanlık.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/16.jpg",
                Eyebrow = "Bugünden Yarına Değer",
                Title = "Kuyulu'da Ayrıcalıklı Bir Yatırım",
                Description = "Düzenli villa sıraları, geniş yeşil alanları ve sosyal tesis binasıyla bütüncül bir yerleşim planına sahip La Via Villalar 1. Etap, hem huzurlu bir yaşamı hem de Kuyulu'nun gelişen değeriyle birlikte güçlü bir yatırım fırsatını bir araya getiriyor.",
                DisplayOrder = 3
            }
        };
    }

    // Davutlar D Latis's real architectural drawings (client-supplied,
    // 2026-08-09, under kat-planlari/{a,b,c}-blok-planlar/), copied to
    // floorplans/originals/ with clean names. Each source drawing is a full
    // building floor showing every unit on that floor (mixed 1+0/1+1/2+1
    // types together, per-unit area labels baked into the artwork itself),
    // not a single apartment type — so, same as
    // BuildKuyuluLaViaVillalarFloorPlans/BuildLeJardinFloorPlans above, each
    // FloorPlan row's "ApartmentType" badge holds a "{Blok} – {Kat}" label
    // instead of a unit size, and the existing switcher/carousel needs no
    // changes. Net/Gross/Sales Gross figures and room breakdown are still
    // the shared placeholder shape (no real per-unit specs were supplied at
    // this granularity). kat-planlari/genel-planlar/ (7 whole-site plans
    // showing all 3 blocks + amenities together) is a different thing — the
    // project's "Vaziyet Planı" site plan — and is wired through
    // Project.SitePlanImages instead, not into this list.
    private static List<FloorPlan> BuildDavutlarDLatisFloorPlans()
    {
        var placeholderRooms = new Func<List<FloorPlanRoom>>(() => new List<FloorPlanRoom>
        {
            new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
            new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
            new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
            new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
            new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
            new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
        });

        var entries = new (string ApartmentType, string FileName)[]
        {
            ("A Blok – 2. Bodrum Katı", "a-blok-2-bodrum-kati"),
            ("A Blok – 1. Bodrum Katı", "a-blok-1-bodrum-kati"),
            ("A Blok – Zemin Kat", "a-blok-zemin-kat"),
            ("A Blok – 1. Kat", "a-blok-1-kat"),
            ("A Blok – 2. Kat", "a-blok-2-kat"),
            ("A Blok – 3. Kat", "a-blok-3-kat"),
            ("A Blok – Çatı Katı", "a-blok-cati-kati"),
            ("B Blok – 2. Bodrum Katı", "b-blok-2-bodrum-kati"),
            ("B Blok – 1. Bodrum Katı", "b-blok-1-bodrum-kati"),
            ("B Blok – Zemin Kat", "b-blok-zemin-kat"),
            ("B Blok – 1. Kat", "b-blok-1-kat"),
            ("B Blok – 2. Kat", "b-blok-2-kat"),
            ("B Blok – 3. Kat", "b-blok-3-kat"),
            ("C Blok – 2. Bodrum Katı", "c-blok-2-bodrum-kati"),
            ("C Blok – 1. Bodrum Katı", "c-blok-1-bodrum-kati"),
            ("C Blok – Zemin Kat", "c-blok-zemin-kat"),
            ("C Blok – 1. Kat", "c-blok-1-kat"),
            ("C Blok – 2. Kat", "c-blok-2-kat"),
            ("C Blok – 3. Kat", "c-blok-3-kat")
        };

        return entries
            .Select((entry, index) => new FloorPlan
            {
                ApartmentType = entry.ApartmentType,
                ImagePath = $"/images/projects/davutlar-d-latis/floorplans/originals/{entry.FileName}.jpg",
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = index + 1,
                Rooms = placeholderRooms()
            })
            .ToList();
    }

    // Concept section's 5-slide video carousel (Davutlar D Latis Concept
    // redesign, 2026-08-09) — slide 1 reuses the project's existing concept
    // video (concept/video.mp4 + poster.webp); slides 2–5 are the four
    // newly-supplied client videos (konsept-video/video1-web.mp4 through
    // video4-web.mp4, in that order, per the client's explicit instruction),
    // each paired with a poster frame extracted from its own video via
    // ffmpeg + ThumbnailTool (concept/poster-video1.webp..poster-video4.webp).
    //
    // Eyebrow/Title/Description copy is PLACEHOLDER — no presentation
    // transcript was available in this session (docs/... Placeholder Content
    // rule). Recombines only facts already approved elsewhere in this file
    // (ShortDescription/ConceptDescription: Dilek Yarımadası Milli Parkı,
    // Efes, Meryem Ana Evi, Davutlar's thermal waters, spa/fitness, Ege's
    // turizm başkenti, yatırım fırsatı) across 5 non-overlapping topics
    // (location/nature, thermal wellness, architecture, social life,
    // investment) rather than inventing new claims. Replace with the
    // client's real copy once supplied.
    private static List<ProjectConceptVideo> BuildDavutlarDLatisConceptVideos()
    {
        var entries = new (string VideoPath, string PosterPath, string Eyebrow, string Title, string Description)[]
        {
            (
                "/images/projects/davutlar-d-latis/concept/video.mp4",
                "/images/projects/davutlar-d-latis/concept/poster.webp",
                "Kuşadası'na Hoş Geldiniz",
                "Kuşadası'nın Yeni Yaşam Merkezi",
                "Dilek Yarımadası Milli Parkı'nın eteğinde, Efes ve Meryem Ana Evi gibi kadim mirasların komşuluğunda yükselen Davutlar D Latis, bakir doğayla iç içe ayrıcalıklı bir yaşam alanı sunuyor."
            ),
            (
                "/images/projects/davutlar-d-latis/konsept-video/video1-web.mp4",
                "/images/projects/davutlar-d-latis/concept/poster-video1.webp",
                "Şifalı Sularla Buluşun",
                "Termal Wellness Ayrıcalığı",
                "Davutlar'ın eşsiz termal sularını modern mimariyle buluşturan proje, sağlıklı ve dingin bir yaşamı her daireye taşıyan bütünsel bir wellness konsepti sunuyor."
            ),
            (
                "/images/projects/davutlar-d-latis/konsept-video/video2-web.mp4",
                "/images/projects/davutlar-d-latis/concept/poster-video2.webp",
                "Çağdaş Bir İmza",
                "Modern Mimari",
                "A, B ve C Blok'ları oluşturan çağdaş mimari çizgi, konforu ön planda tutan daire tipleriyle her yaşam tarzına uyum sağlıyor."
            ),
            (
                "/images/projects/davutlar-d-latis/konsept-video/video3-web.mp4",
                "/images/projects/davutlar-d-latis/concept/poster-video3.webp",
                "Her Gün Bir Ayrıcalık",
                "Sosyal Yaşam",
                "Spa ve fitness alanlarından ortak yaşam mekânlarına uzanan sosyal donatılar, günlük yaşamı bir tatil deneyimine dönüştürüyor."
            ),
            (
                "/images/projects/davutlar-d-latis/konsept-video/video4-web.mp4",
                "/images/projects/davutlar-d-latis/concept/poster-video4.webp",
                "Geleceğe Değer Katın",
                "Prestijli Yatırım",
                "Ege'nin turizm başkentinde yükselen Davutlar D Latis, huzurlu bir yaşamı güçlü bir yatırım fırsatıyla bir araya getiriyor."
            )
        };

        return entries
            .Select((entry, index) => new ProjectConceptVideo
            {
                VideoPath = entry.VideoPath,
                PosterPath = entry.PosterPath,
                Eyebrow = entry.Eyebrow,
                Title = entry.Title,
                Description = entry.Description,
                DisplayOrder = index + 1
            })
            .ToList();
    }

    // Le Jardin's real photography (client-supplied, 2026-08-06). Source
    // folders were wwwroot/images/projects/le-jardin/dis-mekan-gorselleri/
    // (32 exterior renders) and ic-mekan-gorselleri/{zemin-kat,birinci-kat}/
    // (interior renders, further split into per-room folders), reorganized
    // into gallery/exterior/originals/ and gallery/interior/{zemin-kat,
    // birinci-kat}/originals/ (clean ASCII filenames), with the raw folders
    // left in place, untouched, as an archival copy — same shape as
    // BuildKuyuluLaViaVillalarImages. The per-room interior split (Salon &
    // Mutfak, Misafir Odası, ... under Zemin Kat; Ebeveyn Yatak Odası,
    // Ebeveyn Banyo, ... under 1. Kat) is flattened into one Category
    // ("Interior") + Block ("Zemin Kat" / "1. Kat") pair per image, reusing
    // the Block chip mechanism the La Fiore Karabağ 2. Etap Gallery pilot
    // already added to _ProjectGallery.cshtml/site.js — this project's
    // Gallery needed zero markup/script changes, only this seed data.
    private static List<ProjectImage> BuildLeJardinImages()
    {
        return new List<ProjectImage>
        {
            // Exterior — dis-mekan-gorselleri/ (client-supplied real renders, 2026-08-06).
            // exterior-28.jpg is also the dedicated Hero Banner source and exterior-17.jpg
            // the dedicated Cover Image source, both derived once into their own
            // banner.webp/cover.webp files below rather than read live from this list, so
            // archiving exterior-17 out of the active Gallery below doesn't affect either.
            // exterior-28.jpg stays in the kept set too (Davutlar D Latis/La Via Villalar
            // precedent of a shared source photo remaining a normal Gallery card as well).
            // Exterior — client curation (2026-08-20): restricted to 5 of the
            // original 32 photos (1, 5, 23, 24, 28). The rest are archived,
            // not deleted — exterior-01..32.jpg all still exist on disk;
            // restore any of them by uncommenting its line below.
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-01.jpg", AltText = "Le Jardin dış cephe görünümü 1", DisplayOrder = 1, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-02.jpg", AltText = "Le Jardin dış cephe görünümü 2", DisplayOrder = 2, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-03.jpg", AltText = "Le Jardin dış cephe görünümü 3", DisplayOrder = 3, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-04.jpg", AltText = "Le Jardin dış cephe görünümü 4", DisplayOrder = 4, Category = "Exterior" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-05.jpg", AltText = "Le Jardin dış cephe görünümü 5", DisplayOrder = 5, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-06.jpg", AltText = "Le Jardin dış cephe görünümü 6", DisplayOrder = 6, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-07.jpg", AltText = "Le Jardin dış cephe görünümü 7", DisplayOrder = 7, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-08.jpg", AltText = "Le Jardin dış cephe görünümü 8", DisplayOrder = 8, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-09.jpg", AltText = "Le Jardin dış cephe görünümü 9", DisplayOrder = 9, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-10.jpg", AltText = "Le Jardin dış cephe görünümü 10", DisplayOrder = 10, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-11.jpg", AltText = "Le Jardin dış cephe görünümü 11", DisplayOrder = 11, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-12.jpg", AltText = "Le Jardin dış cephe görünümü 12", DisplayOrder = 12, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-13.jpg", AltText = "Le Jardin dış cephe görünümü 13", DisplayOrder = 13, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-14.jpg", AltText = "Le Jardin dış cephe görünümü 14", DisplayOrder = 14, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-15.jpg", AltText = "Le Jardin dış cephe görünümü 15", DisplayOrder = 15, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-16.jpg", AltText = "Le Jardin dış cephe görünümü 16", DisplayOrder = 16, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-17.jpg", AltText = "Le Jardin dış cephe görünümü 17", DisplayOrder = 17, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-18.jpg", AltText = "Le Jardin dış cephe görünümü 18", DisplayOrder = 18, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-19.jpg", AltText = "Le Jardin dış cephe görünümü 19", DisplayOrder = 19, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-20.jpg", AltText = "Le Jardin dış cephe görünümü 20", DisplayOrder = 20, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-21.jpg", AltText = "Le Jardin dış cephe görünümü 21", DisplayOrder = 21, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-22.jpg", AltText = "Le Jardin dış cephe görünümü 22", DisplayOrder = 22, Category = "Exterior" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-23.jpg", AltText = "Le Jardin dış cephe görünümü 23", DisplayOrder = 23, Category = "Exterior" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-24.jpg", AltText = "Le Jardin dış cephe görünümü 24", DisplayOrder = 24, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-25.jpg", AltText = "Le Jardin dış cephe görünümü 25", DisplayOrder = 25, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-26.jpg", AltText = "Le Jardin dış cephe görünümü 26", DisplayOrder = 26, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-27.jpg", AltText = "Le Jardin dış cephe görünümü 27", DisplayOrder = 27, Category = "Exterior" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-28.jpg", AltText = "Le Jardin dış cephe görünümü 28", DisplayOrder = 28, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-29.jpg", AltText = "Le Jardin dış cephe görünümü 29", DisplayOrder = 29, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-30.jpg", AltText = "Le Jardin dış cephe görünümü 30", DisplayOrder = 30, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-31.jpg", AltText = "Le Jardin dış cephe görünümü 31", DisplayOrder = 31, Category = "Exterior" },
            // new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-32.jpg", AltText = "Le Jardin dış cephe görünümü 32", DisplayOrder = 32, Category = "Exterior" },

            // Interior — ic-mekan-gorselleri/zemin-kat/ and .../birinci-kat/, flattened from
            // their per-room source folders into one Category ("Interior") split by Block
            // ("Zemin Kat" / "1. Kat") — reuses the Block chip mechanism built for La Fiore
            // Karabağ 2. Etap (see _ProjectGallery.cshtml/site.js), so no Gallery code changes
            // were needed for this project's two-level Category → Floor filter.
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-01.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 1", DisplayOrder = 33, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-02.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 2", DisplayOrder = 34, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-03.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 3", DisplayOrder = 35, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-04.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 4", DisplayOrder = 36, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-05.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 5", DisplayOrder = 37, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-06.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 6", DisplayOrder = 38, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-07.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 7", DisplayOrder = 39, Category = "Interior", Block = "Zemin Kat" },
            // Archived (2026-08-20 client curation) — not deleted, file stays on disk;
            // restore by uncommenting this line.
            // new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-08.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 8", DisplayOrder = 40, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-09.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 9", DisplayOrder = 41, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-10.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 10", DisplayOrder = 42, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-11.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 11", DisplayOrder = 43, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-12.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 12", DisplayOrder = 44, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-13.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 13", DisplayOrder = 45, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-14.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 14", DisplayOrder = 46, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-15.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 15", DisplayOrder = 47, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-16.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Salon & Mutfak 16", DisplayOrder = 48, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/misafir-odasi-01.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Misafir Odası 1", DisplayOrder = 49, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/misafir-odasi-02.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Misafir Odası 2", DisplayOrder = 50, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/misafir-odasi-03.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Misafir Odası 3", DisplayOrder = 51, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/misafir-odasi-04.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Misafir Odası 4", DisplayOrder = 52, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/misafir-odasi-banyo-01.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Misafir Odası Banyosu 1", DisplayOrder = 53, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/misafir-odasi-banyo-02.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Misafir Odası Banyosu 2", DisplayOrder = 54, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/tuvalet-01.jpg", AltText = "Le Jardin İç Mekan – Zemin Kat – Tuvalet 1", DisplayOrder = 55, Category = "Interior", Block = "Zemin Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-yatak-odasi-01.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Yatak Odası 1", DisplayOrder = 56, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-yatak-odasi-02.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Yatak Odası 2", DisplayOrder = 57, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-yatak-odasi-03.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Yatak Odası 3", DisplayOrder = 58, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-yatak-odasi-04.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Yatak Odası 4", DisplayOrder = 59, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-yatak-odasi-05.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Yatak Odası 5", DisplayOrder = 60, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-yatak-odasi-06.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Yatak Odası 6", DisplayOrder = 61, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-banyo-01.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Banyo 1", DisplayOrder = 62, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-banyo-02.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Banyo 2", DisplayOrder = 63, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-banyo-03.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Banyo 3", DisplayOrder = 64, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/ebeveyn-banyo-04.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Ebeveyn Banyo 4", DisplayOrder = 65, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/yatak-odasi-balkonlu-01.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Yatak Odası (Balkonlu) 1", DisplayOrder = 66, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/yatak-odasi-balkonlu-02.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Yatak Odası (Balkonlu) 2", DisplayOrder = 67, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/yatak-odasi-balkonlu-03.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Yatak Odası (Balkonlu) 3", DisplayOrder = 68, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/yatak-odasi-balkonlu-04.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Yatak Odası (Balkonlu) 4", DisplayOrder = 69, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/yatak-odasi-balkonlu-05.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Yatak Odası (Balkonlu) 5", DisplayOrder = 70, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/yatak-odasi-balkonlu-06.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Yatak Odası (Balkonlu) 6", DisplayOrder = 71, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/cocuk-odasi-01.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Çocuk Odası 1", DisplayOrder = 72, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/cocuk-odasi-02.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Çocuk Odası 2", DisplayOrder = 73, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/cocuk-odasi-03.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Çocuk Odası 3", DisplayOrder = 74, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/genel-banyo-01.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Genel Banyo 1", DisplayOrder = 75, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/genel-banyo-02.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Genel Banyo 2", DisplayOrder = 76, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/genel-banyo-03.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Genel Banyo 3", DisplayOrder = 77, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/genel-banyo-04.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Genel Banyo 4", DisplayOrder = 78, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/genel-banyo-05.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Genel Banyo 5", DisplayOrder = 79, Category = "Interior", Block = "1. Kat" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/interior/birinci-kat/originals/genel-banyo-06.jpg", AltText = "Le Jardin İç Mekan – 1. Kat – Genel Banyo 6", DisplayOrder = 80, Category = "Interior", Block = "1. Kat" },

            // "Sosyal Alan" (Social Areas), client curation, 2026-08-20 — copies/
            // references 9 of the real Exterior photos above (same ImagePath, a
            // second ProjectImage row with a different Category, no file
            // duplication) so they also surface in the Gallery's Social Areas
            // filter and feed the Social Facilities cards.
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-15.jpg", AltText = "Le Jardin sosyal alan görünümü 1", DisplayOrder = 81, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-16.jpg", AltText = "Le Jardin sosyal alan görünümü 2", DisplayOrder = 82, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-17.jpg", AltText = "Le Jardin sosyal alan görünümü 3", DisplayOrder = 83, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-18.jpg", AltText = "Le Jardin sosyal alan görünümü 4", DisplayOrder = 84, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-20.jpg", AltText = "Le Jardin sosyal alan görünümü 5", DisplayOrder = 85, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-21.jpg", AltText = "Le Jardin sosyal alan görünümü 6", DisplayOrder = 86, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-22.jpg", AltText = "Le Jardin sosyal alan görünümü 7", DisplayOrder = 87, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-26.jpg", AltText = "Le Jardin sosyal alan görünümü 8", DisplayOrder = 88, Category = "Social Areas" },
            new() { ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-30.jpg", AltText = "Le Jardin sosyal alan görünümü 9", DisplayOrder = 89, Category = "Social Areas" }
        };
    }

    // Le Jardin's real architectural drawings (kat-planlari/, 2 files — Zemin
    // Kat + 1. Kat — copied to floorplans/originals/ with clean names). This
    // project is a single villa layout (not multiple apartment types), so
    // each FloorPlan row's "ApartmentType" badge holds the floor name
    // instead of a unit size. Room list below is transcribed directly from
    // the drawings (client-supplied, verified 2026-08-20) — every label and
    // m² value matches what's printed on zemin-kat.jpg/1-kat.jpg exactly.
    // Net/Brüt now live in the standard stats box (NetAreaM2/GrossAreaM2,
    // same box every other project uses — see _FloorPlans.cshtml) rather
    // than as room-list rows (client request, 2026-08-20): 1. Kat keeps its
    // own printed per-floor totals (119,64/173,31), while Zemin Kat
    // deliberately shows the drawings' combined "TOPLAM" totals for the
    // whole villa (247,25/323,08 — both floors' figures added: 127,61 +
    // 119,64 = 247,25 net, 149,77 + 173,31 = 323,08 brüt) instead of its own
    // per-floor total, per explicit client instruction. SalesGrossAreaM2
    // stays at 0 on both rows since the drawings never print a "Satışa Esas
    // Brüt Alan" figure — ProjectsController.Details maps a 0 Sales Gross to
    // null independently of Net/Gross, so that third stat box simply never
    // renders for Le Jardin while Net Alan/Brüt Alan still show normally.
    private static List<FloorPlan> BuildLeJardinFloorPlans()
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "Zemin Kat",
                ImagePath = "/images/projects/le-jardin/floorplans/originals/zemin-kat.jpg",
                NetAreaM2 = 247.25m,
                GrossAreaM2 = 323.08m,
                SalesGrossAreaM2 = 0m,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Giriş", AreaM2 = 4.00m, DisplayOrder = 1 },
                    new() { Name = "Antre", AreaM2 = 7.09m, DisplayOrder = 2 },
                    new() { Name = "Banyo", AreaM2 = 3.69m, DisplayOrder = 3 },
                    new() { Name = "Banyo", AreaM2 = 4.92m, DisplayOrder = 4 },
                    new() { Name = "Yatak Odası", AreaM2 = 13.52m, DisplayOrder = 5 },
                    new() { Name = "Asansör Boşluğu", AreaM2 = 2.25m, DisplayOrder = 6 },
                    new() { Name = "Merdiven", AreaM2 = 7.99m, DisplayOrder = 7 },
                    new() { Name = "Salon", AreaM2 = 31.76m, DisplayOrder = 8 },
                    new() { Name = "Mutfak", AreaM2 = 22.39m, DisplayOrder = 9 },
                    new() { Name = "Teras", AreaM2 = 23.00m, DisplayOrder = 10 },
                    new() { Name = "Kış Bahçesi", AreaM2 = 30.00m, DisplayOrder = 11 },
                    new() { Name = "Havuz", AreaM2 = 27.00m, DisplayOrder = 12 }
                }
            },
            new()
            {
                ApartmentType = "1. Kat",
                ImagePath = "/images/projects/le-jardin/floorplans/originals/1-kat.jpg",
                NetAreaM2 = 119.64m,
                GrossAreaM2 = 173.31m,
                SalesGrossAreaM2 = 0m,
                DisplayOrder = 2,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Ebeveyn Yatak Odası", AreaM2 = 23.60m, DisplayOrder = 1 },
                    new() { Name = "Banyo", AreaM2 = 6.45m, DisplayOrder = 2 },
                    new() { Name = "Banyo", AreaM2 = 6.47m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 23.67m, DisplayOrder = 4 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 14.35m, DisplayOrder = 5 },
                    new() { Name = "Hol", AreaM2 = 12.81m, DisplayOrder = 6 },
                    new() { Name = "Galeri Boşluğu", AreaM2 = 24.14m, DisplayOrder = 7 },
                    new() { Name = "Asansör Boşluğu", AreaM2 = 2.14m, DisplayOrder = 8 },
                    new() { Name = "Balkon", AreaM2 = 3.08m, DisplayOrder = 9 },
                    new() { Name = "Balkon", AreaM2 = 0.80m, DisplayOrder = 10 },
                    new() { Name = "Balkon", AreaM2 = 26.30m, DisplayOrder = 11 }
                }
            }
        };
    }

    // Le Jardin's Concept carousel (Le Jardin Concept carousel
    // generalization, 2026-08-09) — the pilot for ConceptSlideModel's mixed
    // video/image carousel: 2 concept videos (re-encoded from the client's
    // raw exports at wwwroot/images/projects/le-jardin/konsept/ to match
    // Davutlar D Latis's konsept-video/*-web.mp4 profile — H.264 High,
    // yuv420p, 24fps, faststart, AAC) followed by 3 of the project's own
    // real Exterior gallery photos. DisplayOrder is deliberately continued
    // from this method (3, 4, 5) rather than restarting at 1 — see
    // BuildLeJardinConceptImages below and ConceptSlideModel's own remarks —
    // so ProjectsController.Details's merge-by-DisplayOrder interleaves the
    // two sequences into video, video, image, image, image. Eyebrow/Title/
    // Description throughout this project's Concept slides are adapted from
    // the client-supplied "Le Jardin" room-by-room presentation text (2026-
    // 08-09), not invented copy — Slide 1 from the Giriş section's overall
    // "Akdeniz esintili... modern çizgi" framing, Slide 2 from Giriş's
    // begonvil-kemer/taş-kaplama entrance detail (matches this video's own
    // poster frame), Slides 3-5 each from a different paragraph of the
    // Teras-Havuz-Bahçe section (terrace seating, pool/waterfall, garden
    // landscaping respectively).
    private static List<ProjectConceptVideo> BuildLeJardinConceptVideos()
    {
        return new List<ProjectConceptVideo>
        {
            new()
            {
                VideoPath = "/images/projects/le-jardin/concept/video1.mp4",
                PosterPath = "/images/projects/le-jardin/concept/poster-video1.webp",
                Eyebrow = "İlk İzlenim",
                Title = "Akdeniz Ruhuyla Yükselen Mimari",
                Description = "Açık renk taş kaplamalı cepheleri, begonvillerle sarılı girişleri ve yeşilin içine yerleşen konumuyla Le Jardin, modern mimariyi Akdeniz'in sıcak karakteriyle buluşturan davetkâr bir yaşam alanı sunuyor.",
                DisplayOrder = 1
            },
            new()
            {
                VideoPath = "/images/projects/le-jardin/concept/video2.mp4",
                PosterPath = "/images/projects/le-jardin/concept/poster-video2.webp",
                Eyebrow = "Davetkâr Bir Karşılama",
                Title = "Begonvillerin Sardığı Giriş",
                Description = "Kapı çevresini saran begonvillerin oluşturduğu doğal kemer ve açık renk taş kaplamalı cephesiyle giriş, Le Jardin'e sofistike ve sanatsal bir kimlik kazandırıyor.",
                DisplayOrder = 2
            }
        };
    }

    private static List<ProjectConceptImage> BuildLeJardinConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-30.jpg",
                Eyebrow = "Gün Batımından Geceye",
                Title = "Sıcak Bir Dış Mekân Yaşamı",
                Description = "Yumuşak tonlu modern oturma grubu ve dekoratif aydınlatmalarıyla teras, Le Jardin'i hem akşam hem gündüz keyifle kullanılabilen, sıcak bir yaşam alanına dönüştürüyor.",
                DisplayOrder = 3
            },
            new()
            {
                ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-24.jpg",
                Eyebrow = "Suyla Buluşan Yaşam",
                Title = "Şelale Efektli Modern Havuz",
                Description = "24 m²'lik havuzu ve dekoratif kirişten dökülen şelale efektiyle bu alan, Le Jardin'in en dikkat çekici odak noktalarından birine dönüşüyor.",
                DisplayOrder = 4
            },
            new()
            {
                ImagePath = "/images/projects/le-jardin/gallery/exterior/originals/exterior-16.jpg",
                Eyebrow = "Akdeniz'in Yeşil Dokusu",
                Title = "Zeytin Ağacıyla Bütünleşen Bahçe",
                Description = "Geniş çim alanı, rengarenk begonvilleri ve tek başına duran bonsai zeytin ağacıyla bahçe, modern mimarinin düzenli çizgilerini Akdeniz bitki dokusuyla buluşturan sakin ama lüks bir dış yaşam alanı sunuyor.",
                DisplayOrder = 5
            }
        };
    }

    // Le Jardin's Site Plan (Hero Banner Vaziyet Planı revision, 2026-08-09)
    // — wires this project into the same shared SitePlanImages architecture
    // as Davutlar D Latis/La Fiore Karabağ 2. Etap/La Via AVM: HeroBannerProjectDetail
    // renders the "Vaziyet Planı" button whenever SitePlanImageUrls is
    // non-empty and opens it in the existing shared Media Viewer (zoom/pan/
    // fullscreen/Prev-Next/counter — all pre-existing, generic, no new code).
    // Source is wwwroot/images/projects/le-jardin/vaziyet-plani/vaziyet2.jpg
    // (the client's single site-plan render — note: singular "vaziyet-plani",
    // one file, not a "vaziyet-planlari" folder), downsized to 5000px wide and
    // re-encoded to site-plan-1.webp matching La Fiore Karabağ 2. Etap's own
    // site-plan-N.webp convention (same directory level, same naming).
    private static List<ProjectSitePlanImage> BuildLeJardinSitePlanImages()
    {
        return new List<ProjectSitePlanImage>
        {
            new() { ImagePath = "/images/projects/le-jardin/site-plan-1.webp", AltText = "Le Jardin vaziyet planı", DisplayOrder = 1 }
        };
    }

    // Nysa Gold's real photography, revision 2 (client asset reorganization,
    // 2026-08-09 — supersedes the 2026-08-06 partial set below/above). Source
    // folders are wwwroot/images/projects/nysa-gold/dis-mekan-gorselleri/ (60
    // exterior shots), .../ic-mekan-gorselleri/<apartment-type>/ (239 photos
    // + 7 short room videos across 10 client-named apartment-type
    // subfolders — "2+1 A Tipi " and "2+1 A_ Tipi" merged into one 48-photo
    // category per client confirmation, both original batches were the same
    // unit) and .../satis-ofisi-gorselleri/ (20 photos, new "Sales Office"
    // category — Gallery video support, 2026-08-09). Every photo/video was
    // copied preserving the client's own room-numbering sequence embedded in
    // each original filename (e.g. "17-ebeveyn yatak odası (1).jpg"), never
    // re-sorted alphabetically — see the asset-processing notes in the
    // implementation report. Video rows (VideoPath set) sit at the exact
    // DisplayOrder position their raw file occupied in that sequence, so
    // Image → Video → Image ordering in the Gallery matches the source
    // exactly; ImagePath on a video row is its own poster frame, generated
    // from the same clip and run through the ordinary thumbnail pipeline
    // like any other photo (Gallery video support — ProjectImage.VideoPath).
    // Interior's apartment-type tier reuses the Block chip mechanism (La
    // Fiore Karabağ 2. Etap pilot / Le Jardin's floor-tier precedent).
    // Exterior and Sales Office get no sub-filtering (flat). Social Areas is
    // deliberately left out of this builder — the reorganized folders supply
    // no social-facility photos — and ReconcileNysaGoldMediaOverhaulAsync
    // below fully replaces the previous Images set rather than splicing
    // (unlike the prior revision), since this is a wholesale content
    // refresh, not an incremental addition.
    private static List<ProjectImage> BuildNysaGoldImages()
    {
        var images = new List<ProjectImage>();
        var order = 0;

        void AddExterior(int index, string ext)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/nysa-gold/gallery/exterior/originals/exterior-{index:D2}.{ext}",
                AltText = $"Nysa Gold Residence dış cephe görünümü {index}",
                DisplayOrder = order,
                Category = "Exterior"
            });
        }

        void AddSalesOffice(int index, string ext)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/nysa-gold/gallery/sales-office/originals/sales-office-{index:D2}.{ext}",
                AltText = $"Nysa Gold Residence satış ofisi görünümü {index}",
                DisplayOrder = order,
                Category = "Sales Office"
            });
        }

        void AddInteriorImage(string folder, string apartmentType, int index)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/nysa-gold/gallery/interior/{folder}/originals/interior-{index:D2}.jpg",
                AltText = $"Nysa Gold Residence {apartmentType} iç mekan görünümü {index}",
                DisplayOrder = order,
                Category = "Interior",
                Block = apartmentType
            });
        }

        void AddInteriorRange(string folder, string apartmentType, int fromIndex, int toIndex)
        {
            for (var i = fromIndex; i <= toIndex; i++)
            {
                AddInteriorImage(folder, apartmentType, i);
            }
        }

        // Poster frame (a normal image, thumbnail-pipelined like any photo)
        // + the optimized clip itself — see Gallery video support
        // (ProjectImage.VideoPath / _ProjectGallery.cshtml).
        void AddInteriorVideo(string folder, string apartmentType, int videoIndex)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/nysa-gold/gallery/interior/{folder}/originals/interior-video-{videoIndex:D2}-poster.jpg",
                VideoPath = $"/images/projects/nysa-gold/gallery/interior/{folder}/videos/interior-video-{videoIndex:D2}.mp4",
                AltText = $"Nysa Gold Residence {apartmentType} tanıtım videosu {videoIndex}",
                DisplayOrder = order,
                Category = "Interior",
                Block = apartmentType
            });
        }

        // Exterior — client curation (2026-08-20): restricted to 6 selected
        // shots out of the original 60. The other 54 are archived, not
        // deleted — exterior-01..60 all still exist on disk (see Concept's
        // own direct references to exterior-01/16/18/42 below, unaffected
        // since those don't go through this Images list). Restore by
        // widening exteriorKeep back toward 1..60.
        var exteriorJpeg = new HashSet<int> { 3, 5, 9, 55, 56, 57, 58, 59, 60 };
        var exteriorKeep = new[] { 3, 5, 25, 41, 42, 43 };
        foreach (var i in exteriorKeep)
        {
            AddExterior(i, exteriorJpeg.Contains(i) ? "jpeg" : "jpg");
        }

        // Interior — 10 apartment types, 239 photos + 7 videos, each video
        // inserted at its exact original sequence position.
        AddInteriorRange("bir-arti-bir-a-tipi", "1+1 A Tipi", 1, 13);
        AddInteriorRange("bir-arti-bir-b-tipi", "1+1 B Tipi", 1, 3);

        // 2+1 A Tipi — client curation (2026-08-20): restricted to 19 of the
        // original 48 photos. Archived, not deleted (the rest of
        // interior-01..48.jpg stay on disk); restore by reverting to
        // AddInteriorRange("iki-arti-bir-a-tipi", "2+1 A Tipi", 1, 48).
        foreach (var i in new[] { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 41 })
        {
            AddInteriorImage("iki-arti-bir-a-tipi", "2+1 A Tipi", i);
        }

        AddInteriorRange("iki-arti-bir-b-tipi", "2+1 B Tipi", 1, 27);

        // 2+1 C Tipi — client curation (2026-08-20): restricted to 2 of the
        // original 17 photos (both room-tour videos archived along with the
        // rest). Restore by reverting to the original range/video sequence
        // (1-17, video 1, image 18, video 2, 19-26).
        AddInteriorImage("iki-arti-bir-c-tipi", "2+1 C Tipi", 2);
        AddInteriorImage("iki-arti-bir-c-tipi", "2+1 C Tipi", 3);

        AddInteriorRange("iki-arti-bir-d-tipi", "2+1 D Tipi", 1, 6);

        // 2+1 E Tipi — client curation (2026-08-20): restricted to 2 of the
        // original 25 photos (room-tour video archived too). Restore by
        // reverting to the original range/video sequence (1-11, video, 12-25).
        AddInteriorImage("iki-arti-bir-e-tipi", "2+1 E Tipi", 4);
        AddInteriorImage("iki-arti-bir-e-tipi", "2+1 E Tipi", 13);

        AddInteriorRange("uc-arti-bir-a-tipi", "3+1 A Tipi", 1, 30);

        // 3+1 B Tipi — client curation (2026-08-20): restricted to 4 of the
        // original 26 photos (both room-tour videos archived too). Restore
        // by reverting to the original range/video sequence (1-21, video 1,
        // image 22, video 2, 23-26).
        AddInteriorImage("uc-arti-bir-b-tipi", "3+1 B Tipi", 3);
        AddInteriorImage("uc-arti-bir-b-tipi", "3+1 B Tipi", 4);
        AddInteriorImage("uc-arti-bir-b-tipi", "3+1 B Tipi", 12);
        AddInteriorImage("uc-arti-bir-b-tipi", "3+1 B Tipi", 22);

        // 4+1 Tipi — client curation (2026-08-20): image 2 and the second
        // room-tour video removed from the active gallery (archived, not
        // deleted — restore by reverting to AddInteriorRange(..., 1, 26) and
        // re-adding AddInteriorVideo("dort-arti-bir-tipi", "4+1 Tipi", 2)).
        AddInteriorImage("dort-arti-bir-tipi", "4+1 Tipi", 1);
        AddInteriorRange("dort-arti-bir-tipi", "4+1 Tipi", 3, 26);
        AddInteriorVideo("dort-arti-bir-tipi", "4+1 Tipi", 1);
        AddInteriorImage("dort-arti-bir-tipi", "4+1 Tipi", 27);
        AddInteriorRange("dort-arti-bir-tipi", "4+1 Tipi", 28, 35);

        // Sales Office — 20 photos, flat, new category (Gallery video
        // support / Nysa Gold revision, 2026-08-09).
        var salesOfficeJpeg = new HashSet<int> { 9, 20 };
        for (var i = 1; i <= 20; i++)
        {
            AddSalesOffice(i, salesOfficeJpeg.Contains(i) ? "jpeg" : "jpg");
        }

        // "Sosyal Alan" (Social Areas), client curation, 2026-08-20 — copies/
        // references 14 of the real Exterior photos above (same ImagePath,
        // a second ProjectImage row with a different Category, no file
        // duplication) so they also surface in the Gallery's Social Areas
        // filter and feed the Social Facilities cards (see
        // SocialFacilityModel / ProjectsController.SocialAreasCategory).
        // Selected from the project's full Exterior photo set independent
        // of that category's own restricted active list above.
        var socialAreaSourceIndexes = new[] { 12, 14, 17, 23, 24, 26, 27, 29, 34, 36, 45, 56, 57, 58 };
        var socialAreaIndex = 0;
        foreach (var sourceIndex in socialAreaSourceIndexes)
        {
            socialAreaIndex++;
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/nysa-gold/gallery/exterior/originals/exterior-{sourceIndex:D2}.{(exteriorJpeg.Contains(sourceIndex) ? "jpeg" : "jpg")}",
                AltText = $"Nysa Gold Residence sosyal alan görünümü {socialAreaIndex}",
                DisplayOrder = order,
                Category = "Social Areas"
            });
        }

        return images;
    }

    // Nysa Gold's real per-floor architectural drawings (client asset
    // reorganization, 2026-08-09; Bodrum Kat added 2026-08-10 once the
    // client converted its PDF to JPG themselves) — replaces the generic
    // single-"2+1" BuildPlaceholderFloorPlans placeholder. Whole-building
    // floor plans (Bodrum Kat, Zemin Kat, 1-7. Kat), same "repurpose
    // ApartmentType as the floor label" precedent as Le Jardin/La Fiore
    // Karabağ 2. Etap's own per-floor drawings. Every floor shares the same
    // temporary Net/Gross/Sales-Gross stats and room list as
    // BuildPlaceholderFloorPlans/every other project's own real-drawing
    // floor plans (client-requested, 2026-08-10, replacing the previously
    // blank stats row) — not real per-floor figures, clearly a placeholder
    // pending the client's confirmed data, same disclaimer as every other
    // project using this exact same placeholder set.
    private static List<FloorPlan> BuildNysaGoldFloorPlans()
    {
        var floors = new (string Label, string File)[]
        {
            ("Bodrum Kat", "bodrum-kat.jpg"),
            ("Zemin Kat", "zemin-kat.jpg"),
            ("1. Kat", "1-kat.jpg"),
            ("2. Kat", "2-kat.jpg"),
            ("3. Kat", "3-kat.jpg"),
            ("4. Kat", "4-kat.jpg"),
            ("5. Kat", "5-kat.jpg"),
            ("6. Kat", "6-kat.jpg"),
            ("7. Kat", "7-kat.jpg")
        };

        var floorPlans = new List<FloorPlan>();
        var order = 1;
        foreach (var (label, file) in floors)
        {
            floorPlans.Add(new FloorPlan
            {
                ApartmentType = label,
                ImagePath = $"/images/projects/nysa-gold/floorplans/originals/{file}",
                // Same temporary placeholder stats/rooms as every other
                // project's real-drawing floor plans (BuildPlaceholderFloorPlans/
                // BuildLaFioreKarabag2EtapFloorPlans/BuildLeJardinFloorPlans) —
                // client-requested, 2026-08-10, to replace the previously blank
                // stats row until real per-floor figures are supplied.
                NetAreaM2 = 68.00m,
                GrossAreaM2 = 95.00m,
                SalesGrossAreaM2 = 78.00m,
                DisplayOrder = order++,
                Rooms = new List<FloorPlanRoom>
                {
                    new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                    new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                    new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                    new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                    new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                    new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                }
            });
        }

        return floorPlans;
    }

    // Nysa Gold's Concept carousel (client asset reorganization, 2026-08-09)
    // — the project's first real Concept content; previously fell all the
    // way to the generic placeholder (no ConceptVideos/ConceptImages rows
    // existed). Slide 1 is the client's own concept walkthrough video
    // (re-encoded to the shared CRF 20-22/H.264/AAC/faststart profile);
    // Slides 2-5 reuse 4 of this project's own real Exterior gallery photos
    // (no duplication — same files BuildNysaGoldImages already seeds),
    // selected for wide architectural/landscaping/atmosphere composition
    // over repetitive angles or construction close-ups, per the client's
    // brief. Copy is descriptive of what each image actually shows, written
    // for Nysa Gold specifically — not reused from another project.
    private static List<ProjectConceptVideo> BuildNysaGoldConceptVideos()
    {
        return new List<ProjectConceptVideo>
        {
            new()
            {
                VideoPath = "/images/projects/nysa-gold/concept/video.mp4",
                PosterPath = "/images/projects/nysa-gold/concept/video-poster.webp",
                Eyebrow = "Yeni Nesil Yaşam",
                Title = "Nysa Gold Residence'ın Mimari Vizyonu",
                Description = "Zarif cepheleri, geniş balkonları ve özenle tasarlanmış sosyal alanlarıyla Nysa Gold Residence, modern mimariyi konforlu bir yaşam deneyimiyle buluşturuyor.",
                DisplayOrder = 1
            }
        };
    }

    private static List<ProjectConceptImage> BuildNysaGoldConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/nysa-gold/gallery/exterior/originals/exterior-01.jpg",
                Eyebrow = "Yukarıdan Bir Bakış",
                Title = "Peyzajla Bütünleşen Yerleşim",
                Description = "Havuzu, yeşil alanları ve düzenli site içi dolaşım aksıyla Nysa Gold Residence, kuşbakışı görünümünde bile dengeli ve ferah bir yerleşim planı sunuyor.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/nysa-gold/gallery/exterior/originals/exterior-16.jpg",
                Eyebrow = "Aile Odaklı Yaşam",
                Title = "Yeşilin İçinde Sosyal Alanlar",
                Description = "Çocuk oyun alanı ve geniş çim yüzeyleriyle donatılan iç bahçe, Nysa Gold Residence sakinlerine güvenli ve keyifli bir sosyal yaşam alanı sunuyor.",
                DisplayOrder = 3
            },
            new()
            {
                ImagePath = "/images/projects/nysa-gold/gallery/exterior/originals/exterior-18.jpg",
                Eyebrow = "Günbatımı Keyfi",
                Title = "Havuz Başında Huzurlu Anlar",
                Description = "Palmiye ağaçları ve şezlonglarıyla çevrelenen havuz alanı, gün batımının sıcak tonlarında Nysa Gold Residence'a otel konforunda bir dinlenme deneyimi katıyor.",
                DisplayOrder = 4
            },
            new()
            {
                ImagePath = "/images/projects/nysa-gold/gallery/exterior/originals/exterior-42.jpg",
                Eyebrow = "Geceye Özel Atmosfer",
                Title = "Işıklarla Aydınlanan Bir Yaşam Alanı",
                Description = "Peyzaj aydınlatması ve havuz çevresindeki ışık tasarımıyla Nysa Gold Residence, gece saatlerinde de davetkâr ve güvenli bir yaşam atmosferi sunuyor.",
                DisplayOrder = 5
            }
        };
    }

    // Nysa Gold's Site Plan, revision 2 (client asset reorganization,
    // 2026-08-09) — replaces the single stale site-plan.webp (derived from
    // the pre-reorganization sample drop) with 3 images derived from the
    // client's new vaziyet/ folder: the finished, labeled master plan
    // render (genelplan.jpg — the primary image) plus 2 satellite/aerial
    // reference views. The plan satellite screenshot has two source files
    // for the same shot (plain and with the site sketch overlaid); only the
    // annotated one is included; the plain duplicate adds nothing beyond
    // the "vaziyet/" folder.
    private static List<ProjectSitePlanImage> BuildNysaGoldSitePlanImages()
    {
        return new List<ProjectSitePlanImage>
        {
            new() { ImagePath = "/images/projects/nysa-gold/site-plan-1.webp", AltText = "Nysa Gold Residence vaziyet planı", DisplayOrder = 1 },
            new() { ImagePath = "/images/projects/nysa-gold/site-plan-2.webp", AltText = "Nysa Gold Residence arsa ve yerleşim görünümü", DisplayOrder = 2 },
            new() { ImagePath = "/images/projects/nysa-gold/site-plan-3.webp", AltText = "Nysa Gold Residence çevresel konum görünümü", DisplayOrder = 3 }
        };
    }

    // Not a seed — replaces an already-seeded Nysa Gold row's 10 generic
    // placeholder Exterior/Social Areas Images (2 exterior-0N.webp +
    // "Social Areas" borrowed from the same shoot) with BuildNysaGoldImages'
    // real Exterior + Interior set, while leaving the 5 existing Social Areas
    // rows in place untouched — same RemoveRange-then-rebuild shape as
    // ReconcileLeJardinGalleryAndCatalogueAsync below, but scoped to Images
    // only (Nysa Gold's FloorPlans are untouched per the brief). Guarded to
    // only run once (Images.Count == 10, the untouched original placeholder
    // set), so it never overwrites real content edited after this ran.
    // Not a seed — wholesale content refresh for Nysa Gold (client asset
    // reorganization, 2026-08-09): replaces whatever Images/FloorPlans/
    // SitePlanImages this project currently has with the real reorganized
    // set (BuildNysaGoldImages/BuildNysaGoldFloorPlans/
    // BuildNysaGoldSitePlanImages above), and seeds Concept for the first
    // time (ConceptVideos/ConceptImages, LocationImagePath — none of these
    // existed before this revision). Guarded on ConceptVideos/ConceptImages
    // both being empty — a condition true both for the very first insert's
    // placeholder shape and for the previous (2026-08-06) partial-gallery
    // revision, but never true again once this method has run — so it
    // fires exactly once and never overwrites real content edited after.
    // Same RemoveRange-then-rebuild shape as ReconcileLeJardinGalleryAndCatalogueAsync,
    // extended to every media collection this revision touches.
    private static async Task ReconcileNysaGoldMediaOverhaulAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .Include(p => p.FloorPlans).ThenInclude(f => f.Rooms)
            .Include(p => p.ConceptVideos)
            .Include(p => p.ConceptImages)
            .Include(p => p.SitePlanImages)
            .FirstOrDefaultAsync(p => p.Slug == "nysa-gold");

        if (project is null || project.ConceptVideos.Count > 0 || project.ConceptImages.Count > 0)
        {
            return;
        }

        context.ProjectImages.RemoveRange(project.Images);
        context.FloorPlanRooms.RemoveRange(project.FloorPlans.SelectMany(f => f.Rooms));
        context.FloorPlans.RemoveRange(project.FloorPlans);
        context.ProjectSitePlanImages.RemoveRange(project.SitePlanImages);

        foreach (var image in BuildNysaGoldImages())
        {
            context.ProjectImages.Add(new ProjectImage
            {
                ProjectId = project.Id,
                ImagePath = image.ImagePath,
                VideoPath = image.VideoPath,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                Category = image.Category,
                Block = image.Block,
                ApartmentType = image.ApartmentType
            });
        }

        foreach (var floorPlan in BuildNysaGoldFloorPlans())
        {
            floorPlan.ProjectId = project.Id;
            context.FloorPlans.Add(floorPlan);
        }

        foreach (var video in BuildNysaGoldConceptVideos())
        {
            video.ProjectId = project.Id;
            context.ProjectConceptVideos.Add(video);
        }

        foreach (var image in BuildNysaGoldConceptImages())
        {
            image.ProjectId = project.Id;
            context.ProjectConceptImages.Add(image);
        }

        foreach (var sitePlanImage in BuildNysaGoldSitePlanImages())
        {
            sitePlanImage.ProjectId = project.Id;
            context.ProjectSitePlanImages.Add(sitePlanImage);
        }

        project.LocationImagePath = "/images/projects/nysa-gold/location.webp";

        await context.SaveChangesAsync();
    }

    // Not a seed — Nysa Gold Floor Plans follow-up (2026-08-10): adds the
    // Bodrum Kat drawing the client converted from PDF to JPG themselves,
    // and backfills the same temporary placeholder Net/Gross/Sales-Gross
    // stats + room list every other project's real-drawing floor plans
    // already carry (BuildPlaceholderFloorPlans's own set) onto all 9
    // floors, replacing the previously blank stats row. Guarded on
    // FloorPlans.Count == 8 — the exact shape ReconcileNysaGoldMediaOverhaulAsync
    // above left behind (8 floors, no stats) — so this runs exactly once and
    // never overwrites real per-floor figures entered after this ran. Same
    // RemoveRange-then-rebuild shape as ReconcileLaFioreKarabag2EtapFloorPlansAsync.
    private static async Task ReconcileNysaGoldFloorPlansAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.FloorPlans).ThenInclude(f => f.Rooms)
            .FirstOrDefaultAsync(p => p.Slug == "nysa-gold");

        if (project is null || project.FloorPlans.Count != 8)
        {
            return;
        }

        context.FloorPlanRooms.RemoveRange(project.FloorPlans.SelectMany(f => f.Rooms));
        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var floorPlan in BuildNysaGoldFloorPlans())
        {
            floorPlan.ProjectId = project.Id;
            context.FloorPlans.Add(floorPlan);
        }

        await context.SaveChangesAsync();
    }

    // Not a seed — Nysa Gold Residence "Aydın'ın İlk Pet Parkı" Konsept
    // addition (2026-08-20 client request): appends a 6th Konsept slide
    // (client-supplied photo) after the 5 ReconcileNysaGoldMediaOverhaulAsync
    // above already seeded (1 video + 4 images, DisplayOrder 1-5). Guarded
    // on the image's own path so this only ever inserts once.
    private static async Task ReconcileNysaGoldPetParkConceptAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "nysa-gold");

        if (project is null || project.ConceptImages.Any(i => i.ImagePath.Contains("pet-park")))
        {
            return;
        }

        context.ProjectConceptImages.Add(new ProjectConceptImage
        {
            ProjectId = project.Id,
            ImagePath = "/images/projects/nysa-gold/konsept/nysa-gold-pet-park.jpg",
            Eyebrow = "Evcil Dostlarla Yaşam",
            Title = "Aydın'ın İlk Pet Parkı",
            Description = "Aydın'da bir ilke imza atan Nysa Gold Residence, evcil dostlarınızla özgürce vakit geçirebileceğiniz özel bir pet parkına sahip. Bu ayrıcalıklı alan, sakinlerine hem konforlu hem de evcil dostu, modern bir yaşam deneyimi sunuyor.",
            DisplayOrder = 6
        });

        await context.SaveChangesAsync();
    }

    // Not a seed — replaces an already-seeded Le Jardin row's single generic
    // placeholder Image and two generic "1+1"/"2+1" placeholder FloorPlans
    // with the real 80-image Gallery and 2-entry Floor Plans set above —
    // same shape as ReconcileKuyuluLaViaVillalarFloorPlansAsync/
    // ReconcileLaFioreKarabag2EtapFloorPlansAsync above. Guarded to only run
    // once (Images.Count == 1, the untouched single seeded placeholder), so
    // it never overwrites real content edited after this ran. The real
    // catalogue PDF itself was copied to wwwroot/documents/catalogues/
    // le-jardin-katalog.pdf, replacing the generated placeholder at the same
    // CataloguePath already seeded — no path change needed here. Le Jardin's
    // Catalogue section renders through the same _ProjectCatalogue partial
    // as every other project (Catalogue standardization, 2026-08-06).
    private static async Task ReconcileLeJardinGalleryAndCatalogueAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .Include(p => p.FloorPlans)
            .FirstOrDefaultAsync(p => p.Slug == "le-jardin");

        if (project is null || project.Images.Count != 1)
        {
            return;
        }

        context.ProjectImages.RemoveRange(project.Images);
        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var image in BuildLeJardinImages())
        {
            context.ProjectImages.Add(new ProjectImage
            {
                ProjectId = project.Id,
                ImagePath = image.ImagePath,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                Category = image.Category,
                Block = image.Block
            });
        }

        foreach (var floorPlan in BuildLeJardinFloorPlans())
        {
            context.FloorPlans.Add(new FloorPlan
            {
                ProjectId = project.Id,
                ApartmentType = floorPlan.ApartmentType,
                ImagePath = floorPlan.ImagePath,
                NetAreaM2 = floorPlan.NetAreaM2,
                GrossAreaM2 = floorPlan.GrossAreaM2,
                SalesGrossAreaM2 = floorPlan.SalesGrossAreaM2,
                DisplayOrder = floorPlan.DisplayOrder,
                Rooms = floorPlan.Rooms
                    .Select(r => new FloorPlanRoom { Name = r.Name, AreaM2 = r.AreaM2, DisplayOrder = r.DisplayOrder })
                    .ToList()
            });
        }

        await context.SaveChangesAsync();
    }

    // Backfills Le Jardin's Concept carousel + Hero subtitle + Site Plan onto
    // an already-seeded row (Le Jardin Concept carousel generalization,
    // 2026-08-09; Site Plan added in the same-day Hero Banner Vaziyet Planı
    // revision) — same "reconcile an existing row rather than only seeding
    // fresh databases" shape as ReconcileLaFioreKarabag2EtapVaziyetPlaniVeKonseptAsync
    // below (which backfills that project's own SitePlanImages/ConceptImages
    // together the same way). Only touches ShortDescription while it's still
    // the original placeholder string, so re-running this after a real
    // subtitle has been approved and entered some other way never clobbers it.
    private static async Task ReconcileLeJardinConceptCarouselAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.ConceptVideos)
            .Include(p => p.ConceptImages)
            .Include(p => p.SitePlanImages)
            .FirstOrDefaultAsync(p => p.Slug == "le-jardin");

        if (project is null)
        {
            return;
        }

        var changed = false;

        if (project.ConceptVideos.Count == 0)
        {
            foreach (var conceptVideo in BuildLeJardinConceptVideos())
            {
                context.ProjectConceptVideos.Add(new ProjectConceptVideo
                {
                    ProjectId = project.Id,
                    VideoPath = conceptVideo.VideoPath,
                    PosterPath = conceptVideo.PosterPath,
                    Eyebrow = conceptVideo.Eyebrow,
                    Title = conceptVideo.Title,
                    Description = conceptVideo.Description,
                    DisplayOrder = conceptVideo.DisplayOrder
                });
            }

            changed = true;
        }

        if (project.ConceptImages.Count == 0)
        {
            foreach (var conceptImage in BuildLeJardinConceptImages())
            {
                context.ProjectConceptImages.Add(new ProjectConceptImage
                {
                    ProjectId = project.Id,
                    ImagePath = conceptImage.ImagePath,
                    Eyebrow = conceptImage.Eyebrow,
                    Title = conceptImage.Title,
                    Description = conceptImage.Description,
                    DisplayOrder = conceptImage.DisplayOrder
                });
            }

            changed = true;
        }

        if (project.SitePlanImages.Count == 0)
        {
            foreach (var sitePlanImage in BuildLeJardinSitePlanImages())
            {
                context.ProjectSitePlanImages.Add(new ProjectSitePlanImage
                {
                    ProjectId = project.Id,
                    ImagePath = sitePlanImage.ImagePath,
                    AltText = sitePlanImage.AltText,
                    DisplayOrder = sitePlanImage.DisplayOrder
                });
            }

            changed = true;
        }

        if (project.ShortDescription == "Placeholder short description for the Le Jardin development.")
        {
            project.ShortDescription = "Akdeniz esintili bahçesi ve havuzuyla, modern mimariyle bütünleşen ayrıcalıklı bir yaşam alanı.";
            changed = true;
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedProjectsAsync(AppDbContext context)
    {
        await ReconcileConfirmedProjectNamesAsync(context);
        await ReconcileProjectMetadataAsync(context);
        await ReconcileProjectPlaceholderCopyAsync(context);
        await ReconcileDavutlarDLatisRenameAsync(context);
        await ReconcileDavutlarDLatisCoverImageAsync(context);
        await ReconcileDavutlarDLatisMediaAsync(context);
        await ReconcileFerhundeHanimAptCoverImageAsync(context);
        await ReconcileFerhundeHanimAptRevisionAsync(context);
        await ReconcileLaViaAvmRenameAsync(context);
        await ReconcileLaFioreKarabag2EtapFloorPlansAsync(context);
        await ReconcileLaviaKuyuluToLaViaVillalarRenameAsync(context);
        await ReconcileKuyuluLaViaVillalarFloorPlansAsync(context);
        await ReconcileKuyuluLaViaVillalarVaziyetPlaniCatiKatiVeKonseptAsync(context);
        await ReconcileLeJardinGalleryAndCatalogueAsync(context);
        await ReconcileLeJardinConceptCarouselAsync(context);
        await ReconcileNysaGoldMediaOverhaulAsync(context);
        await ReconcileNysaGoldFloorPlansAsync(context);
        await ReconcileNysaGoldPetParkConceptAsync(context);
        await ReconcileLaFioreKarabag2EtapVaziyetPlaniVeKonseptAsync(context);
        await ReconcileLaFioreKarabag2EtapTumDisMekanGorselleriAsync(context);
        await ReconcileKuyuluAvmVaziyetPlaniPlanlarVeKonseptAsync(context);
        await ReconcileAlindaGoldResidenceRevisionAsync(context);
        await ReconcileMagnesiaGoldResidenceRevisionAsync(context);
        await ReconcileTrallesGoldResidenceRevisionAsync(context);
        await ReconcileNlatisRevisionAsync(context);
        await ReconcileLaFioreKarabagRevisionAsync(context);
        await ReconcileDavutlarDLatisRemoveSitePlanAsync(context);
        await ReconcileFerhundeHanimAptRemoveCatalogueAsync(context);
        await ReconcileLaFioreKarabag2EtapRemoveCatalogueAsync(context);
        await ReconcileKuyuluAvmUnpublishAsync(context);
        await ReconcileRemoveCatalogueAndSitePlanAsync(context, "alinda-gold");
        await ReconcileRemoveCatalogueAndSitePlanAsync(context, "magnesia-gold");
        await ReconcileRemoveCatalogueAndSitePlanAsync(context, "tralles-gold");
        await ReconcileRemoveCatalogueAndSitePlanAsync(context, "nlatis");
        await ReconcileRemoveCatalogueAndSitePlanAsync(context, "la-fiore-karabag");
        await ReconcileQLatisRemoveSitePlanAsync(context);
        await ReconcileNearbyPlacesResearchAsync(context);
        await ReconcileGalleryCurationAsync(context);

        var existingSlugs = new HashSet<string>(await context.Projects.Select(p => p.Slug).ToListAsync());

        var now = DateTime.UtcNow;

        // Asset paths below follow 07_AssetStructure.md's naming convention
        // but the image files themselves do not exist yet (wwwroot/images/projects
        // only holds a .gitkeep) — expected to 404 until real media is added.
        // CataloguePath is the one exception: ProjectsController.Details
        // existence-checks it (FileExistsInWebRoot) before exposing
        // CatalogueUrl, so every project below gets a generated placeholder
        // PDF actually present under wwwroot/documents/catalogues/ — a 404'd
        // download link would be a broken CTA, unlike a 404'd <img>, which
        // degrades silently. See docs/14_Decisions.md.
        var projects = new List<Project>
        {
            new()
            {
                Name = "Nysa Gold Residence",
                Slug = "nysa-gold",
                ShortDescription = "Zarif cepheleri, yüzme havuzu ve özenle tasarlanmış sosyal alanlarıyla Aydın Efeler'de ayrıcalıklı bir yaşam sunan modern rezidans.",
                Description = "Aydın Efeler'in gelişen bölgesinde yükselen Nysa Gold Residence, modern mimariyi konforlu ve güvenli bir yaşamla buluşturuyor. Yüzme havuzu, basketbol sahası, çocuk oyun alanı ve peyzajlı bahçeleriyle proje, her yaştan sakinine günün her saatinde keyifle vakit geçirebileceği sosyal alanlar sunuyor. Geniş balkonları ve ferah iç mekânlarıyla her daire, doğal ışığı içeri taşıyacak şekilde tasarlandı.",
                Status = ProjectStatus.Ongoing,
                // PLACEHOLDER LOCATION DATA — every project below is assigned to one
                // of the three real Projects-page filter districts (Aydın - Efeler /
                // Didim / Kuşadası) as a reasonable stand-in, since the previously
                // seeded "Aydın, Türkiye" value was identical for every project and
                // carried no district-level detail. Replace with the client's
                // verified project locations before production; the filter dropdown
                // reads these values directly (ProjectsController.cs), so updating a
                // project's Location here is the only change needed later.
                Location = "Aydın Efeler",
                ProjectType = "Residence",
                CompletionDate = null,
                CoverImage = "/images/projects/nysa-gold/cover.webp",
                // Real catalogue (2026-07-31, Project Detail redesign) — the PDF
                // supplied in ProjectAssets/Projects/nysa-gold/nysa-gold-katalog.pdf,
                // copied as-is to wwwroot/documents/catalogues/ per
                // 07_AssetStructure.md's convention. ~44MB uncompressed; flagged
                // for compression (e.g. Ghostscript) before production launch —
                // no PDF tooling available in this environment to do it here.
                CataloguePath = "/documents/catalogues/nysa-gold-katalog.pdf",
                // "Vaziyet Planı" Hero button asset, revision 2 (client asset
                // reorganization, 2026-08-09) — see BuildNysaGoldSitePlanImages.
                SitePlanImages = BuildNysaGoldSitePlanImages(),
                LocationImagePath = "/images/projects/nysa-gold/location.webp",
                // Demo/placeholder copy (facility names only — verified against
                // what the real renders below actually show: pool, basketball
                // court, children's playground, landscaped gardens, parking).
                // Wording is generic and meant to be replaced with approved
                // marketing copy before launch; the facilities themselves are
                // not invented. "Aydın'ın İlk Pet Parkı" appended verbatim per
                // client request (2026-08-20).
                Amenities = "Yüzme Havuzu\nBasketbol Sahası\nÇocuk Oyun Alanı\nPeyzaj Bahçeleri\nOtopark\nAydın'ın İlk Pet Parkı",
                DisplayOrder = 1,
                IsFeatured = true,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                // Real Exterior/Interior/Sales Office gallery, revision 2
                // (client asset reorganization, 2026-08-09) — see
                // BuildNysaGoldImages. Seeded directly here (not just in the
                // reconcile) so a genuinely empty database gets the real
                // content immediately rather than a one-run-behind
                // placeholder.
                Images = BuildNysaGoldImages(),
                // Real per-floor architectural drawings, revision 2 (client
                // asset reorganization, 2026-08-09) — see
                // BuildNysaGoldFloorPlans.
                FloorPlans = BuildNysaGoldFloorPlans(),
                ConceptVideos = BuildNysaGoldConceptVideos(),
                ConceptImages = BuildNysaGoldConceptImages(),
                Partners = new List<Partner>
                {
                    new() { Name = "Partner 01", Logo = "/images/partners/partner-01.webp", DisplayOrder = 1 }
                },
                // Placeholder distances (2026-07-31, Project Detail redesign) —
                // real nearby-place distances were not supplied; values are
                // clearly marked so they are obviously not final copy. Replace
                // with confirmed distances before launch.
                NearbyPlaces = new List<ProjectNearbyPlace>
                {
                    new() { Name = "Yer Tutucu — Şehir Merkezi", Distance = "5 km", DisplayOrder = 1 },
                    new() { Name = "Yer Tutucu — Havalimanı", Distance = "30 km", DisplayOrder = 2 },
                    new() { Name = "Yer Tutucu — Alışveriş Merkezi", Distance = "3 km", DisplayOrder = 3 },
                    new() { Name = "Yer Tutucu — Sahil", Distance = "8 km", DisplayOrder = 4 }
                }
            },
            new()
            {
                Name = "Le Jardin",
                Slug = "le-jardin",
                // Hero subtitle (Le Jardin Concept carousel generalization,
                // 2026-08-09) — adapted from the client-supplied "Le Jardin"
                // room-by-room presentation text's recurring Akdeniz/bahçe/
                // havuz framing (Giriş, Teras-Havuz-Bahçe sections), not
                // invented copy. Description below is still the pre-existing
                // placeholder — no full-length project description was
                // supplied, only the room-by-room presentation text used for
                // the Concept slides.
                ShortDescription = "Akdeniz esintili bahçesi ve havuzuyla, modern mimariyle bütünleşen ayrıcalıklı bir yaşam alanı.",
                Description = "Akdeniz'in sıcak karakterini modern mimariyle buluşturan Le Jardin, begonvillerle sarılı girişi ve açık renk taş kaplamalı cephesiyle sakinlerini davetkâr bir atmosferle karşılıyor. Özel havuzu, gölgelikli terası ve geniş bahçesiyle proje, iç mekânın konforunu dış mekânın huzuruyla bütünleştiriyor. Aydın Efeler'de konumlanan Le Jardin, hem günlük yaşamın hem de misafirlerinizi ağırlamanın keyfini çıkarabileceğiniz ayrıcalıklı bir villa deneyimi sunuyor.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın Efeler",
                ProjectType = "Villa",
                CompletionDate = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                CoverImage = "/images/projects/le-jardin/cover.webp",
                // Placeholder catalogue (Hero Banner Catalogue Availability Audit,
                // 2026-08-01) — no real PDF supplied yet, so this generated
                // stand-in lives at ProjectAssets/Projects/le-jardin/, copied
                // as-is to wwwroot/documents/catalogues/ same as Nysa Gold's
                // real one. Replace both the file and this path once the
                // client supplies real catalogue artwork — see docs/14_Decisions.md.
                CataloguePath = "/documents/catalogues/le-jardin-katalog.pdf",
                DisplayOrder = 2,
                IsFeatured = true,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                Images = new List<ProjectImage>
                {
                    new() { ImagePath = "/images/projects/le-jardin/gallery-01.webp", AltText = "Le Jardin placeholder gallery image 1", DisplayOrder = 1 }
                },
                // Concept carousel (Le Jardin Concept carousel generalization,
                // 2026-08-09) — 2 concept videos followed by 3 Exterior
                // gallery photos, merged into one mixed-media carousel by
                // ProjectsController.Details (see ConceptSlideModel). The
                // referenced Exterior photos exist at gallery/exterior/
                // originals/ regardless of Images above still pointing at the
                // placeholder gallery-01.webp — the two are independent.
                ConceptVideos = BuildLeJardinConceptVideos(),
                ConceptImages = BuildLeJardinConceptImages(),
                SitePlanImages = BuildLeJardinSitePlanImages(),
                // Placeholder apartment-type figures (2026-07-31, Floor Plans
                // redesign) — areas and room breakdown are not confirmed
                // project data. ImagePath keeps pointing at a file that does
                // not exist yet; the section shows a placeholder graphic
                // instead of that image until real drawings are supplied.
                FloorPlans = new List<FloorPlan>
                {
                    new()
                    {
                        ApartmentType = "1+1",
                        ImagePath = "/images/projects/le-jardin/floorplan-1-1.webp",
                        NetAreaM2 = 45.00m,
                        GrossAreaM2 = 68.50m,
                        SalesGrossAreaM2 = 55.00m,
                        DisplayOrder = 1,
                        Rooms = new List<FloorPlanRoom>
                        {
                            new() { Name = "Salon", AreaM2 = 22.00m, DisplayOrder = 1 },
                            new() { Name = "Mutfak", AreaM2 = 8.50m, DisplayOrder = 2 },
                            new() { Name = "Yatak Odası", AreaM2 = 12.00m, DisplayOrder = 3 },
                            new() { Name = "Banyo", AreaM2 = 5.50m, DisplayOrder = 4 },
                            new() { Name = "Balkon", AreaM2 = 6.00m, DisplayOrder = 5 }
                        }
                    },
                    new()
                    {
                        ApartmentType = "2+1",
                        ImagePath = "/images/projects/le-jardin/floorplan-2-1.webp",
                        NetAreaM2 = 68.00m,
                        GrossAreaM2 = 95.00m,
                        SalesGrossAreaM2 = 78.00m,
                        DisplayOrder = 2,
                        Rooms = new List<FloorPlanRoom>
                        {
                            new() { Name = "Salon", AreaM2 = 24.00m, DisplayOrder = 1 },
                            new() { Name = "Mutfak", AreaM2 = 9.50m, DisplayOrder = 2 },
                            new() { Name = "Yatak Odası 1", AreaM2 = 14.00m, DisplayOrder = 3 },
                            new() { Name = "Yatak Odası 2", AreaM2 = 11.00m, DisplayOrder = 4 },
                            new() { Name = "Banyo", AreaM2 = 6.00m, DisplayOrder = 5 },
                            new() { Name = "Balkon", AreaM2 = 7.50m, DisplayOrder = 6 }
                        }
                    }
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
                ShortDescription = "Beyaz cepheleri ve dikey kırmızı vurgularıyla Aydın Efeler'in siluetinde öne çıkan, güçlü bir mimari kimliğe sahip tamamlanmış rezidans.",
                Description = "Aydın Efeler'in dokusu içinde yükselen Tralles Gold Residence, beyaz cepheleri ve dikey kırmızı vurgularıyla çevresinden hemen ayrışan güçlü bir mimari kimlik taşıyor. Akşam saatlerinde ışıklandırılan cephesiyle şehrin gece silüetinde kendine özgü bir karakter kazanan proje, gündüzün sadeliğini gecenin canlılığıyla buluşturuyor. Tamamlanmış bu proje, Ançın İnşaat'ın zanaatkârlık anlayışını yansıtan kalıcı bir yaşam alanı olarak sakinlerine sunuluyor.",
                Status = ProjectStatus.Completed,
                Location = "Aydın Efeler",
                ProjectType = "Residence",
                CompletionDate = null,
                CoverImage = "/images/projects/tralles-gold/cover.webp",
                // Placeholder catalogue — see Le Jardin above.
                CataloguePath = "/documents/catalogues/tralles-gold-katalog.pdf",
                DisplayOrder = 3,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                Images = new List<ProjectImage>
                {
                    new() { ImagePath = "/images/projects/tralles-gold/gallery-01.webp", AltText = "Tralles Gold placeholder gallery image 1", DisplayOrder = 1 }
                },
                // Placeholder apartment-type figures — see Le Jardin above.
                FloorPlans = new List<FloorPlan>
                {
                    new()
                    {
                        ApartmentType = "3+1",
                        ImagePath = "/images/projects/tralles-gold/floorplan-3-1.webp",
                        NetAreaM2 = 95.00m,
                        GrossAreaM2 = 128.00m,
                        SalesGrossAreaM2 = 108.00m,
                        DisplayOrder = 1,
                        Rooms = new List<FloorPlanRoom>
                        {
                            new() { Name = "Salon", AreaM2 = 28.00m, DisplayOrder = 1 },
                            new() { Name = "Mutfak", AreaM2 = 11.00m, DisplayOrder = 2 },
                            new() { Name = "Yatak Odası 1", AreaM2 = 15.00m, DisplayOrder = 3 },
                            new() { Name = "Yatak Odası 2", AreaM2 = 13.00m, DisplayOrder = 4 },
                            new() { Name = "Yatak Odası 3", AreaM2 = 12.00m, DisplayOrder = 5 },
                            new() { Name = "Banyo", AreaM2 = 7.00m, DisplayOrder = 6 },
                            new() { Name = "Balkon", AreaM2 = 9.00m, DisplayOrder = 7 }
                        }
                    }
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
            // Images/Partners are intentionally left empty — no detail-page
            // content has been requested or supplied yet. FloorPlans (Floor
            // Plans Availability Audit, 2026-08-01) get the same placeholder
            // "2+1" entry as Nysa Gold/Le Jardin above, so every project's
            // Daire Planları section renders consistently — see
            // docs/14_Decisions.md.
            new()
            {
                Name = "Nlatis",
                Slug = "nlatis",
                ShortDescription = "Kıvrımlı ahşap tonlu çatı hattı ve cam kaplı cephesiyle özgün bir mimari kimliğe sahip, İzmir'de tamamlanmış rezidans projesi.",
                Description = "İzmir'de tamamlanan Nlatis, kıvrımlı ahşap tonlu çatı hattı ve cam kaplı cephesiyle özgün bir mimari kimlik taşıyor. Koyu renkli cephe panelleri ve dikey ahşap vurgularının oluşturduğu katmanlı ritim, cam balkonlarla desteklenerek modern ve sade bir görünüm sunuyor. Zemin kattaki sosyal kullanım alanları, binayı çevresiyle buluşturan canlı bir karşılama noktası oluşturuyor.",
                Status = ProjectStatus.Completed,
                Location = "İzmir",
                ProjectType = "Residence",
                CompletionDate = null,
                CoverImage = "/images/projects/nlatis/cover.webp",
                // Placeholder catalogue — see Le Jardin above.
                CataloguePath = "/documents/catalogues/nlatis-katalog.pdf",
                DisplayOrder = 4,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                FloorPlans = BuildPlaceholderFloorPlans("nlatis")
            },
            new()
            {
                Name = "Alinda Gold Residence",
                Slug = "alinda-gold",
                ShortDescription = "Akıcı hatları ve zarif çatı aydınlatmasıyla Aydın Efeler'in siluetine yeni bir karakter katan, tamamlanmış modern rezidans.",
                Description = "Aydın Efeler'in ufkunda yükselen Alinda Gold Residence, akıcı hatları ve zarif çatı aydınlatmasıyla şehrin siluetine yeni bir karakter katıyor. Bloklar arasına özenle yerleştirilen havuz, yürüyüş yolları ve peyzaj alanları, sakinlerine güne açık havada başlama ve günü dinginlikle bitirme imkânı sunuyor. Özenli aydınlatma tasarımı ve ferah kompozisyonuyla proje girişi, Ançın İnşaat imzasının premium yaklaşımını en baştan hissettiriyor.",
                Status = ProjectStatus.Completed,
                Location = "Aydın Efeler",
                ProjectType = "Residence",
                CompletionDate = null,
                CoverImage = "/images/projects/alinda-gold/cover.webp",
                // Placeholder catalogue — see Le Jardin above.
                CataloguePath = "/documents/catalogues/alinda-gold-katalog.pdf",
                DisplayOrder = 5,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                FloorPlans = BuildPlaceholderFloorPlans("alinda-gold")
            },
            new()
            {
                Name = "Magnesia Gold Residence",
                Slug = "magnesia-gold",
                ShortDescription = "Geniş peyzaj alanları ve özenle tasarlanmış ortak yaşam alanlarıyla Aydın Efeler'de bir aradalığı ön plana çıkaran tamamlanmış rezidans.",
                Description = "Aydın'ın ufkunda yan yana yükselen Magnesia Gold Residence blokları, geniş peyzaj alanları ve özenle tasarlanmış ortak yaşam alanlarıyla bir aradalığı ön plana çıkarıyor. Palmiyelerle çevrili yürüyüş yolları, havuzlar ve geniş çim alanları günün her saatinde huzurlu bir mola sunarken, akşam saatlerinde özenli aydınlatmayla öne çıkan havuz çevresi sosyalleşmek isteyen sakinler için davetkâr bir buluşma noktasına dönüşüyor. Tamamlanan bu proje, yeşille mimarinin uyumunu günlük yaşamın merkezine taşıyor.",
                Status = ProjectStatus.Completed,
                Location = "Aydın Efeler",
                ProjectType = "Residence",
                CompletionDate = null,
                CoverImage = "/images/projects/magnesia-gold/cover.webp",
                // Placeholder catalogue — see Le Jardin above.
                CataloguePath = "/documents/catalogues/magnesia-gold-katalog.pdf",
                DisplayOrder = 6,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                FloorPlans = BuildPlaceholderFloorPlans("magnesia-gold")
            },
            new()
            {
                Name = "La Fiore Karabağ",
                Slug = "la-fiore-karabag",
                ShortDescription = "Gür çam ormanının içine özenle yerleştirilmiş tek katlı villalarıyla Aydın İncirliova'da sakin ve mahrem bir yaşam sunan tamamlanmış proje.",
                Description = "Aydın İncirliova'da gür çam ormanının içine özenle yerleştirilen La Fiore Karabağ, tek katlı villalarıyla sakin ve mahremiyeti önceleyen bir yaşam alanı sunuyor. Yerleşim içindeki kesintisiz yürüyüş yolları ve peyzaj düzenlemesi doğayla iç içe bir günlük yaşam deneyimi vaat ederken, villa bahçelerindeki pergola altı oturma grupları ve şömineler iç mekânı bahçeyle bütünleştiriyor. Taş kaplı cephesi ve ahşap detaylarıyla dikkat çeken proje girişi, güvenlikli giriş noktasıyla sakinlerinin huzurunu güvence altına alıyor.",
                Status = ProjectStatus.Completed,
                Location = "Aydın İncirliova",
                ProjectType = "Villa",
                CompletionDate = null,
                CoverImage = "/images/projects/la-fiore-karabag/cover.webp",
                // No Proje Kataloğu for this project (client request,
                // 2026-08-20) — no real catalogue exists, and the previous
                // placeholder CataloguePath below was being resurrected on
                // every startup by ReconcileMissingCataloguePathsAsync,
                // which backfills any project whose DB row has a null
                // CataloguePath from this very list — undoing
                // ReconcileRemoveCatalogueAndSitePlanAsync's removal above.
                // Leaving CataloguePath unset here (same as La Fiore Karabağ
                // 2. Etap below) fixes that at the source: the row is no
                // longer in ReconcileMissingCataloguePathsAsync's backfill
                // map, so the removal sticks.
                DisplayOrder = 7,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                FloorPlans = BuildPlaceholderFloorPlans("la-fiore-karabag")
            },
            new()
            {
                Name = "La Fiore Karabağ 2. Etap",
                Slug = "la-fiore-karabag-2-etap",
                ShortDescription = "Yüzlerce yıllık zeytin ağaçları arasında konumlanan, doğayla bütünleşen villa yerleşimiyle Aydın İncirliova'da yükselen ayrıcalıklı bir proje.",
                Description = "Aydın Karabağ'ın yüzlerce yıllık zeytin ağaçları arasında konumlanan La Fiore Karabağ 2. Etap, arazinin doğal eğimini ve dokusunu koruyan bir yerleşim planıyla hayat buluyor. Geniş yürüyüş yolları, peyzajlı meydanlar ve ortak sosyal alanlarla örülen proje, sakinlerini binalardan çok bir arada yaşama davet ediyor. Taş duvarları, zarif aydınlatması ve zeytin ağaçlarıyla çevrili villalarıyla La Fiore Karabağ 2. Etap, hem huzurlu bir yaşamı hem de Karabağ'ın gelişen değeriyle güçlü bir yatırım fırsatını bir araya getiriyor.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın İncirliova",
                ProjectType = "Villa",
                CompletionDate = null,
                // Dedicated cover.webp generated via ThumbnailTool's --single
                // mode (1000w/88q) from dis-mekan-gorselleri/a-tipi-blok/
                // 2a.jpeg — same client-selected source as banner.webp below
                // — see Davutlar D Latis's CoverImage above for why a
                // dedicated file replaces the gallery-original path.
                CoverImage = "/images/projects/la-fiore-karabag-2-etap/cover.webp",
                // No Proje Kataloğu for this project (Project Asset Audit,
                // 2026-08-17) — no real catalogue exists, and the client
                // asked for the button/section to be removed entirely
                // rather than shown as "coming soon" (previous behavior via
                // CatalogueComingSoon, see ReconcileLaFioreKarabag2EtapRemoveCatalogueAsync
                // for the same fix applied to an already-seeded row).
                DisplayOrder = 8,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                // Gallery pilot (2026-08-06) — see BuildLaFioreKarabag2EtapImages/
                // BuildLaFioreKarabag2EtapFloorPlans above for what's real vs
                // still placeholder. Vaziyet Planı/Concept/Gallery phase
                // (2026-08-09) added SitePlanImages/ConceptImages and a new
                // "All Exterior" gallery category folded into
                // BuildLaFioreKarabag2EtapImages.
                Images = BuildLaFioreKarabag2EtapImages(),
                FloorPlans = BuildLaFioreKarabag2EtapFloorPlans(),
                SitePlanImages = BuildLaFioreKarabag2EtapSitePlanImages(),
                ConceptImages = BuildLaFioreKarabag2EtapConceptImages()
            },
            new()
            {
                // Renamed from "Lavia Kuyulu" / "lavia-kuyulu" (2026-08-06)
                // once the client's real project name and asset folder
                // ("La Via Villalar 1. Etap" /
                // kuyulu-la-via-villalar-birinci-etap) were delivered — see
                // ReconcileLaviaKuyuluToLaViaVillalarRenameAsync, which
                // migrates any already-seeded row the same way
                // ReconcileDavutlarDLatisRenameAsync does, so existing
                // databases pick up the rename (and the real Images/
                // FloorPlans below, via ReconcileMissingImagesAsync and a
                // dedicated FloorPlans swap) without a duplicate row.
                Name = "La Via Villalar 1. Etap",
                Slug = "kuyulu-la-via-villalar-birinci-etap",
                ShortDescription = "Kendi bahçesi ve garajıyla bağımsız villa mimarisini Kuyulu'nun sakin dokusuyla buluşturan ayrıcalıklı bir yaşam alanı.",
                Description = "La Via Villalar 1. Etap, her biri kendi bahçesi ve garajıyla bağımsız bir villa mimarisini Kuyulu'nun sakin dokusuyla buluşturuyor. Ahşap dokulu cepheler, zarif aydınlatmalar ve modern çizgiler, Ançın İnşaat imzasının kalite anlayışını her villada yeniden tanımlıyor. Her villanın kendi çatı terası, özel havuzu ve peyzajlı bahçesiyle tasarlanan proje, sosyal yaşamı ailenizin kendi mahremiyetine taşıyarak hem huzurlu bir yaşamı hem de Kuyulu'nun gelişen değeriyle güçlü bir yatırım fırsatını bir araya getiriyor.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın Kuyulu",
                ProjectType = "Villa",
                CompletionDate = null,
                CoverImage = "/images/projects/kuyulu-la-via-villalar-birinci-etap/cover.webp",
                // Placeholder catalogue — see Le Jardin above.
                CataloguePath = "/documents/catalogues/kuyulu-la-via-villalar-birinci-etap-katalog.pdf",
                DisplayOrder = 9,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                // Real Gallery photos and Floor Plan drawings (La Via
                // Villalar Gallery integration, 2026-08-06) — see
                // BuildKuyuluLaViaVillalarImages/FloorPlans above.
                Images = BuildKuyuluLaViaVillalarImages(),
                FloorPlans = BuildKuyuluLaViaVillalarFloorPlans(),
                // Vaziyet Planı/Çatı Katı/Katalog/Konsept revision
                // (2026-08-09) — see BuildKuyuluLaViaVillalarSitePlanImages/
                // BuildKuyuluLaViaVillalarConceptImages below.
                SitePlanImages = BuildKuyuluLaViaVillalarSitePlanImages(),
                ConceptImages = BuildKuyuluLaViaVillalarConceptImages()
            },
            new()
            {
                // Renamed from "Dlatis Thermal Wellness Residence" /
                // "dlatis-thermal-wellness" (2026-08-05) once the client's real
                // project name/folder ("Davutlar D Latis") was confirmed — see
                // ReconcileDavutlarDLatisRenameAsync, which migrates any
                // already-seeded row (Slug/Name/CoverImage/CataloguePath) so
                // existing databases pick up the rename without a duplicate row.
                Name = "Davutlar D Latis",
                Slug = "davutlar-d-latis",
                // Real marketing copy (2026-08-09, Davutlar D Latis Media
                // phase) — rewritten from the client's Turkish presentation
                // transcript, not copied verbatim. ShortDescription is a
                // single short sentence reused as the Hero subheading only;
                // ConceptDescription below is the deliberately richer,
                // separate paragraph the Concept section shows, per the
                // client's explicit "do not use the exact same text in both
                // places" instruction.
                ShortDescription = "Kuşadası'nın bakir doğasında, termal şifayla buluşan ayrıcalıklı bir yaşam alanı.",
                Description = "Kuşadası'nın binlerce yıllık tarihi ve doğal güzellikleriyle iç içe konumlanan Davutlar D Latis, bölgenin termal su kaynaklarını modern mimari bir yaşam deneyimiyle buluşturuyor. Spa, fitness ve sosyal donatılarıyla tamamlanan proje, hem gündelik yaşamın konforunu hem de Ege'nin gözde turizm merkezlerinden birinde uzun vadeli bir yatırım fırsatını bir arada sunuyor. Ançın İnşaat güvencesiyle hayata geçen bu proje, huzurlu bir yaşamı termal şifayla buluşturan ayrıcalıklı bir adres olmayı hedefliyor.",
                ConceptDescription = "Dilek Yarımadası Milli Parkı'nın eteğinde, Efes ve Meryem Ana Evi gibi kadim mirasların komşuluğunda yükselen Davutlar D Latis, Davutlar'ın eşsiz termal sularını modern mimariyle buluşturuyor. Spa, fitness ve sosyal yaşam alanlarıyla tamamlanan bu wellness konsepti, Ege'nin turizm başkentinde huzurlu bir yaşamı güçlü bir yatırım fırsatıyla bir araya getiriyor.",
                Status = ProjectStatus.Ongoing,
                Location = "Kuşadası",
                ProjectType = "Residence",
                CompletionDate = null,
                // Cover image (2026-08-06, Project Card Thumbnail Quality
                // fix): a dedicated cover.webp generated from the
                // client-selected exterior-07.jpeg via ThumbnailTool's
                // --single mode (1000w/88q, higher than the shared
                // gallery-thumbnail default of 800w/82q) — previously pointed
                // straight at the gallery original itself, whose auto-derived
                // thumbnail is the exact file the Gallery also shows, so
                // sharpening it for the card would have altered that Gallery
                // photo too. See ReconcileDavutlarDLatisCoverImageAsync for
                // the same fix applied to already-seeded rows.
                CoverImage = "/images/projects/davutlar-d-latis/cover.webp",
                // Placeholder catalogue — see Le Jardin above.
                CataloguePath = "/documents/catalogues/davutlar-d-latis-katalog.pdf",
                // Concept video (2026-08-09) — client-supplied
                // konsept-video/dlatis-sunum-web.mp4 (238MB, 8.8Mbps —
                // despite the filename, not actually web-optimized) was
                // re-encoded via ffmpeg (libx264 CRF 23, 2.2Mbps capped,
                // AAC 128k, +faststart) down to ~57MB at the same 1920x1080/
                // 215s, then copied to concept/video.mp4 alongside the
                // client's own poster.webp at concept/poster.webp. Raw drop
                // left in place under konsept-video/ as an archival copy,
                // same as every other project's raw-folder precedent.
                ConceptVideoPath = "/images/projects/davutlar-d-latis/concept/video.mp4",
                ConceptVideoPosterPath = "/images/projects/davutlar-d-latis/concept/poster.webp",
                // No Vaziyet Planı for this project (client revision,
                // 2026-08-17) — the kat-planlari/genel-planlar/ Zemin Kat
                // drawing previously used here is a single floor's plan, not
                // a true site/master plan, so the Hero's "Vaziyet Planı"
                // button should not display for this project. See
                // ReconcileDavutlarDLatisRemoveSitePlanAsync, which removes
                // any already-seeded SitePlanImages row the same way. Proje
                // Kataloğu/Daire Planları/Videolar are untouched by this
                // change.
                // Location photo (2026-08-09) — konum/d-latis-konum.png
                // converted to WebP via ThumbnailTool --single (1200w/88q),
                // replacing the shared location illustration for this
                // project only in the Location & Distances section.
                LocationImagePath = "/images/projects/davutlar-d-latis/location.webp",
                DisplayOrder = 10,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                // Real photos (2026-08-05, Davutlar D Latis Media phase) — every
                // file already supplied under wwwroot/images/projects/davutlar-d-latis
                // (dis-mekan-gorselleri → Exterior, ic-mekan-gorselleri → Interior
                // including its per-unit-type subfolders, sosyal-olanaklar →
                // Social Areas), none skipped, per the client's explicit
                // instruction. See ReconcileMissingImagesAsync for how this
                // backfills an already-seeded project with 0 images.
                Images = BuildDavutlarDLatisImages(),
                // Real architectural drawings (2026-08-09) — see
                // BuildDavutlarDLatisFloorPlans above for what's real vs
                // placeholder.
                FloorPlans = BuildDavutlarDLatisFloorPlans(),
                // Concept section's 5-slide video carousel (2026-08-09) —
                // see BuildDavutlarDLatisConceptVideos above.
                ConceptVideos = BuildDavutlarDLatisConceptVideos()
            },
            new()
            {
                // Real photos (2026-08-05, Ferhunde Hanım Apt. Media phase;
                // Gallery/Floor Plans/Vaziyet Planı/Concept revision,
                // 2026-08-10) — see BuildFerhundeHanimAptImages above for the
                // Exterior/Interior breakdown and block-chip wiring.
                Name = "Ferhunde Hanım Apt.",
                Slug = "ferhunde-hanim-apt",
                ShortDescription = "Yumuşak hatlı balkonları ve özenle seçilmiş cephe dokusuyla Aydın Efeler'de dikkat çeken, tamamlanmış bir apartman projesi.",
                Description = "Ferhunde Hanım Apt., yumuşak hatlı balkonları ve özenle seçilmiş cephe dokusuyla Aydın Efeler'de dikkat çeken bir mimari kimlik sunuyor. Geniş camları ve ferah balkonlarıyla her kat doğal ışığı içeri taşırken, yüksek çitlerle çevrili özel bahçe alanı sakinlerine şehrin gürültüsünden uzak, güvenli ve huzurlu bir dış mekân yaşamı sağlıyor. Çevre duvarları ve peyzajlı ön bahçesiyle tamamlanan proje, günlük yaşamı kolaylaştıran ve değerini koruyan bir yatırım fırsatı sunuyor.",
                Status = ProjectStatus.Completed,
                Location = "Aydın Efeler",
                ProjectType = "Apartman",
                CompletionDate = null,
                // Cover image (2026-08-06, Project Card Thumbnail Quality
                // fix): a dedicated cover.webp generated from the same
                // client-selected dis-mekan-gorselleri/1.jpg used as the
                // Detail Hero source, via ThumbnailTool's --single mode
                // (1000w/88q) — see Davutlar D Latis's CoverImage above for
                // why a dedicated file replaces the gallery-original path.
                // The Detail Hero itself (banner.webp, per ProjectsController's
                // {slug}/banner.webp convention) is untouched by this.
                CoverImage = "/images/projects/ferhunde-hanim-apt/cover.webp",
                // No Proje Kataloğu for this project (Project Asset Audit,
                // 2026-08-17) — no real catalogue exists, and the client
                // asked for the button/section to be removed entirely
                // rather than shown as "coming soon" (previous behavior via
                // CatalogueComingSoon, see ReconcileFerhundeHanimAptRemoveCatalogueAsync
                // for the same fix applied to an already-seeded row).
                // Location & Distances section photo (2026-08-10) — see
                // BuildFerhundeHanimAptSitePlanImages above for why this
                // shares its source photo with the site plan.
                LocationImagePath = "/images/projects/ferhunde-hanim-apt/location.webp",
                DisplayOrder = 11,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                Images = BuildFerhundeHanimAptImages(),
                FloorPlans = BuildFerhundeHanimAptFloorPlans(),
                SitePlanImages = BuildFerhundeHanimAptSitePlanImages(),
                ConceptImages = BuildFerhundeHanimAptConceptImages()
            },
            new()
            {
                // Real photos (2026-08-06, Kuyulu AVM Media phase) — every
                // file supplied under wwwroot/images/projects/kuyulu-avm,
                // renamed kuyulu-avm-01..15 in numeric order (the source
                // folder had no plain "1-3.jpg", and "4(2).jpg" is treated as
                // photo 4) and moved into gallery/originals per the image
                // architecture, none skipped, per the client's explicit
                // instruction. All 15 are tagged Category = "Exterior" (La
                // Via AVM update, 2026-08-09) — see BuildKuyuluAvmImages.
                //
                // Renamed from "Kuyulu AVM" to "La Via AVM" (2026-08-06,
                // client correction) — Slug/folder/image paths intentionally
                // left as kuyulu-avm/kuyulu-avm-NN per the client's explicit
                // instruction not to rename folders or image paths, only the
                // displayed name.
                Name = "La Via AVM",
                Slug = "kuyulu-avm",
                ShortDescription = "Gölgelikli teraslar ve özenle tasarlanmış peyzajıyla alışverişi bir yaşam deneyimine dönüştüren, Aydın Efeler'de yükselen ticari proje.",
                Description = "Aydın Efeler'de geniş bir zeytinlik dokusunun kenarında yükselen La Via AVM, alışverişi gündelik bir ihtiyaçtan çok bir yaşam deneyimine dönüştürüyor. Gölgelikli teraslar, geniş yürüyüş alanları ve özenle tasarlanmış peyzajıyla proje, Ançın İnşaat'ın imza kalite anlayışını ticari ölçekte yeniden yorumluyor. Doğal taş kaplamalar, ahşap lamel dokular ve gece boyunca yumuşak bir çizgiyle beliren aydınlatma, cepheye kalıcı ve zarif bir karakter kazandırıyor.",
                Status = ProjectStatus.Ongoing,
                Location = "Aydın - Efeler",
                ProjectType = "Commercial",
                CompletionDate = null,
                // Dedicated cover (2026-08-06, Project Card Thumbnail Quality
                // fix) — generated from the same client-selected photo
                // (kuyulu-avm-03.jpg) as a standalone higher-quality asset via
                // ThumbnailTool's --single mode (1000w/88q vs. the shared
                // gallery-thumbnail default of 800w/82q), same convention as
                // every other project's dedicated cover.webp. Previously
                // pointed straight at the gallery original, which meant its
                // auto-derived thumbnail was the exact same file shown in the
                // Gallery — regenerating it for card sharpness would have
                // altered that Gallery photo too. A dedicated file keeps the
                // two fully independent.
                CoverImage = "/images/projects/kuyulu-avm/cover.webp",
                // No real catalogue exists yet for this project (La Via AVM
                // update, 2026-08-09) — CataloguePath points at a placeholder
                // PDF purely so the existing FileExistsInWebRoot check
                // passes; CatalogueComingSoon suppresses the real download
                // exactly like La Fiore Karabağ 2. Etap, so the file's actual
                // content is never served. This is what makes the Hero's
                // Katalog button, the Project Catalogue CTA section and the
                // Floor Plans panel's button all appear, each showing the
                // shared "yakında eklenecektir" toast on click.
                CataloguePath = "/documents/catalogues/kuyulu-avm-katalog.pdf",
                CatalogueComingSoon = true,
                DisplayOrder = 12,
                IsFeatured = false,
                // Unpublished (Project Asset Audit, 2026-08-17) — removed
                // from the live site at the client's request using the
                // application's existing editorial gate (see
                // IProjectQueryService), without deleting the row, its
                // images or Git history. See
                // ReconcileKuyuluAvmUnpublishAsync for the same fix applied
                // to an already-seeded row.
                IsPublished = false,
                CreatedAt = now,
                UpdatedAt = now,
                Images = BuildKuyuluAvmImages(),
                SitePlanImages = BuildKuyuluAvmSitePlanImages(),
                FloorPlans = BuildKuyuluAvmFloorPlans(),
                ConceptImages = BuildKuyuluAvmConceptImages()
            },
            new()
            {
                // Real marketing copy grounded in the client-supplied assets
                // (the concept video's own title card "ANCIN / KUŞADASI
                // RESIDENCES", and the hillside "KUŞADASI" signage visible in
                // gallery/exterior/originals/5.png) — not invented. Full
                // Description kept as the standard placeholder (no long-form
                // copy supplied), same choice made for Davutlar D Latis.
                Name = "Hacıfeyzullah - Q-Latis",
                Slug = "q-latis",
                ShortDescription = "Kuşadası'nda modern mimarisi ve deniz manzaralı yaşam alanlarıyla öne çıkan ayrıcalıklı bir proje.",
                Description = "Kuşadası'nın eşsiz doğasıyla iç içe konumlanan Hacıfeyzullah - Q-Latis, ahşap dokulu cepheleri ve geniş camekânlarıyla bölgenin karakterine saygılı, çağdaş bir mimari dil sunuyor. Üst kat dairelerin geniş balkonlarından izlenen Ege Denizi manzarası her günü ayrıcalıklı kılarken, açık teras, fitness salonu ve aeroyoga/pilates stüdyosu gibi sosyal donatılar aktif bir yaşam tarzını destekliyor. Zeytinliklerle çevrili tepelik konumuyla proje, Kuşadası'nın doğal dokusunu koruyan bir yerleşim anlayışıyla tasarlandı.",
                Status = ProjectStatus.Ongoing,
                // Matches Davutlar D Latis's exact "Kuşadası" value (not a
                // new district string), grounded in the same location
                // evidence noted above.
                Location = "Kuşadası",
                ProjectType = "Residence",
                CompletionDate = null,
                // Dedicated cover/banner (2026-08-10) generated from the
                // client's cleanest full-elevation render
                // (gallery/exterior/originals/2.png — no watermark/diagram/
                // signage overlay) via ThumbnailTool --single, same
                // convention as every other project (1000w/88q cover,
                // 1920w/88q banner). That source photo is deliberately
                // excluded from the Gallery grid below — see
                // BuildQLatisImages.
                CoverImage = "/images/projects/q-latis/cover.webp",
                // Real catalogue (2026-08-10) — client-supplied
                // katalog/proje-katalogu-q-latis.pdf copied as-is to
                // wwwroot/documents/catalogues/ per 07_AssetStructure.md's
                // convention. CatalogueComingSoon left false (default): this
                // is a real, downloadable catalogue, not a coming-soon stub.
                CataloguePath = "/documents/catalogues/q-latis-katalog.pdf",
                // No real Site Plan supplied yet — SitePlanImages stays empty
                // and SitePlanComingSoon activates the Hero's "Vaziyet Planı"
                // button with the shared Coming Soon toast instead of hiding
                // it (Alinda Gold Residence revision precedent).
                SitePlanComingSoon = true,
                // Grounded in the 3 supplied Social Areas renders
                // (gallery/social/originals/1-3.png: open terrace/sun
                // loungers with an attached gym deck, an aerial-yoga/pilates
                // studio, a dedicated fitness equipment wall) — wording is
                // generic and meant to be replaced with approved marketing
                // copy before launch; the facilities themselves are not
                // invented.
                Amenities = "Açık Teras ve Dinlenme Alanı\nFitness Salonu\nAeroyoga / Pilates Stüdyosu",
                DisplayOrder = 13,
                IsFeatured = false,
                IsPublished = true,
                CreatedAt = now,
                UpdatedAt = now,
                // Real Exterior/Interior/Social Areas gallery (2026-08-10) —
                // see BuildQLatisImages.
                Images = BuildQLatisImages(),
                // No real floor plan artwork supplied — FloorPlans left
                // empty (not assigned) so _FloorPlans.cshtml's empty state
                // renders ("Planlar yakında eklenecektir.") instead of
                // BuildPlaceholderFloorPlans' invented 2+1/68m² stand-in.
                //
                // Concept carousel (2026-08-10) — 1 real video slide followed
                // by the 3 strongest exterior renders — see
                // BuildQLatisConceptVideos/BuildQLatisConceptImages.
                ConceptVideos = BuildQLatisConceptVideos(),
                ConceptImages = BuildQLatisConceptImages()
            }
        };

        var missingProjects = projects.Where(p => !existingSlugs.Contains(p.Slug)).ToList();
        if (missingProjects.Count > 0)
        {
            context.Projects.AddRange(missingProjects);
            await context.SaveChangesAsync();
        }

        await ReconcileMissingFloorPlansAsync(context, projects);
        await ReconcileMissingCataloguePathsAsync(context, projects);
        await ReconcileMissingImagesAsync(context, projects);
    }

    // Not a seed — backfills ProjectImage rows for a project that was already
    // present in the database (0 Images) before real gallery photos were
    // added to its seed definition above (Davutlar D Latis Media phase,
    // 2026-08-05 — see BuildDavutlarDLatisImages). Same shape as
    // ReconcileMissingFloorPlansAsync: a no-op for any project that already
    // has at least one ProjectImage row, so this never overwrites real
    // photos supplied — or reordered/recategorized — later.
    private static async Task ReconcileMissingImagesAsync(AppDbContext context, List<Project> seedProjects)
    {
        var slugsWithSeededImages = seedProjects
            .Where(p => p.Images.Count > 0)
            .Select(p => p.Slug)
            .ToHashSet();

        var projectIdsWithImages = new HashSet<int>(
            await context.ProjectImages.Select(i => i.ProjectId).Distinct().ToListAsync());

        var projectsNeedingImages = await context.Projects
            .Where(p => slugsWithSeededImages.Contains(p.Slug))
            .ToListAsync();
        projectsNeedingImages = projectsNeedingImages
            .Where(p => !projectIdsWithImages.Contains(p.Id))
            .ToList();

        if (projectsNeedingImages.Count == 0)
        {
            return;
        }

        foreach (var project in projectsNeedingImages)
        {
            var seedProject = seedProjects.First(p => p.Slug == project.Slug);
            foreach (var image in seedProject.Images)
            {
                context.ProjectImages.Add(new ProjectImage
                {
                    ProjectId = project.Id,
                    ImagePath = image.ImagePath,
                    AltText = image.AltText,
                    DisplayOrder = image.DisplayOrder,
                    Category = image.Category,
                    Block = image.Block,
                    ApartmentType = image.ApartmentType
                });
            }
        }

        await context.SaveChangesAsync();
    }

    // Not a seed — backfills CataloguePath for projects that were already
    // present in the database before their placeholder catalogue was added
    // above (Hero Banner Catalogue Availability Audit, 2026-08-01; all
    // projects except Nysa Gold had a null CataloguePath, so the Hero
    // Banner's "Proje Kataloğunu İndir" button only ever rendered for that
    // one project — see docs/14_Decisions.md). Same shape as
    // ReconcileMissingFloorPlansAsync: a no-op for any project that already
    // has a CataloguePath, so this never overwrites a real catalogue
    // supplied later — only fills a null.
    private static async Task ReconcileMissingCataloguePathsAsync(AppDbContext context, List<Project> seedProjects)
    {
        var seededCataloguePaths = seedProjects
            .Where(p => !string.IsNullOrWhiteSpace(p.CataloguePath))
            .ToDictionary(p => p.Slug, p => p.CataloguePath!);

        var projectsToCheck = await context.Projects
            .Where(p => seededCataloguePaths.Keys.Contains(p.Slug))
            .ToListAsync();

        var projectsNeedingCatalogue = projectsToCheck
            .Where(p => string.IsNullOrWhiteSpace(p.CataloguePath))
            .ToList();

        if (projectsNeedingCatalogue.Count == 0)
        {
            return;
        }

        foreach (var project in projectsNeedingCatalogue)
        {
            project.CataloguePath = seededCataloguePaths[project.Slug];
        }

        await context.SaveChangesAsync();
    }

    // Not a seed — backfills FloorPlan rows for projects that were already
    // present in the database before their placeholder entry was added
    // above (Floor Plans Availability Audit, 2026-08-01; the 8 affected
    // projects previously had 0 FloorPlan rows, so their Daire Planları
    // section never rendered — see docs/14_Decisions.md). Safe to run on
    // every startup, same shape as ReconcileConfirmedProjectNamesAsync: a
    // no-op for any project that already has at least one FloorPlan row, so
    // this never overwrites real floor plan data supplied later.
    private static async Task ReconcileMissingFloorPlansAsync(AppDbContext context, List<Project> seedProjects)
    {
        var slugsWithSeededFloorPlans = seedProjects
            .Where(p => p.FloorPlans.Count > 0)
            .Select(p => p.Slug)
            .ToHashSet();

        var projectIdsWithFloorPlans = new HashSet<int>(
            await context.FloorPlans.Select(f => f.ProjectId).Distinct().ToListAsync());

        var projectsNeedingFloorPlans = await context.Projects
            .Where(p => slugsWithSeededFloorPlans.Contains(p.Slug))
            .ToListAsync();
        projectsNeedingFloorPlans = projectsNeedingFloorPlans
            .Where(p => !projectIdsWithFloorPlans.Contains(p.Id))
            .ToList();

        if (projectsNeedingFloorPlans.Count == 0)
        {
            return;
        }

        foreach (var project in projectsNeedingFloorPlans)
        {
            var seedProject = seedProjects.First(p => p.Slug == project.Slug);
            foreach (var floorPlan in seedProject.FloorPlans)
            {
                context.FloorPlans.Add(new FloorPlan
                {
                    ProjectId = project.Id,
                    ApartmentType = floorPlan.ApartmentType,
                    ImagePath = floorPlan.ImagePath,
                    NetAreaM2 = floorPlan.NetAreaM2,
                    GrossAreaM2 = floorPlan.GrossAreaM2,
                    SalesGrossAreaM2 = floorPlan.SalesGrossAreaM2,
                    DisplayOrder = floorPlan.DisplayOrder,
                    Rooms = floorPlan.Rooms
                        .Select(r => new FloorPlanRoom { Name = r.Name, AreaM2 = r.AreaM2, DisplayOrder = r.DisplayOrder })
                        .ToList()
                });
            }
        }

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

    // Not a seed — backfills already-seeded rows with the client-confirmed
    // Location/Status/ProjectType for these 12 projects (2026-08-20 metadata
    // update). Only these three fields are touched; every other column is
    // left exactly as already seeded. Safe to run every startup: a no-op
    // once each field already matches.
    private static async Task ReconcileProjectMetadataAsync(AppDbContext context)
    {
        var confirmedMetadata = new Dictionary<string, (string Location, ProjectStatus Status, string ProjectType)>
        {
            ["nysa-gold"] = ("Aydın Efeler", ProjectStatus.Ongoing, "Residence"),
            ["le-jardin"] = ("Aydın Efeler", ProjectStatus.Ongoing, "Villa"),
            ["tralles-gold"] = ("Aydın Efeler", ProjectStatus.Completed, "Residence"),
            ["nlatis"] = ("İzmir", ProjectStatus.Completed, "Residence"),
            ["alinda-gold"] = ("Aydın Efeler", ProjectStatus.Completed, "Residence"),
            ["magnesia-gold"] = ("Aydın Efeler", ProjectStatus.Completed, "Residence"),
            ["la-fiore-karabag"] = ("Aydın İncirliova", ProjectStatus.Completed, "Villa"),
            ["la-fiore-karabag-2-etap"] = ("Aydın İncirliova", ProjectStatus.Ongoing, "Villa"),
            ["kuyulu-la-via-villalar-birinci-etap"] = ("Aydın Kuyulu", ProjectStatus.Ongoing, "Villa"),
            ["davutlar-d-latis"] = ("Kuşadası", ProjectStatus.Ongoing, "Residence"),
            ["ferhunde-hanim-apt"] = ("Aydın Efeler", ProjectStatus.Completed, "Apartman"),
            ["q-latis"] = ("Kuşadası", ProjectStatus.Ongoing, "Residence")
        };

        var projectsToCheck = await context.Projects
            .Where(p => confirmedMetadata.Keys.Contains(p.Slug))
            .ToListAsync();

        var changed = false;
        foreach (var project in projectsToCheck)
        {
            var (location, status, projectType) = confirmedMetadata[project.Slug];

            if (project.Location != location)
            {
                project.Location = location;
                changed = true;
            }

            if (project.Status != status)
            {
                project.Status = status;
                changed = true;
            }

            if (project.ProjectType != projectType)
            {
                project.ProjectType = projectType;
                changed = true;
            }
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    // Not a seed — backfills real ShortDescription/Description copy onto
    // already-seeded rows (Placeholder Content sweep, 2026-08-20 client
    // request) for every project whose seed definition above still had
    // "Placeholder short description for the X development." / "Placeholder
    // full description. Replace with approved project copy before launch."
    // literally hardcoded. Same "only touch the field while it still holds
    // the known old placeholder string" guard as
    // ReconcileLeJardinConceptCarouselAsync's ShortDescription fix above, so
    // this is a no-op — and never clobbers — once a row already carries the
    // real copy (from this reconcile, a fresh seed, or later approved
    // editorial changes made some other way).
    private static async Task ReconcileProjectPlaceholderCopyAsync(AppDbContext context)
    {
        var shortDescriptionUpdates = new Dictionary<string, (string OldValue, string NewValue)>
        {
            ["nysa-gold"] = ("Placeholder short description for the Nysa Gold development.", "Zarif cepheleri, yüzme havuzu ve özenle tasarlanmış sosyal alanlarıyla Aydın Efeler'de ayrıcalıklı bir yaşam sunan modern rezidans."),
            ["tralles-gold"] = ("Placeholder short description for the Tralles Gold development.", "Beyaz cepheleri ve dikey kırmızı vurgularıyla Aydın Efeler'in siluetinde öne çıkan, güçlü bir mimari kimliğe sahip tamamlanmış rezidans."),
            ["nlatis"] = ("Placeholder short description for the Nlatis development.", "Kıvrımlı ahşap tonlu çatı hattı ve cam kaplı cephesiyle özgün bir mimari kimliğe sahip, İzmir'de tamamlanmış rezidans projesi."),
            ["alinda-gold"] = ("Placeholder short description for the Alinda Gold Residence development.", "Akıcı hatları ve zarif çatı aydınlatmasıyla Aydın Efeler'in siluetine yeni bir karakter katan, tamamlanmış modern rezidans."),
            ["magnesia-gold"] = ("Placeholder short description for the Magnesia Gold Residence development.", "Geniş peyzaj alanları ve özenle tasarlanmış ortak yaşam alanlarıyla Aydın Efeler'de bir aradalığı ön plana çıkaran tamamlanmış rezidans."),
            ["la-fiore-karabag"] = ("Placeholder short description for the La Fiore Karabağ development.", "Gür çam ormanının içine özenle yerleştirilmiş tek katlı villalarıyla Aydın İncirliova'da sakin ve mahrem bir yaşam sunan tamamlanmış proje."),
            ["la-fiore-karabag-2-etap"] = ("Placeholder short description for the La Fiore Karabağ 2. Etap development.", "Yüzlerce yıllık zeytin ağaçları arasında konumlanan, doğayla bütünleşen villa yerleşimiyle Aydın İncirliova'da yükselen ayrıcalıklı bir proje."),
            ["kuyulu-la-via-villalar-birinci-etap"] = ("Placeholder short description for the La Via Villalar 1. Etap development.", "Kendi bahçesi ve garajıyla bağımsız villa mimarisini Kuyulu'nun sakin dokusuyla buluşturan ayrıcalıklı bir yaşam alanı."),
            ["ferhunde-hanim-apt"] = ("Placeholder short description for the Ferhunde Hanım Apt. development.", "Yumuşak hatlı balkonları ve özenle seçilmiş cephe dokusuyla Aydın Efeler'de dikkat çeken, tamamlanmış bir apartman projesi."),
            ["kuyulu-avm"] = ("Placeholder short description for the La Via AVM development.", "Gölgelikli teraslar ve özenle tasarlanmış peyzajıyla alışverişi bir yaşam deneyimine dönüştüren, Aydın Efeler'de yükselen ticari proje.")
        };

        const string oldDescription = "Placeholder full description. Replace with approved project copy before launch.";
        var descriptionUpdates = new Dictionary<string, string>
        {
            ["nysa-gold"] = "Aydın Efeler'in gelişen bölgesinde yükselen Nysa Gold Residence, modern mimariyi konforlu ve güvenli bir yaşamla buluşturuyor. Yüzme havuzu, basketbol sahası, çocuk oyun alanı ve peyzajlı bahçeleriyle proje, her yaştan sakinine günün her saatinde keyifle vakit geçirebileceği sosyal alanlar sunuyor. Geniş balkonları ve ferah iç mekânlarıyla her daire, doğal ışığı içeri taşıyacak şekilde tasarlandı.",
            ["le-jardin"] = "Akdeniz'in sıcak karakterini modern mimariyle buluşturan Le Jardin, begonvillerle sarılı girişi ve açık renk taş kaplamalı cephesiyle sakinlerini davetkâr bir atmosferle karşılıyor. Özel havuzu, gölgelikli terası ve geniş bahçesiyle proje, iç mekânın konforunu dış mekânın huzuruyla bütünleştiriyor. Aydın Efeler'de konumlanan Le Jardin, hem günlük yaşamın hem de misafirlerinizi ağırlamanın keyfini çıkarabileceğiniz ayrıcalıklı bir villa deneyimi sunuyor.",
            ["tralles-gold"] = "Aydın Efeler'in dokusu içinde yükselen Tralles Gold Residence, beyaz cepheleri ve dikey kırmızı vurgularıyla çevresinden hemen ayrışan güçlü bir mimari kimlik taşıyor. Akşam saatlerinde ışıklandırılan cephesiyle şehrin gece silüetinde kendine özgü bir karakter kazanan proje, gündüzün sadeliğini gecenin canlılığıyla buluşturuyor. Tamamlanmış bu proje, Ançın İnşaat'ın zanaatkârlık anlayışını yansıtan kalıcı bir yaşam alanı olarak sakinlerine sunuluyor.",
            ["nlatis"] = "İzmir'de tamamlanan Nlatis, kıvrımlı ahşap tonlu çatı hattı ve cam kaplı cephesiyle özgün bir mimari kimlik taşıyor. Koyu renkli cephe panelleri ve dikey ahşap vurgularının oluşturduğu katmanlı ritim, cam balkonlarla desteklenerek modern ve sade bir görünüm sunuyor. Zemin kattaki sosyal kullanım alanları, binayı çevresiyle buluşturan canlı bir karşılama noktası oluşturuyor.",
            ["alinda-gold"] = "Aydın Efeler'in ufkunda yükselen Alinda Gold Residence, akıcı hatları ve zarif çatı aydınlatmasıyla şehrin siluetine yeni bir karakter katıyor. Bloklar arasına özenle yerleştirilen havuz, yürüyüş yolları ve peyzaj alanları, sakinlerine güne açık havada başlama ve günü dinginlikle bitirme imkânı sunuyor. Özenli aydınlatma tasarımı ve ferah kompozisyonuyla proje girişi, Ançın İnşaat imzasının premium yaklaşımını en baştan hissettiriyor.",
            ["magnesia-gold"] = "Aydın'ın ufkunda yan yana yükselen Magnesia Gold Residence blokları, geniş peyzaj alanları ve özenle tasarlanmış ortak yaşam alanlarıyla bir aradalığı ön plana çıkarıyor. Palmiyelerle çevrili yürüyüş yolları, havuzlar ve geniş çim alanları günün her saatinde huzurlu bir mola sunarken, akşam saatlerinde özenli aydınlatmayla öne çıkan havuz çevresi sosyalleşmek isteyen sakinler için davetkâr bir buluşma noktasına dönüşüyor. Tamamlanan bu proje, yeşille mimarinin uyumunu günlük yaşamın merkezine taşıyor.",
            ["la-fiore-karabag"] = "Aydın İncirliova'da gür çam ormanının içine özenle yerleştirilen La Fiore Karabağ, tek katlı villalarıyla sakin ve mahremiyeti önceleyen bir yaşam alanı sunuyor. Yerleşim içindeki kesintisiz yürüyüş yolları ve peyzaj düzenlemesi doğayla iç içe bir günlük yaşam deneyimi vaat ederken, villa bahçelerindeki pergola altı oturma grupları ve şömineler iç mekânı bahçeyle bütünleştiriyor. Taş kaplı cephesi ve ahşap detaylarıyla dikkat çeken proje girişi, güvenlikli giriş noktasıyla sakinlerinin huzurunu güvence altına alıyor.",
            ["la-fiore-karabag-2-etap"] = "Aydın Karabağ'ın yüzlerce yıllık zeytin ağaçları arasında konumlanan La Fiore Karabağ 2. Etap, arazinin doğal eğimini ve dokusunu koruyan bir yerleşim planıyla hayat buluyor. Geniş yürüyüş yolları, peyzajlı meydanlar ve ortak sosyal alanlarla örülen proje, sakinlerini binalardan çok bir arada yaşama davet ediyor. Taş duvarları, zarif aydınlatması ve zeytin ağaçlarıyla çevrili villalarıyla La Fiore Karabağ 2. Etap, hem huzurlu bir yaşamı hem de Karabağ'ın gelişen değeriyle güçlü bir yatırım fırsatını bir araya getiriyor.",
            ["kuyulu-la-via-villalar-birinci-etap"] = "La Via Villalar 1. Etap, her biri kendi bahçesi ve garajıyla bağımsız bir villa mimarisini Kuyulu'nun sakin dokusuyla buluşturuyor. Ahşap dokulu cepheler, zarif aydınlatmalar ve modern çizgiler, Ançın İnşaat imzasının kalite anlayışını her villada yeniden tanımlıyor. Her villanın kendi çatı terası, özel havuzu ve peyzajlı bahçesiyle tasarlanan proje, sosyal yaşamı ailenizin kendi mahremiyetine taşıyarak hem huzurlu bir yaşamı hem de Kuyulu'nun gelişen değeriyle güçlü bir yatırım fırsatını bir araya getiriyor.",
            ["davutlar-d-latis"] = "Kuşadası'nın binlerce yıllık tarihi ve doğal güzellikleriyle iç içe konumlanan Davutlar D Latis, bölgenin termal su kaynaklarını modern mimari bir yaşam deneyimiyle buluşturuyor. Spa, fitness ve sosyal donatılarıyla tamamlanan proje, hem gündelik yaşamın konforunu hem de Ege'nin gözde turizm merkezlerinden birinde uzun vadeli bir yatırım fırsatını bir arada sunuyor. Ançın İnşaat güvencesiyle hayata geçen bu proje, huzurlu bir yaşamı termal şifayla buluşturan ayrıcalıklı bir adres olmayı hedefliyor.",
            ["ferhunde-hanim-apt"] = "Ferhunde Hanım Apt., yumuşak hatlı balkonları ve özenle seçilmiş cephe dokusuyla Aydın Efeler'de dikkat çeken bir mimari kimlik sunuyor. Geniş camları ve ferah balkonlarıyla her kat doğal ışığı içeri taşırken, yüksek çitlerle çevrili özel bahçe alanı sakinlerine şehrin gürültüsünden uzak, güvenli ve huzurlu bir dış mekân yaşamı sağlıyor. Çevre duvarları ve peyzajlı ön bahçesiyle tamamlanan proje, günlük yaşamı kolaylaştıran ve değerini koruyan bir yatırım fırsatı sunuyor.",
            ["kuyulu-avm"] = "Aydın Efeler'de geniş bir zeytinlik dokusunun kenarında yükselen La Via AVM, alışverişi gündelik bir ihtiyaçtan çok bir yaşam deneyimine dönüştürüyor. Gölgelikli teraslar, geniş yürüyüş alanları ve özenle tasarlanmış peyzajıyla proje, Ançın İnşaat'ın imza kalite anlayışını ticari ölçekte yeniden yorumluyor. Doğal taş kaplamalar, ahşap lamel dokular ve gece boyunca yumuşak bir çizgiyle beliren aydınlatma, cepheye kalıcı ve zarif bir karakter kazandırıyor.",
            ["q-latis"] = "Kuşadası'nın eşsiz doğasıyla iç içe konumlanan Hacıfeyzullah - Q-Latis, ahşap dokulu cepheleri ve geniş camekânlarıyla bölgenin karakterine saygılı, çağdaş bir mimari dil sunuyor. Üst kat dairelerin geniş balkonlarından izlenen Ege Denizi manzarası her günü ayrıcalıklı kılarken, açık teras, fitness salonu ve aeroyoga/pilates stüdyosu gibi sosyal donatılar aktif bir yaşam tarzını destekliyor. Zeytinliklerle çevrili tepelik konumuyla proje, Kuşadası'nın doğal dokusunu koruyan bir yerleşim anlayışıyla tasarlandı."
        };

        var slugsToCheck = shortDescriptionUpdates.Keys.Union(descriptionUpdates.Keys).ToHashSet();
        var projectsToCheck = await context.Projects
            .Where(p => slugsToCheck.Contains(p.Slug))
            .ToListAsync();

        var changed = false;
        foreach (var project in projectsToCheck)
        {
            if (shortDescriptionUpdates.TryGetValue(project.Slug, out var shortDescriptionUpdate) &&
                project.ShortDescription == shortDescriptionUpdate.OldValue)
            {
                project.ShortDescription = shortDescriptionUpdate.NewValue;
                changed = true;
            }

            if (descriptionUpdates.TryGetValue(project.Slug, out var newDescription) &&
                project.Description == oldDescription)
            {
                project.Description = newDescription;
                changed = true;
            }
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    // Not a seed — a one-time reconciliation renaming the row seeded as
    // "Dlatis Thermal Wellness Residence" / "dlatis-thermal-wellness" once
    // the client's real project name and asset folder ("Davutlar D Latis" /
    // wwwroot/images/projects/davutlar-d-latis) were confirmed (2026-08-05).
    // Must run before SeedProjectsAsync's existingSlugs snapshot so the
    // renamed row is recognized as already existing under its new slug
    // rather than seeding a second, duplicate project. Safe to run on every
    // startup: a no-op once the slug has already been migrated.
    private static async Task ReconcileDavutlarDLatisRenameAsync(AppDbContext context)
    {
        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Slug == "dlatis-thermal-wellness");

        if (project is null)
        {
            return;
        }

        project.Name = "Davutlar D Latis";
        project.Slug = "davutlar-d-latis";
        project.CoverImage = "/images/projects/davutlar-d-latis/cover.webp";
        project.CataloguePath = "/documents/catalogues/davutlar-d-latis-katalog.pdf";

        await context.SaveChangesAsync();
    }

    // Not a seed — corrects an already-seeded Davutlar D Latis row's
    // CoverImage. Originally pointed at a never-supplied cover.webp (Home/
    // Projects cards fell back to the shared placeholder), then moved
    // (2026-08-05) to point directly at the confirmed gallery original
    // (exterior-07.jpeg, client-selected as the card image) since no
    // dedicated file existed yet. Moved again (2026-08-06, Project Card
    // Thumbnail Quality fix) to a real dedicated cover.webp generated from
    // that same original at a higher quality than the shared gallery-
    // thumbnail default — the gallery-original path meant the card's
    // thumbnail and that one Gallery photo were the exact same file, so
    // sharpening it for the card would have altered the Gallery too. The
    // Detail Hero (3A.jpeg, via ProjectsController.HeroBackgroundImageUrl's
    // own {slug}/banner.webp convention) and Gallery are untouched by this.
    // Safe to run every startup: a no-op once CoverImage already matches.
    private static async Task ReconcileDavutlarDLatisCoverImageAsync(AppDbContext context)
    {
        const string correctCoverImage = "/images/projects/davutlar-d-latis/cover.webp";

        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Slug == "davutlar-d-latis");

        if (project is null || project.CoverImage == correctCoverImage)
        {
            return;
        }

        project.CoverImage = correctCoverImage;
        await context.SaveChangesAsync();
    }

    // Not a seed — backfills an already-seeded Davutlar D Latis row with the
    // Media phase's real floor plans, concept video, concept copy and
    // location photo (2026-08-09), same targeted-swap shape as
    // ReconcileLaFioreKarabag2EtapFloorPlansAsync below. Each field is
    // independently guarded (only replaced while it still looks like the
    // original seed value), so this never overwrites real edits made after
    // this ran once, and running it twice is a no-op. Site plan backfill
    // removed (2026-08-17) — see ReconcileDavutlarDLatisRemoveSitePlanAsync.
    private static async Task ReconcileDavutlarDLatisMediaAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.FloorPlans)
            .Include(p => p.ConceptVideos)
            .Include(p => p.SitePlanImages)
            .FirstOrDefaultAsync(p => p.Slug == "davutlar-d-latis");

        if (project is null)
        {
            return;
        }

        var changed = false;

        if (project.FloorPlans.Count == 1 && project.FloorPlans.First().ApartmentType == "2+1")
        {
            context.FloorPlans.RemoveRange(project.FloorPlans);

            foreach (var floorPlan in BuildDavutlarDLatisFloorPlans())
            {
                context.FloorPlans.Add(new FloorPlan
                {
                    ProjectId = project.Id,
                    ApartmentType = floorPlan.ApartmentType,
                    ImagePath = floorPlan.ImagePath,
                    NetAreaM2 = floorPlan.NetAreaM2,
                    GrossAreaM2 = floorPlan.GrossAreaM2,
                    SalesGrossAreaM2 = floorPlan.SalesGrossAreaM2,
                    DisplayOrder = floorPlan.DisplayOrder,
                    Rooms = floorPlan.Rooms
                        .Select(r => new FloorPlanRoom { Name = r.Name, AreaM2 = r.AreaM2, DisplayOrder = r.DisplayOrder })
                        .ToList()
                });
            }

            changed = true;
        }

        if (project.ShortDescription == "Placeholder short description for the Davutlar D Latis development.")
        {
            project.ShortDescription = "Kuşadası'nın bakir doğasında, termal şifayla buluşan ayrıcalıklı bir yaşam alanı.";
            changed = true;
        }

        if (string.IsNullOrEmpty(project.ConceptDescription))
        {
            project.ConceptDescription = "Dilek Yarımadası Milli Parkı'nın eteğinde, Efes ve Meryem Ana Evi gibi kadim mirasların komşuluğunda yükselen Davutlar D Latis, Davutlar'ın eşsiz termal sularını modern mimariyle buluşturuyor. Spa, fitness ve sosyal yaşam alanlarıyla tamamlanan bu wellness konsepti, Ege'nin turizm başkentinde huzurlu bir yaşamı güçlü bir yatırım fırsatıyla bir araya getiriyor.";
            changed = true;
        }

        if (string.IsNullOrEmpty(project.ConceptVideoPath))
        {
            project.ConceptVideoPath = "/images/projects/davutlar-d-latis/concept/video.mp4";
            project.ConceptVideoPosterPath = "/images/projects/davutlar-d-latis/concept/poster.webp";
            changed = true;
        }

        if (string.IsNullOrEmpty(project.LocationImagePath))
        {
            project.LocationImagePath = "/images/projects/davutlar-d-latis/location.webp";
            changed = true;
        }

        if (project.ConceptVideos.Count == 0)
        {
            foreach (var video in BuildDavutlarDLatisConceptVideos())
            {
                context.ProjectConceptVideos.Add(new ProjectConceptVideo
                {
                    ProjectId = project.Id,
                    VideoPath = video.VideoPath,
                    PosterPath = video.PosterPath,
                    Eyebrow = video.Eyebrow,
                    Title = video.Title,
                    Description = video.Description,
                    DisplayOrder = video.DisplayOrder
                });
            }

            changed = true;
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    // Not a seed — replaces an already-seeded La Fiore Karabağ 2. Etap row's
    // single generic "2+1" placeholder FloorPlan (from BuildPlaceholderFloorPlans,
    // before this project's real kat-planlari/ drawings were wired in) with
    // the real 14-entry set from BuildLaFioreKarabag2EtapFloorPlans (Floor
    // Plans pilot, 2026-08-06). Unlike ReconcileMissingFloorPlansAsync (which
    // only backfills a project with 0 rows), this project already had 1 row
    // seeded, so the generic reconcile is a permanent no-op for it — this
    // targeted one-time swap is required instead, same shape as
    // ReconcileDavutlarDLatisCoverImageAsync. Guarded to only replace rows
    // that still look like the untouched placeholder (single row, "2+1"),
    // so it never overwrites real floor plan edits made after this ran once.
    // EF Core's configured Cascade delete (AppDbContext) removes each
    // FloorPlan's Rooms along with it.
    private static async Task ReconcileLaFioreKarabag2EtapFloorPlansAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.FloorPlans)
            .FirstOrDefaultAsync(p => p.Slug == "la-fiore-karabag-2-etap");

        if (project is null
            || project.FloorPlans.Count != 1
            || project.FloorPlans.First().ApartmentType != "2+1")
        {
            return;
        }

        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var floorPlan in BuildLaFioreKarabag2EtapFloorPlans())
        {
            context.FloorPlans.Add(new FloorPlan
            {
                ProjectId = project.Id,
                ApartmentType = floorPlan.ApartmentType,
                ImagePath = floorPlan.ImagePath,
                NetAreaM2 = floorPlan.NetAreaM2,
                GrossAreaM2 = floorPlan.GrossAreaM2,
                SalesGrossAreaM2 = floorPlan.SalesGrossAreaM2,
                DisplayOrder = floorPlan.DisplayOrder,
                Rooms = floorPlan.Rooms
                    .Select(r => new FloorPlanRoom { Name = r.Name, AreaM2 = r.AreaM2, DisplayOrder = r.DisplayOrder })
                    .ToList()
            });
        }

        await context.SaveChangesAsync();
    }

    // Not a seed — backfills an already-seeded La Fiore Karabağ 2. Etap row
    // with the Vaziyet Planı/Concept/Gallery phase's site plan images,
    // concept image-carousel slides and CatalogueComingSoon flag
    // (2026-08-09), same targeted-backfill shape as
    // ReconcileDavutlarDLatisMediaAsync above. Each piece is independently
    // guarded, so this never overwrites real edits made after this ran
    // once, and running it twice is a no-op.
    private static async Task ReconcileLaFioreKarabag2EtapVaziyetPlaniVeKonseptAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.SitePlanImages)
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "la-fiore-karabag-2-etap");

        if (project is null)
        {
            return;
        }

        var changed = false;

        if (project.SitePlanImages.Count == 0)
        {
            foreach (var sitePlanImage in BuildLaFioreKarabag2EtapSitePlanImages())
            {
                context.ProjectSitePlanImages.Add(new ProjectSitePlanImage
                {
                    ProjectId = project.Id,
                    ImagePath = sitePlanImage.ImagePath,
                    AltText = sitePlanImage.AltText,
                    DisplayOrder = sitePlanImage.DisplayOrder
                });
            }

            changed = true;
        }

        if (project.ConceptImages.Count == 0)
        {
            foreach (var conceptImage in BuildLaFioreKarabag2EtapConceptImages())
            {
                context.ProjectConceptImages.Add(new ProjectConceptImage
                {
                    ProjectId = project.Id,
                    ImagePath = conceptImage.ImagePath,
                    Eyebrow = conceptImage.Eyebrow,
                    Title = conceptImage.Title,
                    Description = conceptImage.Description,
                    DisplayOrder = conceptImage.DisplayOrder
                });
            }

            changed = true;
        }

        if (!project.CatalogueComingSoon)
        {
            project.CatalogueComingSoon = true;
            changed = true;
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    // Not a seed — backfills an already-seeded La Fiore Karabağ 2. Etap row
    // with the new "Tüm Dış Mekan Görselleri" ("All Exterior") gallery
    // category (Vaziyet Planı/Concept/Gallery phase, 2026-08-09), same
    // shape as ReconcileLeJardinGalleryAndCatalogueAsync below. Guarded so
    // it only adds the 49 new images once, even if this project already had
    // other images seeded.
    private static async Task ReconcileLaFioreKarabag2EtapTumDisMekanGorselleriAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Slug == "la-fiore-karabag-2-etap");

        if (project is null || project.Images.Any(image => image.Category == "All Exterior"))
        {
            return;
        }

        foreach (var image in BuildLaFioreKarabag2EtapImages().Where(image => image.Category == "All Exterior"))
        {
            context.ProjectImages.Add(new ProjectImage
            {
                ProjectId = project.Id,
                ImagePath = image.ImagePath,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                Category = image.Category
            });
        }

        await context.SaveChangesAsync();
    }

    // Not a seed — backfills an already-seeded La Via AVM (slug kuyulu-avm)
    // row with the Hero's Vaziyet Planı site plan image, the Floor Plans
    // section's floor drawings' CatalogueComingSoon-driven button, and the
    // Concept image carousel (La Via AVM update, 2026-08-09) — same
    // targeted-backfill shape as
    // ReconcileLaFioreKarabag2EtapVaziyetPlaniVeKonseptAsync above.
    // FloorPlans and CataloguePath are NOT handled here — they're already
    // covered by the generic ReconcileMissingFloorPlansAsync/
    // ReconcileMissingCataloguePathsAsync (this project had 0 FloorPlan rows
    // and a null CataloguePath), so duplicating that logic here would only
    // risk double-inserting. Each piece below is independently guarded, so
    // this never overwrites real edits made after this ran once, and running
    // it twice is a no-op. The gallery photos' Category deliberately stays
    // null (client revision, 2026-08-09 — see BuildKuyuluAvmImages), so this
    // no longer backfills a Category here either.
    private static async Task ReconcileKuyuluAvmVaziyetPlaniPlanlarVeKonseptAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .Include(p => p.SitePlanImages)
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "kuyulu-avm");

        if (project is null)
        {
            return;
        }

        var changed = false;

        if (project.SitePlanImages.Count == 0)
        {
            foreach (var sitePlanImage in BuildKuyuluAvmSitePlanImages())
            {
                context.ProjectSitePlanImages.Add(new ProjectSitePlanImage
                {
                    ProjectId = project.Id,
                    ImagePath = sitePlanImage.ImagePath,
                    AltText = sitePlanImage.AltText,
                    DisplayOrder = sitePlanImage.DisplayOrder
                });
            }

            changed = true;
        }

        if (project.ConceptImages.Count == 0)
        {
            foreach (var conceptImage in BuildKuyuluAvmConceptImages())
            {
                context.ProjectConceptImages.Add(new ProjectConceptImage
                {
                    ProjectId = project.Id,
                    ImagePath = conceptImage.ImagePath,
                    Eyebrow = conceptImage.Eyebrow,
                    Title = conceptImage.Title,
                    Description = conceptImage.Description,
                    DisplayOrder = conceptImage.DisplayOrder
                });
            }

            changed = true;
        }

        if (!project.CatalogueComingSoon)
        {
            project.CatalogueComingSoon = true;
            changed = true;
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    // Not a seed — renames an already-seeded "Lavia Kuyulu" row
    // (slug lavia-kuyulu, no images ever supplied for it) to the client's
    // real project name and asset folder, "La Via Villalar 1. Etap" /
    // kuyulu-la-via-villalar-birinci-etap (2026-08-06), same shape as
    // ReconcileDavutlarDLatisRenameAsync above. Must run before
    // SeedProjectsAsync's existingSlugs snapshot so the renamed row is
    // recognized as already present (no duplicate insert), and before
    // ReconcileKuyuluLaViaVillalarFloorPlansAsync/ReconcileMissingImagesAsync
    // so both find the row under its new slug.
    private static async Task ReconcileLaviaKuyuluToLaViaVillalarRenameAsync(AppDbContext context)
    {
        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Slug == "lavia-kuyulu");

        if (project is null)
        {
            return;
        }

        project.Name = "La Via Villalar 1. Etap";
        project.Slug = "kuyulu-la-via-villalar-birinci-etap";
        project.ShortDescription = "Kendi bahçesi ve garajıyla bağımsız villa mimarisini Kuyulu'nun sakin dokusuyla buluşturan ayrıcalıklı bir yaşam alanı.";
        project.CoverImage = "/images/projects/kuyulu-la-via-villalar-birinci-etap/cover.webp";
        project.CataloguePath = "/documents/catalogues/kuyulu-la-via-villalar-birinci-etap-katalog.pdf";

        await context.SaveChangesAsync();
    }

    // Not a seed — replaces an already-seeded La Via Villalar 1. Etap row's
    // single generic "2+1" placeholder FloorPlan (from BuildPlaceholderFloorPlans,
    // seeded back when the row was still "Lavia Kuyulu") with the real
    // 4-entry set from BuildKuyuluLaViaVillalarFloorPlans, same shape as
    // ReconcileLaFioreKarabag2EtapFloorPlansAsync above. Guarded to only
    // replace rows that still look like the untouched placeholder (single
    // row, "2+1"), so it never overwrites real floor plan edits made after
    // this ran once. Must run after
    // ReconcileLaviaKuyuluToLaViaVillalarRenameAsync so the project is
    // found under its new slug.
    private static async Task ReconcileKuyuluLaViaVillalarFloorPlansAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.FloorPlans)
            .FirstOrDefaultAsync(p => p.Slug == "kuyulu-la-via-villalar-birinci-etap");

        if (project is null
            || project.FloorPlans.Count != 1
            || project.FloorPlans.First().ApartmentType != "2+1")
        {
            return;
        }

        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var floorPlan in BuildKuyuluLaViaVillalarFloorPlans())
        {
            context.FloorPlans.Add(new FloorPlan
            {
                ProjectId = project.Id,
                ApartmentType = floorPlan.ApartmentType,
                ImagePath = floorPlan.ImagePath,
                NetAreaM2 = floorPlan.NetAreaM2,
                GrossAreaM2 = floorPlan.GrossAreaM2,
                SalesGrossAreaM2 = floorPlan.SalesGrossAreaM2,
                DisplayOrder = floorPlan.DisplayOrder,
                Rooms = floorPlan.Rooms
                    .Select(r => new FloorPlanRoom { Name = r.Name, AreaM2 = r.AreaM2, DisplayOrder = r.DisplayOrder })
                    .ToList()
            });
        }

        await context.SaveChangesAsync();
    }

    // Not a seed — backfills an already-seeded La Via Villalar 1. Etap row
    // with the Vaziyet Planı/Çatı Katı/Katalog/Konsept revision's site plan
    // image, roof floor plan and concept image-carousel slides (2026-08-09),
    // same targeted-backfill shape as
    // ReconcileLaFioreKarabag2EtapVaziyetPlaniVeKonseptAsync above. The
    // catalogue swap itself (real PDF replacing the placeholder at the same
    // CataloguePath) is a file-only change — CatalogueComingSoon is already
    // false for this project, so no flag flip is needed here. Each piece
    // below is independently guarded, so this never overwrites real edits
    // made after this ran once, and running it twice is a no-op.
    private static async Task ReconcileKuyuluLaViaVillalarVaziyetPlaniCatiKatiVeKonseptAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.FloorPlans)
            .Include(p => p.SitePlanImages)
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "kuyulu-la-via-villalar-birinci-etap");

        if (project is null)
        {
            return;
        }

        var changed = false;

        if (project.SitePlanImages.Count == 0)
        {
            foreach (var sitePlanImage in BuildKuyuluLaViaVillalarSitePlanImages())
            {
                context.ProjectSitePlanImages.Add(new ProjectSitePlanImage
                {
                    ProjectId = project.Id,
                    ImagePath = sitePlanImage.ImagePath,
                    AltText = sitePlanImage.AltText,
                    DisplayOrder = sitePlanImage.DisplayOrder
                });
            }

            changed = true;
        }

        if (project.FloorPlans.All(fp => fp.ApartmentType != "Çatı Katı"))
        {
            var catiKati = BuildKuyuluLaViaVillalarFloorPlans().Single(fp => fp.ApartmentType == "Çatı Katı");
            context.FloorPlans.Add(new FloorPlan
            {
                ProjectId = project.Id,
                ApartmentType = catiKati.ApartmentType,
                ImagePath = catiKati.ImagePath,
                NetAreaM2 = catiKati.NetAreaM2,
                GrossAreaM2 = catiKati.GrossAreaM2,
                SalesGrossAreaM2 = catiKati.SalesGrossAreaM2,
                DisplayOrder = catiKati.DisplayOrder,
                Rooms = catiKati.Rooms
                    .Select(r => new FloorPlanRoom { Name = r.Name, AreaM2 = r.AreaM2, DisplayOrder = r.DisplayOrder })
                    .ToList()
            });

            changed = true;
        }

        if (project.ConceptImages.Count == 0)
        {
            foreach (var conceptImage in BuildKuyuluLaViaVillalarConceptImages())
            {
                context.ProjectConceptImages.Add(new ProjectConceptImage
                {
                    ProjectId = project.Id,
                    ImagePath = conceptImage.ImagePath,
                    Eyebrow = conceptImage.Eyebrow,
                    Title = conceptImage.Title,
                    Description = conceptImage.Description,
                    DisplayOrder = conceptImage.DisplayOrder
                });
            }

            changed = true;
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    // Not a seed — corrects an already-seeded Ferhunde Hanım Apt. row's
    // CoverImage to its new dedicated cover.webp (Project Card Thumbnail
    // Quality fix, 2026-08-06) — see ReconcileDavutlarDLatisCoverImageAsync
    // above for why a dedicated file replaces the gallery-original path.
    // Safe to run every startup: a no-op once CoverImage already matches.
    private static async Task ReconcileFerhundeHanimAptCoverImageAsync(AppDbContext context)
    {
        const string correctCoverImage = "/images/projects/ferhunde-hanim-apt/cover.webp";

        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Slug == "ferhunde-hanim-apt");

        if (project is null || project.CoverImage == correctCoverImage)
        {
            return;
        }

        project.CoverImage = correctCoverImage;
        await context.SaveChangesAsync();
    }

    // Not a seed — replaces an already-seeded Ferhunde Hanım Apt. row's
    // broken-path gallery images (its original seed pointed at
    // gallery/exterior|interior/originals/ files that were never actually
    // supplied, so the Gallery rendered empty) and generic "2+1" placeholder
    // FloorPlan with the real assets added in the Gallery/Floor Plans/
    // Vaziyet Planı/Concept revision (2026-08-10), and adds the new Site
    // Plan/Concept/Location/CatalogueComingSoon data — same
    // RemoveRange-then-rebuild shape as ReconcileNysaGoldMediaOverhaulAsync.
    // Guarded on SitePlanImages/ConceptImages both being empty (only this
    // reconcile ever populates them for this project), so it runs exactly
    // once and never overwrites real edits made after it ran.
    private static async Task ReconcileFerhundeHanimAptRevisionAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .Include(p => p.FloorPlans).ThenInclude(f => f.Rooms)
            .Include(p => p.SitePlanImages)
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "ferhunde-hanim-apt");

        if (project is null || project.SitePlanImages.Count > 0 || project.ConceptImages.Count > 0)
        {
            return;
        }

        context.ProjectImages.RemoveRange(project.Images);
        context.FloorPlanRooms.RemoveRange(project.FloorPlans.SelectMany(f => f.Rooms));
        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var image in BuildFerhundeHanimAptImages())
        {
            context.ProjectImages.Add(new ProjectImage
            {
                ProjectId = project.Id,
                ImagePath = image.ImagePath,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                Category = image.Category,
                Block = image.Block,
                ApartmentType = image.ApartmentType
            });
        }

        foreach (var floorPlan in BuildFerhundeHanimAptFloorPlans())
        {
            floorPlan.ProjectId = project.Id;
            context.FloorPlans.Add(floorPlan);
        }

        foreach (var sitePlanImage in BuildFerhundeHanimAptSitePlanImages())
        {
            sitePlanImage.ProjectId = project.Id;
            context.ProjectSitePlanImages.Add(sitePlanImage);
        }

        foreach (var conceptImage in BuildFerhundeHanimAptConceptImages())
        {
            conceptImage.ProjectId = project.Id;
            context.ProjectConceptImages.Add(conceptImage);
        }

        project.LocationImagePath = "/images/projects/ferhunde-hanim-apt/location.webp";
        project.CatalogueComingSoon = true;

        await context.SaveChangesAsync();
    }

    // Not a seed — corrects an already-seeded "Kuyulu AVM" row to the
    // client's real project name, "La Via AVM" (Consistency Revisions,
    // 2026-08-06), and to its new dedicated cover.webp (same Project Card
    // Thumbnail Quality fix as Davutlar D Latis/Ferhunde Hanım Apt. above).
    // Slug and every image path deliberately stay "kuyulu-avm" per the
    // client's explicit instruction not to rename folders or image paths —
    // only the displayed name/description move. Safe to run every startup:
    // a no-op once already migrated.
    private static async Task ReconcileLaViaAvmRenameAsync(AppDbContext context)
    {
        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Slug == "kuyulu-avm");

        if (project is null)
        {
            return;
        }

        var changed = false;

        if (project.Name != "La Via AVM")
        {
            project.Name = "La Via AVM";
            changed = true;
        }

        const string shortDescription = "Gölgelikli teraslar ve özenle tasarlanmış peyzajıyla alışverişi bir yaşam deneyimine dönüştüren, Aydın Efeler'de yükselen ticari proje.";
        if (project.ShortDescription != shortDescription)
        {
            project.ShortDescription = shortDescription;
            changed = true;
        }

        const string coverImage = "/images/projects/kuyulu-avm/cover.webp";
        if (project.CoverImage != coverImage)
        {
            project.CoverImage = coverImage;
            changed = true;
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    // Real exterior/interior photos (Alinda Gold Residence Gallery/Concept/
    // Floor Plans/Hero revision, 2026-08-10) — client-supplied drone/render
    // shots, moved from the flat wwwroot/images/projects/alinda-gold-
    // residence/{dis,ic}-mekan-gorselleri/ drop into this project's own
    // gallery/{category}/originals/ folder (see
    // ReconcileAlindaGoldResidenceRevisionAsync). Only two categories exist
    // in the client's folder structure — no per-apartment-type subfolders,
    // unlike Nysa Gold/La Fiore Karabağ 2. Etap — so this stays a flat
    // Exterior/Interior split, same as this project's Gallery dropdown
    // (Tüm Görseller / Dış Mekan Görselleri / İç Mekan Görselleri).
    private static List<ProjectImage> BuildAlindaGoldImages()
    {
        var images = new List<ProjectImage>();
        var order = 0;

        for (var i = 1; i <= 8; i++)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/alinda-gold/gallery/exterior/originals/exterior-{i:D2}.jpg",
                AltText = $"Alinda Gold Residence dış cephe görünümü {i}",
                DisplayOrder = order,
                Category = "Exterior"
            });
        }

        for (var i = 1; i <= 5; i++)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/alinda-gold/gallery/interior/originals/interior-{i:D2}.jpg",
                AltText = $"Alinda Gold Residence iç mekan görünümü {i}",
                DisplayOrder = order,
                Category = "Interior"
            });
        }

        return images;
    }

    // Concept section's 3-slide image carousel (Alinda Gold Residence
    // revision, 2026-08-10) — this project has no concept video, so it gets
    // the image-only carousel, same shared markup as La Fiore Karabağ 2.
    // Etap's (see _ProjectConcept.cshtml). Images are 3 of the 8 exterior
    // photos above, chosen for visual variety (a wide establishing dusk
    // shot, an aerial social/garden shot, an illuminated night entrance
    // shot) rather than arbitrarily. Copy is written specifically for this
    // project from what these three photos actually show — no invented
    // facilities, distances or figures, per the client's explicit
    // instruction not to fabricate project details.
    private static List<ProjectConceptImage> BuildAlindaGoldConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/alinda-gold/gallery/exterior/originals/exterior-04.jpg",
                Eyebrow = "Didim'in Yeni Silüeti",
                Title = "Gün Batımında Yükselen Kıvrımlı Mimari",
                Description = "Didim'in ufkunda yükselen Alinda Gold Residence, akıcı hatları ve zarif çatı aydınlatmasıyla şehrin siluetine yeni bir karakter katıyor. Alacakaranlıkta ışıldayan cepheleri, gündüzün enerjisini gecenin sakinliğiyle buluşturan bir yaşam deneyimi vaat ediyor.",
                DisplayOrder = 1
            },
            new()
            {
                ImagePath = "/images/projects/alinda-gold/gallery/exterior/originals/exterior-06.jpg",
                Eyebrow = "Bloklar Arasında Yeşil Bir Soluk",
                Title = "Peyzajla Bütünleşen Ortak Yaşam Alanları",
                Description = "Bloklar arasına özenle yerleştirilen havuz, yürüyüş yolları ve peyzaj alanları, Alinda Gold Residence sakinlerine güne açık havada başlama ve günü dinginlikle bitirme imkânı sunuyor. Yeşilin ve mimarinin bu uyumu, projeye günlük yaşamın ötesinde bir ayrıcalık katıyor.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/alinda-gold/gallery/exterior/originals/exterior-08.jpg",
                Eyebrow = "Işıkla Karşılanan Bir Giriş",
                Title = "Akşamın İçinde Zarif Bir Karşılama",
                Description = "Alinda Gold Residence'ın girişi, özenli aydınlatma tasarımı ve ferah kompozisyonuyla her akşam sakinlerini ve konuklarını şıklıkla karşılıyor. Bu ilk izlenim, projenin genelinde hissedilen premium yaklaşımın habercisi niteliğinde.",
                DisplayOrder = 3
            }
        };
    }

    // Floor Plans' one and only entry for this project (Alinda Gold
    // Residence revision, 2026-08-10) — deliberately NOT
    // BuildPlaceholderFloorPlans's fabricated "2+1"/68/95/78 figures/room
    // list per the client's explicit instruction not to invent apartment
    // types, drawings, room layouts or m² values. ApartmentType carries the
    // literal Coming Soon message instead of a real type badge; NetAreaM2/
    // GrossAreaM2/SalesGrossAreaM2 all stay at their 0 default so
    // ProjectsController.Details' hasAreaStats check omits the stats row
    // entirely (same mechanism as La Via AVM's "no invented figures" case —
    // see FloorPlanModel), Rooms is empty so no room list renders, and
    // ImagePath deliberately points at a file that does not exist so
    // _FloorPlans.cshtml's existing static "Kat planı görseli daha sonra
    // eklenecek." placeholder renders — no new markup needed anywhere.
    private static List<FloorPlan> BuildAlindaGoldFloorPlans()
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "Planlar yakında eklenecektir.",
                ImagePath = "/images/projects/alinda-gold/floorplan-placeholder.webp",
                NetAreaM2 = 0,
                GrossAreaM2 = 0,
                SalesGrossAreaM2 = 0,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>()
            }
        };
    }

    // TODO (Image Quality follow-up, 2026-08-10) — cover.webp and
    // banner.webp (the latter reused by both the Project Details Hero and
    // the Catalogue CTA banner, see _ProjectCatalogue.cshtml's
    // HeroBackgroundImageUrl reuse) are currently only 428×201 — every one
    // of the client's 13 supplied source photos
    // (gallery/{exterior,interior}/originals/*.jpg) is capped at 428px
    // wide, so this is the true resolution ceiling, not a pipeline bug.
    // ThumbnailTool's --single mode was already run at --width 1000/1920
    // for cover/banner respectively; ThumbnailGenerator's "never upscale"
    // rule silently capped both at the 428px source. Explicitly deferred
    // rather than upscaled per the client's instruction not to synthesize
    // detail (Image Quality Improvement request, 2026-08-10) — do not
    // "fix" this with a resize/sharpen filter on the existing 428px files.
    //
    // Once genuine high-resolution originals are supplied (2000px+ wide;
    // higher still for the Hero, which renders full-bleed at 1440px+ with
    // background-size: cover), regenerate all three from the new source:
    //   dotnet run --project tools/ThumbnailTool -- --single <new-source> \
    //     wwwroot/images/projects/alinda-gold/cover.webp --width 1000 --quality 88
    //   dotnet run --project tools/ThumbnailTool -- --single <new-source> \
    //     wwwroot/images/projects/alinda-gold/banner.webp --width 1920 --quality 88
    // (Catalogue CTA needs no separate file — it reuses banner.webp.) Also
    // rerun the plain walk mode (no args) to refresh the 13 gallery/
    // Concept-carousel originals' thumbnails from the new sources if the
    // client's new delivery also replaces those.

    // Not a seed — replaces the already-seeded Alinda Gold Residence row's
    // fabricated BuildPlaceholderFloorPlans entry with real data (Gallery/
    // Concept/Floor Plans/Hero revision, 2026-08-10): the client's supplied
    // exterior/interior photos (BuildAlindaGoldImages), a 3-slide Concept
    // carousel (BuildAlindaGoldConceptImages), a non-fabricated Floor Plans
    // Coming Soon entry (BuildAlindaGoldFloorPlans — see its own comment for
    // why this isn't BuildPlaceholderFloorPlans), and three Coming Soon
    // flags: CatalogueComingSoon (this project's katalog PDF is a stub —
    // "Bu dosya bir yer tutucudur" — same mechanism as La Fiore Karabağ 2.
    // Etap/Kuyulu AVM), CatalogueComingSoonHeroToast (this project also opts
    // the Hero's Katalog button into firing the toast immediately, unlike
    // those two projects — see Project.CatalogueComingSoonHeroToast), and
    // SitePlanComingSoon (no site plan supplied yet — see
    // Project.SitePlanComingSoon). Same RemoveRange-then-rebuild shape as
    // ReconcileFerhundeHanimAptRevisionAsync. Guarded on ConceptImages being
    // empty (only this reconcile ever populates it for this project, and it
    // never becomes empty again once seeded), so it runs exactly once and
    // never overwrites real edits made after it ran.
    private static async Task ReconcileAlindaGoldResidenceRevisionAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .Include(p => p.FloorPlans).ThenInclude(f => f.Rooms)
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "alinda-gold");

        if (project is null || project.ConceptImages.Count > 0)
        {
            return;
        }

        context.ProjectImages.RemoveRange(project.Images);
        context.FloorPlanRooms.RemoveRange(project.FloorPlans.SelectMany(f => f.Rooms));
        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var image in BuildAlindaGoldImages())
        {
            context.ProjectImages.Add(new ProjectImage
            {
                ProjectId = project.Id,
                ImagePath = image.ImagePath,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                Category = image.Category
            });
        }

        foreach (var floorPlan in BuildAlindaGoldFloorPlans())
        {
            floorPlan.ProjectId = project.Id;
            context.FloorPlans.Add(floorPlan);
        }

        foreach (var conceptImage in BuildAlindaGoldConceptImages())
        {
            conceptImage.ProjectId = project.Id;
            context.ProjectConceptImages.Add(conceptImage);
        }

        project.CatalogueComingSoon = true;
        project.CatalogueComingSoonHeroToast = true;
        project.SitePlanComingSoon = true;

        await context.SaveChangesAsync();
    }

    // Real exterior/interior photos (Magnesia Gold Residence Gallery/
    // Concept/Floor Plans/Hero revision, 2026-08-10) — same shape as
    // BuildAlindaGoldImages. The client's interior folder has no
    // apartment-type subfolders (just 13 flat numbered files), so — unlike
    // Nysa Gold/La Fiore Karabağ 2. Etap — this stays a flat Exterior/
    // Interior split with Block/ApartmentType left null; no Interior filter
    // chips render for this project, per the client's explicit instruction
    // not to invent sub-types where the folder structure doesn't have them.
    private static List<ProjectImage> BuildMagnesiaGoldImages()
    {
        var images = new List<ProjectImage>();
        var order = 0;

        for (var i = 1; i <= 8; i++)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/magnesia-gold/gallery/exterior/originals/exterior-{i:D2}.jpg",
                AltText = $"Magnesia Gold Residence dış cephe görünümü {i}",
                DisplayOrder = order,
                Category = "Exterior"
            });
        }

        for (var i = 1; i <= 13; i++)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/magnesia-gold/gallery/interior/originals/interior-{i:D2}.jpg",
                AltText = $"Magnesia Gold Residence iç mekan görünümü {i}",
                DisplayOrder = order,
                Category = "Interior"
            });
        }

        return images;
    }

    // Concept section's 3-slide image carousel (Magnesia Gold Residence
    // revision, 2026-08-10) — same shared carousel as Alinda Gold Residence/
    // La Fiore Karabağ 2. Etap. Images are 3 of the 8 exterior photos above,
    // chosen for visual variety (a wide panoramic establishing shot, an
    // illuminated night pool/fire-feature shot, a sunset garden/pool shot)
    // rather than arbitrarily. Copy is written specifically for this
    // project from what these three photos actually show — no invented
    // apartment counts, facilities, distances or figures, per the client's
    // explicit instruction not to fabricate project details.
    private static List<ProjectConceptImage> BuildMagnesiaGoldConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/magnesia-gold/gallery/exterior/originals/exterior-04.jpg",
                Eyebrow = "Aydın'ın Yükselen Silüeti",
                Title = "Birlikte Yükselen Kuleler, Ortak Bir Vizyon",
                Description = "Aydın'ın ufkunda yan yana yükselen Magnesia Gold Residence blokları, geniş peyzaj alanları ve özenle tasarlanmış ortak yaşam alanlarıyla bir aradalığı ön plana çıkarıyor. Alacakaranlıkta ışıldayan cepheler, projenin mimari bütünlüğünü güçlü bir şekilde ortaya koyuyor.",
                DisplayOrder = 1
            },
            new()
            {
                ImagePath = "/images/projects/magnesia-gold/gallery/exterior/originals/exterior-06.jpg",
                Eyebrow = "Gece Işıltısında Bir İmza",
                Title = "Işıkla Tanımlanan Sosyal Yaşam",
                Description = "Magnesia Gold Residence'ın havuz çevresi, akşam saatlerinde özenli aydınlatma tasarımıyla adeta bir sahneye dönüşüyor. Bu atmosfer, günün yorgunluğunu geride bırakıp sosyalleşmek isteyen sakinler için davetkâr bir buluşma noktası yaratıyor.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/magnesia-gold/gallery/exterior/originals/exterior-01.jpg",
                Eyebrow = "Doğayla Bütünleşen Bir Bahçe",
                Title = "Yeşilin İçinde Huzurlu Bir Mola",
                Description = "Palmiyelerle çevrili yürüyüş yolları, havuzlar ve geniş çim alanlarıyla Magnesia Gold Residence'ın peyzajı, günün her saatinde huzurlu bir mola sunuyor. Bu yeşil dokunun mimariyle kurduğu uyum, projenin yaşam kalitesini günlük deneyimin merkezine taşıyor.",
                DisplayOrder = 3
            }
        };
    }

    // Floor Plans' one and only entry for this project (Magnesia Gold
    // Residence revision, 2026-08-10) — same non-fabricated Coming Soon
    // pattern as BuildAlindaGoldFloorPlans (see its comment for the full
    // rationale): ApartmentType carries the literal Coming Soon message,
    // areas stay at 0 so the stats row is omitted, Rooms is empty, and
    // ImagePath points at a file that does not exist so the existing static
    // "Kat planı görseli daha sonra eklenecek." placeholder renders.
    private static List<FloorPlan> BuildMagnesiaGoldFloorPlans()
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "Planlar yakında eklenecektir.",
                ImagePath = "/images/projects/magnesia-gold/floorplan-placeholder.webp",
                NetAreaM2 = 0,
                GrossAreaM2 = 0,
                SalesGrossAreaM2 = 0,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>()
            }
        };
    }

    // TODO (Image Quality follow-up, 2026-08-10) — cover.webp and
    // banner.webp (the latter reused by both the Project Details Hero and
    // the Catalogue CTA banner) are currently only 428×201 — every one of
    // the client's 21 supplied source photos
    // (gallery/{exterior,interior}/originals/*.jpg) is capped at 428px
    // wide, same ceiling as Alinda Gold Residence (see that project's own
    // TODO above for the full rationale). Not upscaled, per the client's
    // explicit instruction not to synthesize detail. Once genuine
    // high-resolution originals are supplied (2000px+ wide; higher still
    // for the Hero), regenerate cover.webp/banner.webp the same way — see
    // the Alinda Gold Residence TODO for the exact ThumbnailTool commands.
    //
    // Not a seed — replaces the already-seeded Magnesia Gold Residence
    // row's fabricated BuildPlaceholderFloorPlans entry with real data
    // (Gallery/Concept/Floor Plans/Hero revision, 2026-08-10), same shape
    // as ReconcileAlindaGoldResidenceRevisionAsync: the client's supplied
    // exterior/interior photos (BuildMagnesiaGoldImages), a 3-slide Concept
    // carousel (BuildMagnesiaGoldConceptImages), a non-fabricated Floor
    // Plans Coming Soon entry (BuildMagnesiaGoldFloorPlans), and three
    // Coming Soon flags: CatalogueComingSoon (this project's katalog PDF is
    // a stub — "Bu dosya bir yer tutucudur"), CatalogueComingSoonHeroToast
    // (Hero's Katalog button also fires the toast immediately, same as
    // Alinda Gold Residence), and SitePlanComingSoon (no site plan supplied
    // yet). LocationImagePath is deliberately left unset — no location
    // photo was supplied for this project, so _ProjectLocation.cshtml keeps
    // falling back to the shared illustration exactly as before. Guarded on
    // ConceptImages being empty (only this reconcile ever populates it for
    // this project, and it never becomes empty again once seeded), so it
    // runs exactly once and never overwrites real edits made after it ran.
    private static async Task ReconcileMagnesiaGoldResidenceRevisionAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .Include(p => p.FloorPlans).ThenInclude(f => f.Rooms)
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "magnesia-gold");

        if (project is null || project.ConceptImages.Count > 0)
        {
            return;
        }

        context.ProjectImages.RemoveRange(project.Images);
        context.FloorPlanRooms.RemoveRange(project.FloorPlans.SelectMany(f => f.Rooms));
        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var image in BuildMagnesiaGoldImages())
        {
            context.ProjectImages.Add(new ProjectImage
            {
                ProjectId = project.Id,
                ImagePath = image.ImagePath,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                Category = image.Category
            });
        }

        foreach (var floorPlan in BuildMagnesiaGoldFloorPlans())
        {
            floorPlan.ProjectId = project.Id;
            context.FloorPlans.Add(floorPlan);
        }

        foreach (var conceptImage in BuildMagnesiaGoldConceptImages())
        {
            conceptImage.ProjectId = project.Id;
            context.ProjectConceptImages.Add(conceptImage);
        }

        project.CatalogueComingSoon = true;
        project.CatalogueComingSoonHeroToast = true;
        project.SitePlanComingSoon = true;

        await context.SaveChangesAsync();
    }

    // Real exterior/interior photos (Tralles Gold Residence Gallery/Concept/
    // Floor Plans/Hero revision, 2026-08-10) — same shape as
    // BuildMagnesiaGoldImages/BuildAlindaGoldImages. The client's supplied
    // folders (dis-mekan-gorselleri/ic-mekan-gorselleri) have no
    // apartment-type subfolders, so this stays a flat Exterior/Interior
    // split with Block/ApartmentType left null; no Interior filter chips
    // render for this project.
    private static List<ProjectImage> BuildTrallesGoldImages()
    {
        var images = new List<ProjectImage>();
        var order = 0;

        for (var i = 1; i <= 12; i++)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/tralles-gold/gallery/exterior/originals/exterior-{i:D2}.jpg",
                AltText = $"Tralles Gold Residence dış cephe görünümü {i}",
                DisplayOrder = order,
                Category = "Exterior"
            });
        }

        for (var i = 1; i <= 6; i++)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/tralles-gold/gallery/interior/originals/interior-{i:D2}.jpg",
                AltText = $"Tralles Gold Residence iç mekan görünümü {i}",
                DisplayOrder = order,
                Category = "Interior"
            });
        }

        return images;
    }

    // Concept section's 3-slide image carousel (Tralles Gold Residence
    // revision, 2026-08-10) — this project has no concept video, so it gets
    // the image-only carousel, same shared markup as Alinda Gold Residence/
    // Magnesia Gold Residence. Images are 3 of the 12 exterior photos above
    // (a wide daylight establishing aerial, a wide night aerial, and a
    // closer night facade detail), chosen for visual variety. Copy is
    // written specifically for this project from what these three photos
    // actually show plus the project's confirmed Location ("Aydın -
    // Efeler") — no invented facilities, apartment counts, distances or
    // figures.
    private static List<ProjectConceptImage> BuildTrallesGoldConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/tralles-gold/gallery/exterior/originals/exterior-01.jpg",
                Eyebrow = "Efeler'in Yükselen Üç Kulesi",
                Title = "Şehrin Kalbinde Yan Yana Yükselen Bloklar",
                Description = "Aydın Efeler'in dokusu içinde yan yana yükselen Tralles Gold Residence blokları, beyaz cepheleri ve dikey kırmızı vurgularıyla çevresinden hemen ayrışan güçlü bir mimari kimlik ortaya koyuyor. Gün ışığında çekilen bu kuşbakışı görünüm, projenin şehir dokusuyla kurduğu doğrudan bağlantıyı net bir şekilde gösteriyor.",
                DisplayOrder = 1
            },
            new()
            {
                ImagePath = "/images/projects/tralles-gold/gallery/exterior/originals/exterior-06.jpg",
                Eyebrow = "Gece Çöktüğünde Değişen Bir Atmosfer",
                Title = "Kırmızı Vurgularla Aydınlanan Bir Silüet",
                Description = "Akşam saatlerinde ışıklandırılan cepheleriyle Tralles Gold Residence, şehrin gece silüetinde kendine özgü bir karakter kazanıyor. Balkonları çevreleyen kırmızı hatlar, gündüzün beyaz sadeliğini gecenin canlılığıyla buluşturan görsel bir imza haline geliyor.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/tralles-gold/gallery/exterior/originals/exterior-09.jpg",
                Eyebrow = "Cepheye İşlenen Kırmızı Bir İmza",
                Title = "Detayda Ortaya Çıkan Mimari Karakter",
                Description = "Cepheyi boydan boya kat eden kırmızı dikey elemanlar, yakın çekimde Tralles Gold Residence'ın mimari kimliğini daha da belirgin kılıyor. Gece aydınlatmasıyla öne çıkan bu detay, projenin her katmanında hissedilen özenli tasarım yaklaşımını yansıtıyor.",
                DisplayOrder = 3
            }
        };
    }

    // Floor Plans' one and only entry for this project (Tralles Gold
    // Residence revision, 2026-08-10) — same non-fabricated Coming Soon
    // pattern as BuildAlindaGoldFloorPlans/BuildMagnesiaGoldFloorPlans (see
    // those comments for the full rationale). ApartmentType carries the
    // literal Coming Soon message, areas stay at 0 so the stats row is
    // omitted, Rooms is empty, and ImagePath deliberately points at a file
    // that does not exist so _FloorPlans.cshtml's existing static "Kat planı
    // görseli daha sonra eklenecek." placeholder renders. This project's
    // wwwroot folder also contains an orphaned floorplan-a.webp left over
    // from earlier scaffolding — deliberately NOT referenced here, since
    // there is no approved real floor plan for this project yet.
    private static List<FloorPlan> BuildTrallesGoldFloorPlans()
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "Planlar yakında eklenecektir.",
                ImagePath = "/images/projects/tralles-gold/floorplan-placeholder.webp",
                NetAreaM2 = 0,
                GrossAreaM2 = 0,
                SalesGrossAreaM2 = 0,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>()
            }
        };
    }

    // TODO (Image Quality follow-up, 2026-08-10) — cover.webp and
    // banner.webp are currently only 500×321 — every one of the client's 18
    // supplied source photos (gallery/{exterior,interior}/originals/*.jpg)
    // is capped between 428px and 500px wide (even lower than Alinda Gold
    // Residence/Magnesia Gold Residence's own 428px ceiling), so this is the
    // true resolution ceiling, not a pipeline bug. Not upscaled, per the
    // client's explicit instruction not to synthesize detail. Once genuine
    // high-resolution originals are supplied (2000px+ wide; higher still for
    // the Hero), regenerate cover.webp/banner.webp — see the Alinda Gold
    // Residence TODO above for the exact ThumbnailTool commands (use
    // exterior-03.jpg's replacement as the new source, the widest of this
    // project's 12 exterior photos today).
    //
    // Not a seed — replaces the already-seeded Tralles Gold Residence row's
    // fabricated single flat-file Images list and fabricated "3+1" floor
    // plan with real data, same shape as
    // ReconcileMagnesiaGoldResidenceRevisionAsync: the client's supplied
    // exterior/interior photos (BuildTrallesGoldImages), a 3-slide Concept
    // carousel (BuildTrallesGoldConceptImages), a non-fabricated Floor Plans
    // Coming Soon entry (BuildTrallesGoldFloorPlans), and three Coming Soon
    // flags: CatalogueComingSoon (this project's katalog PDF is a stub —
    // "Bu dosya bir yer tutucudur"), CatalogueComingSoonHeroToast (Hero's
    // Katalog button also fires the toast immediately, same as Alinda Gold
    // Residence/Magnesia Gold Residence), and SitePlanComingSoon (no site
    // plan supplied yet). Guarded on ConceptImages being empty, so it runs
    // exactly once and never overwrites real edits made after it ran.
    private static async Task ReconcileTrallesGoldResidenceRevisionAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .Include(p => p.FloorPlans).ThenInclude(f => f.Rooms)
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "tralles-gold");

        if (project is null || project.ConceptImages.Count > 0)
        {
            return;
        }

        context.ProjectImages.RemoveRange(project.Images);
        context.FloorPlanRooms.RemoveRange(project.FloorPlans.SelectMany(f => f.Rooms));
        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var image in BuildTrallesGoldImages())
        {
            context.ProjectImages.Add(new ProjectImage
            {
                ProjectId = project.Id,
                ImagePath = image.ImagePath,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                Category = image.Category
            });
        }

        foreach (var floorPlan in BuildTrallesGoldFloorPlans())
        {
            floorPlan.ProjectId = project.Id;
            context.FloorPlans.Add(floorPlan);
        }

        foreach (var conceptImage in BuildTrallesGoldConceptImages())
        {
            conceptImage.ProjectId = project.Id;
            context.ProjectConceptImages.Add(conceptImage);
        }

        project.CatalogueComingSoon = true;
        project.CatalogueComingSoonHeroToast = true;
        project.SitePlanComingSoon = true;

        await context.SaveChangesAsync();
    }

    // Real exterior photos only (Nlatis Gallery/Concept/Floor Plans/Hero
    // revision, 2026-08-10) — the client's supplied folder
    // (dis-mekan-gorselleri) has no interior photos and no apartment-type
    // subfolders, so this project gets a single "Exterior" category only;
    // its Gallery dropdown never gets an "İç Mekan Görselleri" entry (no
    // empty category is ever seeded — see GalleryCategorySortOrder/
    // GalleryCategoryLabels, which only surface categories actually present
    // on a project's images).
    private static List<ProjectImage> BuildNlatisImages()
    {
        var images = new List<ProjectImage>();
        var order = 0;

        for (var i = 1; i <= 10; i++)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/nlatis/gallery/exterior/originals/exterior-{i:D2}.jpg",
                AltText = $"Nlatis dış cephe görünümü {i}",
                DisplayOrder = order,
                Category = "Exterior"
            });
        }

        return images;
    }

    // Concept section's 3-slide image carousel (Nlatis revision,
    // 2026-08-10) — this project has no concept video, so it gets the
    // image-only carousel, same shared markup as Alinda Gold Residence/
    // Magnesia Gold Residence/Tralles Gold Residence. Images are 3 of the
    // 10 exterior renders above (a dusk elevation with the building's
    // curved wood-toned roofline lit from within, a daylight elevation
    // showing the same dark-panel/wood-accent facade rhythm, and the
    // project's one genuinely high-resolution source — a street-level
    // daylight render showing the ground-floor restaurant/social frontage),
    // chosen for visual variety. Copy is written specifically for this
    // project from what these three renders actually show plus the
    // project's confirmed Location ("Kuşadası") — no invented facilities,
    // apartment counts, distances or figures; the ground-floor restaurant
    // mentioned in slide 3 is described only because it is literally
    // visible/labelled in that render, not asserted as a confirmed resident
    // amenity.
    private static List<ProjectConceptImage> BuildNlatisConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/nlatis/gallery/exterior/originals/exterior-01.jpg",
                Eyebrow = "Kuşadası'nda Akşamın İçinde Bir Siluet",
                Title = "Kıvrımlı Çatı Hattıyla Tanımlanan Bir Karakter",
                Description = "Nlatis'in ahşap tonlu kıvrımlı çatı hattı, akşam saatlerinde içeriden yayılan ışıkla birlikte Kuşadası'nın silüetinde kendine özgü bir karakter kazanıyor. Camla kaplı cephesi, günün son ışıklarını binanın içine taşıyan davetkâr bir görünüm sunuyor.",
                DisplayOrder = 1
            },
            new()
            {
                ImagePath = "/images/projects/nlatis/gallery/exterior/originals/exterior-04.jpg",
                Eyebrow = "Koyu Tonlar ve Ahşabın Dengeli Uyumu",
                Title = "Cephede Ritmik Bir Malzeme Dili",
                Description = "Koyu renkli cephe panelleri ile ahşap tonlu dikey vurguların bir arada kullanıldığı Nlatis, cam balkonlarıyla katmanlı bir mimari ritim oluşturuyor. Önündeki yeşil peyzaj, binanın sade ve modern duruşunu yumuşak bir çerçeveyle tamamlıyor.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/nlatis/gallery/exterior/originals/exterior-07.jpg",
                Eyebrow = "Sokak Seviyesinde Yaşayan Bir Cephe",
                Title = "Zemin Kattan Başlayan Sosyal Yaşam",
                Description = "Zemin katta yer alan restoran ve sosyal kullanım alanları, Nlatis'in cephesini sokakla buluşturan canlı bir karşılama noktası oluşturuyor. Kıvrımlı çatı hattının gölgesinde uzanan bu bölüm, binanın günün her saatinde hareketli bir yüzü olduğunu gösteriyor.",
                DisplayOrder = 3
            }
        };
    }

    // Floor Plans' one and only entry for this project (Nlatis revision,
    // 2026-08-10) — same non-fabricated Coming Soon pattern as
    // BuildTrallesGoldFloorPlans (see its comment for the full rationale).
    // ApartmentType carries the literal Coming Soon message, areas stay at
    // 0 so the stats row is omitted, Rooms is empty, and ImagePath
    // deliberately points at a file that does not exist so
    // _FloorPlans.cshtml's existing static "Kat planı görseli daha sonra
    // eklenecek." placeholder renders.
    private static List<FloorPlan> BuildNlatisFloorPlans()
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "Planlar yakında eklenecektir.",
                ImagePath = "/images/projects/nlatis/floorplan-placeholder.webp",
                NetAreaM2 = 0,
                GrossAreaM2 = 0,
                SalesGrossAreaM2 = 0,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>()
            }
        };
    }

    // Note (Image Quality, 2026-08-10) — unlike every other project seeded
    // above, this project's cover.webp/banner.webp are NOT resolution-capped:
    // 9 of the client's 10 supplied exterior renders are ~700-940px wide,
    // but exterior-07.jpg is a genuine 4350×2698 source (the only truly
    // high-resolution asset supplied for either Tralles Gold Residence or
    // Nlatis in this revision), so cover.webp/banner.webp were generated
    // from it at the pipeline's normal 1000w/1920w targets with no capping.
    // The remaining 9 Gallery photos stay at their native ~700-940px width —
    // still not upscaled, per the client's explicit instruction, just not
    // the bottleneck for this project's Hero/Card imagery the way it is for
    // Tralles Gold Residence/Alinda Gold Residence/Magnesia Gold Residence.
    //
    // Not a seed — replaces the already-seeded Nlatis row's
    // BuildPlaceholderFloorPlans entry (fabricated "2+1"/68/95/78 figures)
    // and empty Images list with real data, same shape as
    // ReconcileTrallesGoldResidenceRevisionAsync: the client's supplied
    // exterior renders (BuildNlatisImages), a 3-slide Concept carousel
    // (BuildNlatisConceptImages), a non-fabricated Floor Plans Coming Soon
    // entry (BuildNlatisFloorPlans), and three Coming Soon flags:
    // CatalogueComingSoon (this project's katalog PDF is a stub — "Bu dosya
    // bir yer tutucudur"), CatalogueComingSoonHeroToast (Hero's Katalog
    // button also fires the toast immediately, same as Alinda Gold
    // Residence/Magnesia Gold Residence/Tralles Gold Residence), and
    // SitePlanComingSoon (no site plan supplied yet). Guarded on
    // ConceptImages being empty, so it runs exactly once and never
    // overwrites real edits made after it ran.
    private static async Task ReconcileNlatisRevisionAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .Include(p => p.FloorPlans).ThenInclude(f => f.Rooms)
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "nlatis");

        if (project is null || project.ConceptImages.Count > 0)
        {
            return;
        }

        context.ProjectImages.RemoveRange(project.Images);
        context.FloorPlanRooms.RemoveRange(project.FloorPlans.SelectMany(f => f.Rooms));
        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var image in BuildNlatisImages())
        {
            context.ProjectImages.Add(new ProjectImage
            {
                ProjectId = project.Id,
                ImagePath = image.ImagePath,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                Category = image.Category
            });
        }

        foreach (var floorPlan in BuildNlatisFloorPlans())
        {
            floorPlan.ProjectId = project.Id;
            context.FloorPlans.Add(floorPlan);
        }

        foreach (var conceptImage in BuildNlatisConceptImages())
        {
            conceptImage.ProjectId = project.Id;
            context.ProjectConceptImages.Add(conceptImage);
        }

        project.CatalogueComingSoon = true;
        project.CatalogueComingSoonHeroToast = true;
        project.SitePlanComingSoon = true;

        await context.SaveChangesAsync();
    }

    // Real exterior/interior renders (La Fiore Karabağ Gallery/Concept/Floor
    // Plans/Hero revision, 2026-08-10) — client-supplied renders, moved from
    // the flat wwwroot/images/projects/la-fiore-karabag-1-etap/{dis,ic}-
    // mekan-gorselleri/ drop into this project's own gallery/{category}/
    // originals/ folder (see ReconcileLaFioreKarabagRevisionAsync), same
    // slug-mismatch situation as La Fiore Karabağ 2. Etap's original
    // reorganization. Only two categories exist in the client's folder
    // structure — no per-apartment-type subfolders, same as Alinda Gold
    // Residence/Magnesia Gold Residence — so this stays a flat Exterior/
    // Interior split with Block/ApartmentType left null; no Interior filter
    // chips render for this project.
    private static List<ProjectImage> BuildLaFioreKarabagImages()
    {
        var images = new List<ProjectImage>();
        var order = 0;

        for (var i = 1; i <= 14; i++)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/la-fiore-karabag/gallery/exterior/originals/exterior-{i:D2}.{(i == 1 ? "png" : "jpg")}",
                AltText = $"La Fiore Karabağ dış cephe görünümü {i}",
                DisplayOrder = order,
                Category = "Exterior"
            });
        }

        for (var i = 1; i <= 20; i++)
        {
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/la-fiore-karabag/gallery/interior/originals/interior-{i:D2}.jpg",
                AltText = $"La Fiore Karabağ iç mekan görünümü {i}",
                DisplayOrder = order,
                Category = "Interior"
            });
        }

        // "Sosyal Alan" (Social Areas), client curation, 2026-08-20 — copies/
        // references 4 of the real Exterior renders above (same ImagePath, a
        // second ProjectImage row with a different Category, no file
        // duplication) so they also surface in the Gallery's Social Areas
        // filter and feed the Social Facilities cards.
        var socialAreaSourceIndexes = new[] { 3, 12, 13, 14 };
        var socialAreaIndex = 0;
        foreach (var sourceIndex in socialAreaSourceIndexes)
        {
            socialAreaIndex++;
            order++;
            images.Add(new ProjectImage
            {
                ImagePath = $"/images/projects/la-fiore-karabag/gallery/exterior/originals/exterior-{sourceIndex:D2}.jpg",
                AltText = $"La Fiore Karabağ sosyal alan görünümü {socialAreaIndex}",
                DisplayOrder = order,
                Category = "Social Areas"
            });
        }

        return images;
    }

    // Concept section's 3-slide image carousel (La Fiore Karabağ revision,
    // 2026-08-10) — this project has no concept video, so it gets the
    // image-only carousel, same shared markup as Alinda Gold Residence/
    // Magnesia Gold Residence. Images are 3 of the 14 exterior renders above,
    // chosen for visual variety (a wide aerial establishing shot of the
    // whole villa layout among the pines, a garden lounge terrace, and the
    // gated stone-clad entrance) rather than arbitrarily. Copy is written
    // specifically for this project from what these three renders actually
    // show — no invented facilities, distances or figures.
    private static List<ProjectConceptImage> BuildLaFioreKarabagConceptImages()
    {
        return new List<ProjectConceptImage>
        {
            new()
            {
                ImagePath = "/images/projects/la-fiore-karabag/gallery/exterior/originals/exterior-02.jpg",
                Eyebrow = "Didim'in Yeşil Dokusunda",
                Title = "Ormanla Bütünleşen Villa Yerleşimi",
                Description = "La Fiore Karabağ, gür çam ormanının içine özenle yerleştirilmiş tek katlı villalarıyla sakin ve mahremiyeti önceleyen bir yaşam alanı sunuyor. Yerleşim içindeki kesintisiz yürüyüş yolları ve peyzaj düzenlemesi, doğayla iç içe bir günlük yaşam deneyimi vaat ediyor.",
                DisplayOrder = 1
            },
            new()
            {
                ImagePath = "/images/projects/la-fiore-karabag/gallery/exterior/originals/exterior-03.jpg",
                Eyebrow = "Bahçede Geçen Akşamlar",
                Title = "Şömineli Terasta Açık Hava Konforu",
                Description = "Villa bahçesine kurulan pergola altındaki oturma grubu ve şömine, camla çevrili ferah salonlarla birleşerek gündüzü akşama, iç mekânı bahçeye bağlıyor. La Fiore Karabağ'da dış mekân, evin doğal bir uzantısı olarak tasarlandı.",
                DisplayOrder = 2
            },
            new()
            {
                ImagePath = "/images/projects/la-fiore-karabag/gallery/exterior/originals/exterior-05.jpg",
                Eyebrow = "Karşılamanın İlk Adımı",
                Title = "Taş ve Ahşabın Buluştuğu Güvenlikli Giriş",
                Description = "Taş kaplı cephesi ve ahşap detaylarıyla dikkat çeken La Fiore girişi, palmiyelerle çevrili yaklaşım yoluyla sakinlerini ve konuklarını karşılıyor. Güvenlikli giriş noktası, yerleşimin mahremiyetini ve huzurunu güvence altına alıyor.",
                DisplayOrder = 3
            }
        };
    }

    // Floor Plans' one and only entry for this project (La Fiore Karabağ
    // revision, 2026-08-10) — deliberately NOT BuildPlaceholderFloorPlans's
    // fabricated "2+1"/68/95/78 figures/room list, same reasoning as
    // BuildAlindaGoldFloorPlans/BuildNlatisFloorPlans (see either for the
    // full mechanism explanation): ApartmentType carries the literal Coming
    // Soon message, NetAreaM2/GrossAreaM2/SalesGrossAreaM2 all stay at their
    // 0 default so the stats row is omitted entirely, Rooms is empty so no
    // room list renders, and ImagePath deliberately points at a file that
    // does not exist so _FloorPlans.cshtml's existing static "Kat planı
    // görseli daha sonra eklenecek." placeholder renders.
    private static List<FloorPlan> BuildLaFioreKarabagFloorPlans()
    {
        return new List<FloorPlan>
        {
            new()
            {
                ApartmentType = "Planlar yakında eklenecektir.",
                ImagePath = "/images/projects/la-fiore-karabag/floorplan-placeholder.webp",
                NetAreaM2 = 0,
                GrossAreaM2 = 0,
                SalesGrossAreaM2 = 0,
                DisplayOrder = 1,
                Rooms = new List<FloorPlanRoom>()
            }
        };
    }

    // Note (Image Quality, 2026-08-10) — cover.webp/banner.webp (the latter
    // reused by both the Project Details Hero and the Catalogue CTA banner)
    // were generated from exterior-02.jpg, the best of the client's 14
    // supplied exterior renders at a native 1170×610 — better than Alinda
    // Gold Residence/Magnesia Gold Residence's 428px ceiling, but still well
    // short of genuine HD, so ThumbnailGenerator's "never upscale" rule caps
    // both outputs at 1170px wide (cover.webp lands at 999×521, under its
    // 1000w target; banner.webp is capped at the full 1170×610 source,
    // nowhere near its 1920w target). Explicitly deferred rather than
    // upscaled per the client's instruction not to synthesize detail — do
    // not "fix" this with a resize/sharpen filter on the existing source.
    // The remaining 13 exterior/20 interior Gallery renders stay at their
    // native resolution (428px wide for most) for the same reason.
    //
    // Once genuine high-resolution originals are supplied, regenerate all
    // three from the new source:
    //   dotnet run --project tools/ThumbnailTool -- --single <new-source> \
    //     wwwroot/images/projects/la-fiore-karabag/cover.webp --width 1000 --quality 88
    //   dotnet run --project tools/ThumbnailTool -- --single <new-source> \
    //     wwwroot/images/projects/la-fiore-karabag/banner.webp --width 1920 --quality 88
    // (Catalogue CTA needs no separate file — it reuses banner.webp.) Also
    // rerun the plain walk mode (no args) to refresh the 34 gallery/Concept-
    // carousel originals' thumbnails from the new sources if the client's new
    // delivery also replaces those.

    // Not a seed — replaces the already-seeded La Fiore Karabağ row's
    // fabricated BuildPlaceholderFloorPlans entry with real data (Gallery/
    // Concept/Floor Plans/Hero revision, 2026-08-10): the client's supplied
    // exterior/interior renders (BuildLaFioreKarabagImages), a 3-slide
    // Concept carousel (BuildLaFioreKarabagConceptImages), a non-fabricated
    // Floor Plans Coming Soon entry (BuildLaFioreKarabagFloorPlans), and
    // three Coming Soon flags: CatalogueComingSoon (this project's katalog
    // PDF is a stub — "Bu dosya bir yer tutucudur"), CatalogueComingSoonHeroToast
    // (Hero's Katalog button also fires the toast immediately, same as
    // Alinda Gold Residence/Magnesia Gold Residence/Tralles Gold Residence/
    // Nlatis), and SitePlanComingSoon (no site plan supplied yet). Same
    // RemoveRange-then-rebuild shape as ReconcileAlindaGoldResidenceRevisionAsync.
    // Guarded on ConceptImages being empty, so it runs exactly once and never
    // overwrites real edits made after it ran.
    private static async Task ReconcileLaFioreKarabagRevisionAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.Images)
            .Include(p => p.FloorPlans).ThenInclude(f => f.Rooms)
            .Include(p => p.ConceptImages)
            .FirstOrDefaultAsync(p => p.Slug == "la-fiore-karabag");

        if (project is null || project.ConceptImages.Count > 0)
        {
            return;
        }

        context.ProjectImages.RemoveRange(project.Images);
        context.FloorPlanRooms.RemoveRange(project.FloorPlans.SelectMany(f => f.Rooms));
        context.FloorPlans.RemoveRange(project.FloorPlans);

        foreach (var image in BuildLaFioreKarabagImages())
        {
            context.ProjectImages.Add(new ProjectImage
            {
                ProjectId = project.Id,
                ImagePath = image.ImagePath,
                AltText = image.AltText,
                DisplayOrder = image.DisplayOrder,
                Category = image.Category
            });
        }

        foreach (var floorPlan in BuildLaFioreKarabagFloorPlans())
        {
            floorPlan.ProjectId = project.Id;
            context.FloorPlans.Add(floorPlan);
        }

        foreach (var conceptImage in BuildLaFioreKarabagConceptImages())
        {
            conceptImage.ProjectId = project.Id;
            context.ProjectConceptImages.Add(conceptImage);
        }

        project.CatalogueComingSoon = true;
        project.CatalogueComingSoonHeroToast = true;
        project.SitePlanComingSoon = true;

        await context.SaveChangesAsync();
    }

    // Not a seed — removes Davutlar D Latis's already-seeded Vaziyet Planı
    // (Project Asset Audit, 2026-08-17): the kat-planlari/genel-planlar/
    // Zemin Kat drawing ReconcileDavutlarDLatisMediaAsync previously backfilled
    // here is a single floor's plan, not a true site/master plan, so the
    // client asked for the Hero's "Vaziyet Planı" button to stop rendering
    // for this project. Proje Kataloğu/Daire Planları/Videolar are untouched.
    // Safe to run every startup: a no-op once no SitePlanImages rows remain.
    private static async Task ReconcileDavutlarDLatisRemoveSitePlanAsync(AppDbContext context)
    {
        var project = await context.Projects
            .Include(p => p.SitePlanImages)
            .FirstOrDefaultAsync(p => p.Slug == "davutlar-d-latis");

        if (project is null || project.SitePlanImages.Count == 0)
        {
            return;
        }

        context.ProjectSitePlanImages.RemoveRange(project.SitePlanImages);
        await context.SaveChangesAsync();
    }

    // Not a seed — removes Ferhunde Hanım Apt.'s Proje Kataloğu entirely
    // (Project Asset Audit, 2026-08-17), per explicit client instruction:
    // no real catalogue exists for this project, and the client wants the
    // button/section gone rather than showing a "coming soon" toast.
    // Clearing CataloguePath makes CatalogueUrl null, which already hides
    // the Hero button and the lower Project Catalogue CTA section; clearing
    // CatalogueComingSoon (and its Hero-toast opt-in) too, since leaving it
    // true would otherwise still show a "coming soon" toast button inside
    // the Floor Plans panel — see _FloorPlans.cshtml. Vaziyet Planı/Daire
    // Planları are untouched. Safe to run every startup: a no-op once
    // CataloguePath is already null.
    private static async Task ReconcileFerhundeHanimAptRemoveCatalogueAsync(AppDbContext context)
    {
        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Slug == "ferhunde-hanim-apt");

        if (project is null || project.CataloguePath is null)
        {
            return;
        }

        project.CataloguePath = null;
        project.CatalogueComingSoon = false;
        project.CatalogueComingSoonHeroToast = false;
        await context.SaveChangesAsync();
    }

    // Not a seed — removes La Fiore Karabağ 2. Etap's Proje Kataloğu
    // entirely (Project Asset Audit, 2026-08-17), same reasoning and shape
    // as ReconcileFerhundeHanimAptRemoveCatalogueAsync above: no real
    // catalogue exists for this project, so the button/section is removed
    // rather than left showing a "coming soon" toast. Vaziyet Planı/Daire
    // Planları are untouched. Safe to run every startup: a no-op once
    // CataloguePath is already null.
    private static async Task ReconcileLaFioreKarabag2EtapRemoveCatalogueAsync(AppDbContext context)
    {
        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Slug == "la-fiore-karabag-2-etap");

        if (project is null || project.CataloguePath is null)
        {
            return;
        }

        project.CataloguePath = null;
        project.CatalogueComingSoon = false;
        project.CatalogueComingSoonHeroToast = false;
        await context.SaveChangesAsync();
    }

    // Not a seed — unpublishes "Kuyulu La Via AVM" (Project Asset Audit,
    // 2026-08-17), per explicit client instruction to remove the project
    // from the live site without deleting its row, images or Git history.
    // IsPublished is the application's existing editorial gate (see
    // IProjectQueryService) — every published-project surface (Projects
    // listing, Home page showcase, site search, and the /projects/{slug}
    // detail route) already filters on it, so flipping this one flag hides
    // the project everywhere with no other code change. Reversible by
    // flipping IsPublished back to true. Safe to run every startup: a
    // no-op once already unpublished.
    private static async Task ReconcileKuyuluAvmUnpublishAsync(AppDbContext context)
    {
        var project = await context.Projects
            .FirstOrDefaultAsync(p => p.Slug == "kuyulu-avm");

        if (project is null || !project.IsPublished)
        {
            return;
        }

        project.IsPublished = false;
        await context.SaveChangesAsync();
    }

    // Not a seed — removes Proje Kataloğu and Vaziyet Planı for a project
    // currently showing both as "Coming Soon" (Project Asset Audit
    // follow-up, 2026-08-17). Alinda Gold Residence, Magnesia Gold
    // Residence, Tralles Gold Residence, N Latis and Karabağ La Fiore 1.
    // Etap all got CatalogueComingSoon/CatalogueComingSoonHeroToast/
    // SitePlanComingSoon = true from their own ReconcileXxxRevisionAsync
    // method (2026-08-10); the client now wants both buttons/sections fully
    // removed for these five projects specifically, rather than shown as
    // "coming soon". Clearing CataloguePath makes CatalogueUrl null, which
    // already hides the Hero Katalog button, the Project Catalogue CTA
    // section and (since the Floor Plans panel's no-catalogue branch was
    // changed to render nothing) the Floor Plans panel's Katalog button
    // too. Daire Planları (Floor Plans) and the Concept image carousel
    // ("Videolar") are untouched — only these two flags/fields change.
    // Shared across the five projects (identical operation) rather than
    // five near-duplicate methods. Safe to run every startup: a no-op once
    // already applied.
    private static async Task ReconcileRemoveCatalogueAndSitePlanAsync(AppDbContext context, string slug)
    {
        var project = await context.Projects.FirstOrDefaultAsync(p => p.Slug == slug);

        if (project is null)
        {
            return;
        }

        var changed = false;

        if (project.CataloguePath is not null)
        {
            project.CataloguePath = null;
            project.CatalogueComingSoon = false;
            project.CatalogueComingSoonHeroToast = false;
            changed = true;
        }

        if (project.SitePlanComingSoon)
        {
            project.SitePlanComingSoon = false;
            changed = true;
        }

        if (changed)
        {
            await context.SaveChangesAsync();
        }
    }

    // Not a seed — removes Vaziyet Planı for Hacıfeyzullah Q-Latis (Project
    // Asset Audit follow-up, 2026-08-17): SitePlanComingSoon previously
    // showed the Hero's "Vaziyet Planı" button with a Coming Soon toast
    // (see BuildQLatisImages's seed comment); the client wants it removed
    // while Proje Kataloğu (real, downloadable), Daire Planları (already
    // empty — untouched) and the Concept video carousel ("Videolar") stay
    // exactly as they are. Safe to run every startup: a no-op once already
    // applied.
    private static async Task ReconcileQLatisRemoveSitePlanAsync(AppDbContext context)
    {
        var project = await context.Projects.FirstOrDefaultAsync(p => p.Slug == "q-latis");

        if (project is null || !project.SitePlanComingSoon)
        {
            return;
        }

        project.SitePlanComingSoon = false;
        await context.SaveChangesAsync();
    }

    // "Yakındaki Önemli Noktalar" location-advantage data (2026-08-20 client
    // request) — real driving distances, not the earlier per-project
    // guesses/placeholders. Each project's reference point is its mahalle
    // (the only location detail confirmed so far — no verified street
    // address exists yet for any of them), geocoded via OpenStreetMap
    // Nominatim and cross-checked against Wikipedia's neighbourhood
    // coordinates where available; every destination landmark below was
    // individually geocoded and, where a name from the client's brief could
    // not be verified to exist (e.g. no "İncirliova Devlet Hastanesi" is
    // findable in any source — İncirliova has no state hospital, only
    // Aile Sağlığı Merkezi clinics routed to Aydın's hospitals), swapped for
    // a verified nearby landmark instead per the brief's own fallback rule.
    // Distances are real routed driving distances (OSRM), not straight-line,
    // rounded to one decimal. Only the eight projects the client named are
    // covered here — La Via Villalar 1. Etap, D-Latis and Ferhunde Hanım
    // Apt. are deliberately absent; their addresses are still unverified.
    // Idempotent/safe to run every startup: a no-op once each project's
    // NearbyPlaces already matches this table exactly.
    private static readonly IReadOnlyDictionary<string, (string Name, string Distance)[]> NearbyPlacesResearch =
        new Dictionary<string, (string Name, string Distance)[]>
        {
            // Efeler / Zeybek — shared reference point with Magnesia Gold below.
            ["nysa-gold"] = new[]
            {
                ("Forum Aydın AVM", "5 km"),
                ("Aydın Atatürk Devlet Hastanesi", "2.3 km"),
                ("Aydın Şehir Hastanesi", "3.8 km"),
                ("Aydın Adnan Menderes Üniversitesi", "4.9 km"),
                ("Atatürk Kent Meydanı", "3.3 km"),
                ("Aydın Çıldır Havalimanı", "7.9 km")
            },
            // Efeler / Mimar Sinan.
            ["le-jardin"] = new[]
            {
                ("Forum Aydın AVM", "5.5 km"),
                ("Aydın Atatürk Devlet Hastanesi", "3.9 km"),
                ("Aydın Şehir Hastanesi", "4.9 km"),
                ("Aydın Adnan Menderes Üniversitesi", "5.7 km"),
                ("Atatürk Kent Meydanı", "4.5 km"),
                ("Aydın Çıldır Havalimanı", "9.5 km")
            },
            // Efeler / Adnan Menderes — shared reference point with Alinda
            // Gold below (same mahalle; no separate street address confirmed
            // for either project yet, so both resolve to the same distances).
            ["tralles-gold"] = new[]
            {
                ("Forum Aydın AVM", "2.9 km"),
                ("Aydın Atatürk Devlet Hastanesi", "1.5 km"),
                ("Aydın Şehir Hastanesi", "6.5 km"),
                ("Aydın Adnan Menderes Üniversitesi", "3.6 km"),
                ("Atatürk Kent Meydanı", "1.6 km"),
                ("Aydın Çıldır Havalimanı", "4.7 km")
            },
            // İzmir / Narlıdere. "Dokuz Eylül Üniversitesi" resolves to DEÜ's
            // Güzel Sanatlar Fakültesi campus, the actual DEÜ site inside
            // Narlıdere — its main Tınaztepe campus is a separate, much
            // farther location. "Narlıdere Metro" is the Kaymakamlık
            // terminal station (opened March 2024, confirmed via news
            // coverage), not the under-construction line the name could
            // otherwise ambiguously refer to.
            ["nlatis"] = new[]
            {
                ("Dokuz Eylül Üniversitesi (Güzel Sanatlar Fakültesi)", "2.1 km"),
                ("Narlıdere Metro İstasyonu", "1.1 km"),
                ("İzmir Ekonomi Üniversitesi", "4.5 km"),
                ("Dokuz Eylül Üniversitesi Hastanesi", "4 km"),
                ("İstinyePark İzmir", "6.5 km"),
                ("İnciraltı Kent Ormanı", "7.6 km")
            },
            // Efeler / Adnan Menderes — see tralles-gold above.
            ["alinda-gold"] = new[]
            {
                ("Forum Aydın AVM", "2.9 km"),
                ("Aydın Atatürk Devlet Hastanesi", "1.5 km"),
                ("Aydın Şehir Hastanesi", "6.5 km"),
                ("Aydın Adnan Menderes Üniversitesi", "3.6 km"),
                ("Atatürk Kent Meydanı", "1.6 km"),
                ("Aydın Çıldır Havalimanı", "4.7 km")
            },
            // Efeler / Zeybek — see nysa-gold above.
            ["magnesia-gold"] = new[]
            {
                ("Forum Aydın AVM", "5 km"),
                ("Aydın Atatürk Devlet Hastanesi", "2.3 km"),
                ("Aydın Şehir Hastanesi", "3.8 km"),
                ("Aydın Adnan Menderes Üniversitesi", "4.9 km"),
                ("Atatürk Kent Meydanı", "3.3 km"),
                ("Aydın Çıldır Havalimanı", "7.9 km")
            },
            // İncirliova / Karabağ — shared reference point (and identical
            // distances) with la-fiore-karabag-2-etap below, same village.
            // "İncirliova Devlet Hastanesi" replaced with "İncirliova Tren
            // İstasyonu": no state hospital exists in İncirliova in any
            // source checked (its residents are routed to Aydın's
            // hospitals, already covered by "Aydın Atatürk Devlet
            // Hastanesi" below); the railway station is a verifiable,
            // genuinely significant transportation landmark instead.
            ["la-fiore-karabag"] = new[]
            {
                ("İncirliova İlçe Merkezi", "4.2 km"),
                ("İncirliova Tren İstasyonu", "4.8 km"),
                ("Aydın Adnan Menderes Üniversitesi", "18.3 km"),
                ("Forum Aydın AVM", "17.5 km"),
                ("Aydın Atatürk Devlet Hastanesi", "14.9 km"),
                ("Aydın Çıldır Havalimanı", "20.4 km")
            },
            // İncirliova / Karabağ — see la-fiore-karabag above.
            ["la-fiore-karabag-2-etap"] = new[]
            {
                ("İncirliova İlçe Merkezi", "4.2 km"),
                ("İncirliova Tren İstasyonu", "4.8 km"),
                ("Aydın Adnan Menderes Üniversitesi", "18.3 km"),
                ("Forum Aydın AVM", "17.5 km"),
                ("Aydın Atatürk Devlet Hastanesi", "14.9 km"),
                ("Aydın Çıldır Havalimanı", "20.4 km")
            }
        };

    // Not a seed — backfills/corrects the "Yakındaki Önemli Noktalar"
    // (Location & Distances) section for the eight projects above with real
    // researched data; see NearbyPlacesResearch. Explicitly does not touch
    // any other project (including q-latis, kuyulu-avm, ferhunde-hanim-apt,
    // davutlar-d-latis and kuyulu-la-via-villalar-birinci-etap, none of
    // which appear in the table), so a project with no verified address
    // keeps whatever NearbyPlaces it already had — none, in every current
    // case. Safe to run every startup: a no-op per project once its
    // NearbyPlaces rows already match the table exactly.
    private static async Task ReconcileNearbyPlacesResearchAsync(AppDbContext context)
    {
        foreach (var (slug, desired) in NearbyPlacesResearch)
        {
            var project = await context.Projects
                .Include(p => p.NearbyPlaces)
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if (project is null)
            {
                continue;
            }

            var current = project.NearbyPlaces
                .OrderBy(place => place.DisplayOrder)
                .Select(place => (place.Name, place.Distance))
                .ToArray();

            if (current.SequenceEqual(desired))
            {
                continue;
            }

            context.ProjectNearbyPlaces.RemoveRange(project.NearbyPlaces);

            var displayOrder = 1;
            foreach (var (name, distance) in desired)
            {
                context.ProjectNearbyPlaces.Add(new ProjectNearbyPlace
                {
                    ProjectId = project.Id,
                    Name = name,
                    Distance = distance,
                    DisplayOrder = displayOrder++
                });
            }

            await context.SaveChangesAsync();
        }
    }

    // Gallery client curation (2026-08-20) — fixes already-seeded databases
    // to match the Build*Images()/Amenities edits above (those alone only
    // affect a project's very first insert; an existing database's rows
    // were seeded long before this change and never get replayed). Each
    // project's block below is guarded on its own "Social Areas" category
    // not existing yet, so this only ever runs once per project — safe to
    // call on every startup, including a freshly-seeded database (whose
    // Build*Images() output already has no "Social Areas" rows to touch
    // here, since it was built curated from the start). Archived images are
    // only ever removed from ProjectImages (the row Gallery actually
    // queries) — their files stay on disk untouched, matching the
    // commented-out seed rows left in the Build*Images() methods above for
    // restoration reference.
    private static async Task ReconcileGalleryCurationAsync(AppDbContext context)
    {
        async Task RemoveExcept(int projectId, string category, IReadOnlySet<string> keepImagePaths)
        {
            var toRemove = await context.ProjectImages
                .Where(i => i.ProjectId == projectId && i.Category == category && !keepImagePaths.Contains(i.ImagePath))
                .ToListAsync();
            context.ProjectImages.RemoveRange(toRemove);
        }

        async Task RemoveByPath(int projectId, string imagePath)
        {
            var toRemove = await context.ProjectImages
                .Where(i => i.ProjectId == projectId && i.ImagePath == imagePath)
                .ToListAsync();
            context.ProjectImages.RemoveRange(toRemove);
        }

        async Task RemoveByVideoPath(int projectId, string videoPath)
        {
            var toRemove = await context.ProjectImages
                .Where(i => i.ProjectId == projectId && i.VideoPath == videoPath)
                .ToListAsync();
            context.ProjectImages.RemoveRange(toRemove);
        }

        void AddSocialAreas(int projectId, IEnumerable<(string ImagePath, string AltText, int DisplayOrder)> rows)
        {
            foreach (var row in rows)
            {
                context.ProjectImages.Add(new ProjectImage
                {
                    ProjectId = projectId,
                    ImagePath = row.ImagePath,
                    AltText = row.AltText,
                    DisplayOrder = row.DisplayOrder,
                    Category = "Social Areas"
                });
            }
        }

        // ---- Nysa Gold ----
        {
            var project = await context.Projects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Slug == "nysa-gold");

            if (project is not null && !project.Images.Any(i => i.Category == "Social Areas"))
            {
                var exteriorKeep = new[] { 3, 5, 25, 41, 42, 43 };
                var exteriorJpeg = new HashSet<int> { 3, 5, 9, 55, 56, 57, 58, 59, 60 };
                var keepExteriorPaths = exteriorKeep
                    .Select(i => $"/images/projects/nysa-gold/gallery/exterior/originals/exterior-{i:D2}.{(exteriorJpeg.Contains(i) ? "jpeg" : "jpg")}")
                    .ToHashSet();
                await RemoveExcept(project.Id, "Exterior", keepExteriorPaths);

                string InteriorPath(string folder, int index) =>
                    $"/images/projects/nysa-gold/gallery/interior/{folder}/originals/interior-{index:D2}.jpg";

                async Task RemoveBlockExcept(string block, string folder, int[] keepIndexes)
                {
                    var keepPaths = keepIndexes.Select(i => InteriorPath(folder, i)).ToHashSet();
                    var toRemove = await context.ProjectImages
                        .Where(i => i.ProjectId == project.Id && i.Block == block && !keepPaths.Contains(i.ImagePath))
                        .ToListAsync();
                    context.ProjectImages.RemoveRange(toRemove);
                }

                await RemoveBlockExcept("2+1 A Tipi", "iki-arti-bir-a-tipi",
                    new[] { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 41 });
                await RemoveBlockExcept("2+1 C Tipi", "iki-arti-bir-c-tipi", new[] { 2, 3 });
                await RemoveBlockExcept("2+1 E Tipi", "iki-arti-bir-e-tipi", new[] { 4, 13 });
                await RemoveBlockExcept("3+1 B Tipi", "uc-arti-bir-b-tipi", new[] { 3, 4, 12, 22 });

                await RemoveByPath(project.Id, InteriorPath("dort-arti-bir-tipi", 2));
                await RemoveByVideoPath(project.Id, "/images/projects/nysa-gold/gallery/interior/dort-arti-bir-tipi/videos/interior-video-02.mp4");

                var socialSources = new[] { 12, 14, 17, 23, 24, 26, 27, 29, 34, 36, 45, 56, 57, 58 };
                AddSocialAreas(project.Id, socialSources.Select((src, idx) =>
                {
                    var ext = exteriorJpeg.Contains(src) ? "jpeg" : "jpg";
                    return (
                        $"/images/projects/nysa-gold/gallery/exterior/originals/exterior-{src:D2}.{ext}",
                        $"Nysa Gold Residence sosyal alan görünümü {idx + 1}",
                        327 + idx
                    );
                }));

                if (!string.IsNullOrWhiteSpace(project.Amenities) && !project.Amenities.Contains("Pet Parkı"))
                {
                    project.Amenities += "\nAydın'ın İlk Pet Parkı";
                }
            }
        }

        // ---- Le Jardin ----
        {
            var project = await context.Projects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Slug == "le-jardin");

            if (project is not null && !project.Images.Any(i => i.Category == "Social Areas"))
            {
                var keepExteriorPaths = new[] { 1, 5, 23, 24, 28 }
                    .Select(i => $"/images/projects/le-jardin/gallery/exterior/originals/exterior-{i:D2}.jpg")
                    .ToHashSet();
                await RemoveExcept(project.Id, "Exterior", keepExteriorPaths);
                await RemoveByPath(project.Id, "/images/projects/le-jardin/gallery/interior/zemin-kat/originals/salon-mutfak-08.jpg");

                var socialSources = new[] { 15, 16, 17, 18, 20, 21, 22, 26, 30 };
                AddSocialAreas(project.Id, socialSources.Select((src, idx) => (
                    $"/images/projects/le-jardin/gallery/exterior/originals/exterior-{src:D2}.jpg",
                    $"Le Jardin sosyal alan görünümü {idx + 1}",
                    81 + idx
                )));
            }
        }

        // ---- La Fiore Karabağ 1. Etap ----
        {
            var project = await context.Projects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Slug == "la-fiore-karabag");

            if (project is not null && !project.Images.Any(i => i.Category == "Social Areas"))
            {
                var socialSources = new[] { 3, 12, 13, 14 };
                AddSocialAreas(project.Id, socialSources.Select((src, idx) => (
                    $"/images/projects/la-fiore-karabag/gallery/exterior/originals/exterior-{src:D2}.jpg",
                    $"La Fiore Karabağ sosyal alan görünümü {idx + 1}",
                    35 + idx
                )));
            }
        }

        // ---- La Fiore Karabağ 2. Etap ----
        {
            var project = await context.Projects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Slug == "la-fiore-karabag-2-etap");

            if (project is not null && !project.Images.Any(i => i.Category == "Social Areas"))
            {
                var blockSources = new (string Folder, string File)[]
                {
                    ("c-tipi-blok", "3.jpeg"), ("c-tipi-blok", "3a.jpg"), ("c-tipi-blok", "3b.jpeg"),
                    ("d-tipi-blok", "2.jpeg"),
                    ("e-tipi-blok", "1.jpeg"), ("e-tipi-blok", "1a.jpeg"), ("e-tipi-blok", "3.jpeg"),
                    ("e-tipi-blok", "3a.jpeg"), ("e-tipi-blok", "3b.jpeg"),
                    ("f-tipi-blok", "1.jpeg"), ("f-tipi-blok", "1a.jpeg"), ("f-tipi-blok", "2.jpeg"),
                    ("g-tipi-blok", "1.jpeg"), ("g-tipi-blok", "1a.jpeg"), ("g-tipi-blok", "9.jpeg"), ("g-tipi-blok", "9a.jpeg")
                };
                var allExteriorSources = new[] { "35.jpeg", "36.jpeg", "37.jpeg" };

                var rows = blockSources
                    .Select((b, idx) => (
                        $"/images/projects/la-fiore-karabag-2-etap/gallery/exterior/{b.Folder}/originals/{b.File}",
                        $"La Fiore Karabağ 2. Etap sosyal alan görünümü {idx + 1}",
                        192 + idx
                    ))
                    .Concat(allExteriorSources.Select((file, idx) => (
                        $"/images/projects/la-fiore-karabag-2-etap/gallery/all-exterior/originals/{file}",
                        $"La Fiore Karabağ 2. Etap sosyal alan görünümü {blockSources.Length + idx + 1}",
                        192 + blockSources.Length + idx
                    )));

                AddSocialAreas(project.Id, rows);
            }
        }

        // ---- La Via Villalar 1. Etap ----
        {
            var project = await context.Projects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Slug == "kuyulu-la-via-villalar-birinci-etap");

            if (project is not null && !project.Images.Any(i => i.Category == "Social Areas"))
            {
                var socialSources = new[] { "02.jpg", "03.jpg", "04.jpg", "07.jpg", "09.jpg" };
                AddSocialAreas(project.Id, socialSources.Select((file, idx) => (
                    $"/images/projects/kuyulu-la-via-villalar-birinci-etap/gallery/exterior/originals/{file}",
                    $"La Via Villalar 1. Etap sosyal alan görünümü {idx + 1}",
                    92 + idx
                )));
            }
        }

        // ---- Ferhunde Hanım Apt. ----
        {
            var project = await context.Projects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Slug == "ferhunde-hanim-apt");

            if (project is not null && !project.Images.Any(i => i.Category == "Social Areas"))
            {
                var socialSources = new[] { 13, 16, 17, 18, 19 };
                AddSocialAreas(project.Id, socialSources.Select((src, idx) => (
                    $"/images/projects/ferhunde-hanim-apt/gallery/exterior/originals/exterior-{src:D2}.jpg",
                    $"Ferhunde Hanım Apt. sosyal alan görünümü {idx + 1}",
                    68 + idx
                )));
            }
        }

        await context.SaveChangesAsync();
    }
}
