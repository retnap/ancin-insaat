namespace AncinInsaat.Services;

// Projects whose public Project Detail page (/projects/{slug} and
// /en/projects/{slug}) is temporarily switched off (client request,
// 2026-10-19): Davutlar D Latis and Hacıfeyzullah - Q-Latis. Their database
// rows, assets and IsPublished/IsFutureProject flags are left exactly as
// they are — they still appear on the Projects listing as "Yakında"
// coming-soon cards (which never link anywhere) — but a direct request for
// their detail URL returns 404 and no generated link (search results, Home
// showcase, Home hero CTA) points at it.
//
// To make a project's detail page available again, remove its slug from
// this set. That alone re-enables the route and every link generator
// below; whether its listing card links to the page is still decided by
// Project.IsFutureProject, exactly as before.
//
// Single source of truth, read by ProjectsController (Details/Index),
// ProjectSearchProvider, ProjectsShowcaseViewComponent and
// HeroBannerViewComponent. A future Admin Panel can replace this set with
// a per-project database flag without changing those call sites.
public static class ProjectDetailAvailability
{
    private static readonly IReadOnlySet<string> DisabledDetailPageSlugs =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "davutlar-d-latis",
            "q-latis"
        };

    public static bool IsDetailPageAvailable(string slug) =>
        !string.IsNullOrWhiteSpace(slug) && !DisabledDetailPageSlugs.Contains(slug);
}
