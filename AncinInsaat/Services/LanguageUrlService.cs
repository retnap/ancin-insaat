using Microsoft.AspNetCore.Http;

namespace AncinInsaat.Services;

/// <summary>
/// Single source of truth for the TR ⇄ EN URL mapping (English
/// localization, 2026-10-02). Every Turkish route keeps its exact existing
/// path — nothing here ever rewrites a Turkish URL, so current links/SEO
/// are untouched — and the entire English site lives under an "/en"
/// prefix. Only "sosyal-sorumluluk-projelerimiz" needs a translated first
/// segment (its English page is "social-responsibility"); every other
/// top-level path (projects, about-us, career, contact, values, hr-policy,
/// kvkk) is already English-spelled for the Turkish site and keeps the
/// identical segment under "/en". Project slugs are never translated —
/// brand/project names stay identical in both languages (e.g.
/// /projects/nysa-gold ⇄ /en/projects/nysa-gold).
///
/// This is the one place that understands the TR/EN route shape, used by
/// the navbar's language switch, RouteSegmentRequestCultureProvider and
/// _Layout's hreflang tags — so no other file needs its own "is this an
/// /en/ path?" check.
/// </summary>
public static class LanguageUrlService
{
    public const string EnglishPrefixSegment = "en";

    // Keyed by the Turkish first-path-segment; only entries whose wording
    // actually changes need listing here — every other top-level path
    // passes through unchanged under the "/en" prefix.
    private static readonly Dictionary<string, string> TurkishToEnglishSegment =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["sosyal-sorumluluk-projelerimiz"] = "social-responsibility",
        };

    private static readonly Dictionary<string, string> EnglishToTurkishSegment =
        TurkishToEnglishSegment.ToDictionary(
            pair => pair.Value,
            pair => pair.Key,
            StringComparer.OrdinalIgnoreCase);

    public static bool IsEnglishPath(PathString path)
    {
        var segments = Segments(path);
        return segments.Length > 0 && segments[0].Equals(EnglishPrefixSegment, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Given any current request path (Turkish or English), returns the
    /// equivalent English path — preserving everything after the
    /// first/translated segment (project slug, trailing segments) and the
    /// query string untouched.
    /// </summary>
    public static string GetEnglishEquivalent(PathString path, QueryString query)
    {
        var segments = Segments(path).ToList();

        if (segments.Count > 0 && segments[0].Equals(EnglishPrefixSegment, StringComparison.OrdinalIgnoreCase))
        {
            return Rebuild(segments, query);
        }

        if (segments.Count > 0 && TurkishToEnglishSegment.TryGetValue(segments[0], out var translated))
        {
            segments[0] = translated;
        }

        segments.Insert(0, EnglishPrefixSegment);
        return Rebuild(segments, query);
    }

    /// <summary>
    /// Given any current request path (Turkish or English), returns the
    /// equivalent Turkish path — the exact-unchanged path when it is
    /// already Turkish, or the "/en" prefix (and translated segment, where
    /// one exists) stripped back out.
    /// </summary>
    public static string GetTurkishEquivalent(PathString path, QueryString query)
    {
        var segments = Segments(path).ToList();

        if (segments.Count == 0 || !segments[0].Equals(EnglishPrefixSegment, StringComparison.OrdinalIgnoreCase))
        {
            return Rebuild(segments, query);
        }

        segments.RemoveAt(0);

        if (segments.Count > 0 && EnglishToTurkishSegment.TryGetValue(segments[0], out var translated))
        {
            segments[0] = translated;
        }

        return Rebuild(segments, query);
    }

    private static string Rebuild(List<string> segments, QueryString query) =>
        (segments.Count == 0 ? "/" : "/" + string.Join('/', segments)) + query;

    private static string[] Segments(PathString path) =>
        path.HasValue
            ? path.Value!.Split('/', StringSplitOptions.RemoveEmptyEntries)
            : Array.Empty<string>();
}
