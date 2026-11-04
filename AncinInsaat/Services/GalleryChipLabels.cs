using System.Text.RegularExpressions;

namespace AncinInsaat.Services;

/// <summary>
/// English display labels for the Gallery's Block / Apartment Type chips
/// (English copy revision, 2026-11-03). ProjectImage.Block/ApartmentType are
/// Turkish filter keys shared by both languages (e.g. "A Tipi Blok",
/// "Zemin Kat", "1+1 A Tipi"), so only the chip's visible text is translated
/// here — the data-gallery-*-chip attribute keeps the raw key, so filtering
/// behaves exactly as before. A value no rule recognises is returned as-is.
/// </summary>
public static class GalleryChipLabels
{
    private static readonly (Regex Pattern, string Replacement)[] Rules =
    {
        (new Regex(@"^Zemin Kat$"), "Ground Floor"),
        (new Regex(@"^1\. Kat$"), "First Floor"),
        (new Regex(@"^Sol Tip$"), "Left-Hand Unit"),
        (new Regex(@"^Sağ Tip$"), "Right-Hand Unit"),
        (new Regex(@"^([A-Z]) Tipi Blok$"), "Type $1 Block"),
        (new Regex(@"^([A-Z]) Tipi?$"), "Type $1"),
        (new Regex(@"^(\d\+\d) ([A-Z]) Tipi$"), "$1 Type $2"),
        (new Regex(@"^(\d\+\d) Tipi$"), "$1"),
        (new Regex(@"^(\d\+\d)$"), "$1"),
    };

    public static string ToEnglish(string value)
    {
        foreach (var (pattern, replacement) in Rules)
        {
            if (pattern.IsMatch(value))
            {
                return pattern.Replace(value, replacement);
            }
        }

        return value;
    }
}
