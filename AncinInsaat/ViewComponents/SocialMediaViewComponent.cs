using Microsoft.AspNetCore.Mvc;

namespace AncinInsaat.ViewComponents;

public class SocialMediaViewComponent : ViewComponent
{
    // PLACEHOLDER — real handles are not yet available. Replace with the
    // client's actual profile URLs before launch (see CLAUDE.md Placeholder
    // Content rules). Structured as a fixed list rather than a database
    // read because Social Media Links are out of scope for the Admin Panel
    // until Milestone 3+ (08_DatabasePlan.md).
    private static readonly IReadOnlyList<SocialMediaLink> PlaceholderLinks = new List<SocialMediaLink>
    {
        new()
        {
            Platform = "Instagram",
            Url = "https://www.instagram.com/ancininsaat",
            IconMarkup = """
                <rect x="3" y="3" width="18" height="18" rx="5" fill="none" stroke="currentColor" stroke-width="1.6" />
                <circle cx="12" cy="12" r="4.2" fill="none" stroke="currentColor" stroke-width="1.6" />
                <circle cx="17.2" cy="6.8" r="1.1" fill="currentColor" />
                """
        },
        new()
        {
            Platform = "Facebook",
            Url = "https://www.facebook.com/ancininsaat",
            IconMarkup = """
                <path d="M14.5 21v-7.5h2.5l.4-3H14.5V8.3c0-.9.3-1.5 1.6-1.5h1.7V4.1C17.5 4 16.4 4 15.2 4c-2.5 0-4.2 1.5-4.2 4.3v2.2H8.5v3H11V21h3.5z" fill="currentColor" />
                """
        },
        new()
        {
            Platform = "LinkedIn",
            Url = "https://www.linkedin.com/company/ancininsaat",
            IconMarkup = """
                <rect x="3" y="3" width="18" height="18" rx="3" fill="none" stroke="currentColor" stroke-width="1.6" />
                <circle cx="7.8" cy="8.3" r="1.3" fill="currentColor" />
                <path d="M6.7 11h2.2v7H6.7z" fill="currentColor" />
                <path d="M11.3 11h2.1v1c.5-.8 1.3-1.2 2.3-1.2 1.9 0 3 1.2 3 3.6V18h-2.2v-3.2c0-1-.4-1.7-1.4-1.7-.8 0-1.5.6-1.5 1.7V18h-2.3z" fill="currentColor" />
                """
        },
        new()
        {
            Platform = "YouTube",
            Url = "https://www.youtube.com/@ancininsaat",
            IconMarkup = """
                <rect x="3" y="5.5" width="18" height="13" rx="4" fill="none" stroke="currentColor" stroke-width="1.6" />
                <path d="M10.2 9.3v5.4l4.8-2.7z" fill="currentColor" />
                """
        }
    };

    // onLight: true on light page sections, false (default) inside the near-black Footer.
    public IViewComponentResult Invoke(bool onLight = false)
    {
        var model = new SocialMediaViewModel { Links = PlaceholderLinks, OnLight = onLight };
        return View(model);
    }
}
