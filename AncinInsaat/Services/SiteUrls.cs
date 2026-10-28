namespace AncinInsaat.Services;

// The site's one public address (2026-10-28) — canonical URLs, Open Graph
// image URLs and JSON-LD always use this, never the request's own
// scheme/host, so a visit via www.ancin.com.tr, plain http or the server's
// IP address can't leak a second address into search results.
// LegacyUrlRedirectMiddleware sends www.ancin.com.tr here with a 301.
public static class SiteUrls
{
    public const string CanonicalHost = "ancin.com.tr";
    public const string WwwHost = "www.ancin.com.tr";
    public const string CanonicalBaseUrl = "https://" + CanonicalHost;
}
