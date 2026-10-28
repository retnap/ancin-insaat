namespace AncinInsaat.Services;

// Pre-launch SEO (docs/10_SEO.md "Redirects": permanent 301s when URLs
// change) — the previous static ancin.com.tr served flat *.html pages that
// are still indexed by Google. Each known old page is mapped explicitly to
// its new route; any other *.html request falls back to the homepage
// rather than a 404 — the English homepage (/en) when the path is under
// /en/, the Turkish one (/) otherwise. Matching is case-insensitive on the
// path only, so a query string never affects the target (and is not
// carried over); a trailing slash is ignored.
// Real files under wwwroot (e.g. a future search-console verification
// .html) are left to UseStaticFiles untouched.
//
// 2026-10-28 additions:
//   - www.ancin.com.tr is answered first with a 301 to the same path and
//     query on https://ancin.com.tr (SiteUrls), so a www legacy URL takes
//     two hops: www → apex → new route. Any other host (localhost, the
//     server IP) is never redirected.
//   - A few extension-less pages of the previous site are mapped too, and
//     any other "/proje-..." page (an even older site's project URLs) falls
//     back to /projects. "/projects" and "/projects/{slug}" never match that
//     prefix, and every other extension-less route still passes through.
public class LegacyUrlRedirectMiddleware
{
    private const string FallbackTarget = "/";
    private const string EnglishFallbackTarget = "/en";
    private const string OldProjectPrefix = "/proje-";
    private const string OldProjectFallbackTarget = "/projects";

    private static readonly Dictionary<string, string> Redirects = new(StringComparer.OrdinalIgnoreCase)
    {
        ["/index.html"] = "/",
        ["/hakkimizda.html"] = "/about-us",
        ["/iletisim.html"] = "/contact",
        ["/tamamlanan.html"] = "/projects",
        ["/devameden.html"] = "/projects",
        ["/nysagold.html"] = "/projects/nysa-gold",
        ["/lejardin.html"] = "/projects/le-jardin",
        ["/lafiore.html"] = "/projects/la-fiore-karabag",
        ["/lafiore02.html"] = "/projects/la-fiore-karabag-2-etap",
        ["/ferhunde.html"] = "/projects/ferhunde-hanim-apt",
        ["/trallesgold.html"] = "/projects/tralles-gold",
        ["/alindagold.html"] = "/projects/alinda-gold",
        ["/magnesiagold.html"] = "/projects/magnesia-gold",
        ["/nlatis.html"] = "/projects/nlatis",
        ["/bize-ulasin"] = "/contact",
        ["/on-talep-formu"] = "/contact",
        ["/proje-tralles-gold-residence-41"] = "/projects/tralles-gold",
        ["/proje-nlatis-narlidere-luxury-loft-44"] = "/projects/nlatis",
    };

    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environment;

    public LegacyUrlRedirectMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }

    public Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        if (string.Equals(request.Host.Host, SiteUrls.WwwHost, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Redirect(SiteUrls.CanonicalBaseUrl + request.PathBase + request.Path + request.QueryString, permanent: true);
            return Task.CompletedTask;
        }

        var path = request.Path.Value;

        if (string.IsNullOrEmpty(path)
            || !(HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method)))
        {
            return _next(context);
        }

        if (path.Length > 1 && path.EndsWith('/'))
        {
            path = path.TrimEnd('/');
        }

        var target = ResolveTarget(path);
        if (target is null || _environment.WebRootFileProvider.GetFileInfo(path).Exists)
        {
            return _next(context);
        }

        context.Response.Redirect(target, permanent: true);
        return Task.CompletedTask;
    }

    private static string? ResolveTarget(string path)
    {
        if (Redirects.TryGetValue(path, out var mapped))
        {
            return mapped;
        }

        if (path.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
        {
            return path.StartsWith("/en/", StringComparison.OrdinalIgnoreCase) ? EnglishFallbackTarget : FallbackTarget;
        }

        return path.StartsWith(OldProjectPrefix, StringComparison.OrdinalIgnoreCase) ? OldProjectFallbackTarget : null;
    }
}
