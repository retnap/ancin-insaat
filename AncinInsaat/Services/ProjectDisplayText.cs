using System.Text.RegularExpressions;

namespace AncinInsaat.Services;

// English display forms of a project's Name and Location (English copy
// revision, 2026-11-03, general manager's decision). Project.Name and
// Project.Location stay Turkish in the database (they are also slugs'
// source, filter keys and the Turkish site's text); only what an English
// page shows goes through here.
public static class ProjectDisplayText
{
    // "La Fiore Karabağ 2. Etap" → "La Fiore Karabağ Phase 2". The phase
    // suffix is the only translated part of a project name.
    private static readonly Regex PhaseSuffix = new(@"^(?<name>.+?) (?<number>\d+)\. Etap$", RegexOptions.CultureInvariant);

    // Locations written differently in English; anything else ("Aydın
    // Efeler") is a place name shown as-is in both languages.
    private static readonly IReadOnlyDictionary<string, string> LocationsEn = new Dictionary<string, string>
    {
        ["İzmir / Narlıdere"] = "Narlıdere, İzmir"
    };

    public static string Name(string name, bool isEnglish) =>
        isEnglish ? PhaseSuffix.Replace(name, "${name} Phase ${number}") : name;

    public static string? Location(string? location, bool isEnglish) =>
        isEnglish && location is not null && LocationsEn.TryGetValue(location, out var english) ? english : location;
}
