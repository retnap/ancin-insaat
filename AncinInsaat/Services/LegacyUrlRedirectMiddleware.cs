namespace AncinInsaat.Services;

// Pre-launch SEO (docs/10_SEO.md "Redirects": permanent 301s when URLs
// change) — the previous static ancin.com.tr served flat *.html pages that
// are still indexed by Google. Each known old page is mapped explicitly to
// its new route; any other *.html request falls back to the homepage
// rather than a 404 — the English homepage (/en) when the path is under
// /en/, the Turkish one (/) otherwise. Matching is case-insensitive on the
// path only, so a query string never affects the target (and is not
// carried over).
// Real files under wwwroot (e.g. a future search-console verification
// .html) are left to UseStaticFiles untouched. Extension-less routes —
// every current route, including all /en ones — never reach the map.
public class LegacyUrlRedirectMiddleware
{
    private const string FallbackTarget = "/";
    private const string EnglishFallbackTarget = "/en";

    private static readonly Dictionary<string, string> Redirects = new(StringComparer.OrdinalIgnoreCase)
    {
        ["/index.html"] = "/",
        ["/hakkimizda.html"] = "/about-us",
        ["/iletisim.html"] = "/contact",
        ["/tamamlanan.html"] = "/projects",
        ["/nysagold.html"] = "/projects/nysa-gold",
        ["/lejardin.html"] = "/projects/le-jardin",
        ["/lafiore.html"] = "/projects/la-fiore-karabag",
        ["/lafiore02.html"] = "/projects/la-fiore-karabag-2-etap",
        ["/ferhunde.html"] = "/projects/ferhunde-hanim-apt",
        ["/trallesgold.html"] = "/projects/tralles-gold",
        ["/alindagold.html"] = "/projects/alinda-gold",
        ["/magnesiagold.html"] = "/projects/magnesia-gold",
        ["/nlatis.html"] = "/projects/nlatis",
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
        var path = context.Request.Path.Value;

        if (string.IsNullOrEmpty(path)
            || !path.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
            || !(HttpMethods.IsGet(context.Request.Method) || HttpMethods.IsHead(context.Request.Method))
            || _environment.WebRootFileProvider.GetFileInfo(path).Exists)
        {
            return _next(context);
        }

        var target = Redirects.TryGetValue(path, out var mapped)
            ? mapped
            : path.StartsWith("/en/", StringComparison.OrdinalIgnoreCase) ? EnglishFallbackTarget : FallbackTarget;
        context.Response.Redirect(target, permanent: true);
        return Task.CompletedTask;
    }
}
