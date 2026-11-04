using System.Globalization;
using Microsoft.Extensions.Localization;

namespace AncinInsaat.Services;

/// <summary>
/// DataAnnotations localizer (English copy revision, 2026-11-03). The form
/// view models keep their Turkish ErrorMessage strings as the single source
/// of truth; on an English request this returns the English equivalent from
/// <see cref="ModelStateLocalizer.Translations"/>, so the data-val-* attributes
/// rendered into the /en forms and any server-side validation errors are
/// English too. Any other string (or any Turkish request) is returned
/// unchanged, so Turkish pages behave exactly as before.
/// </summary>
public class ValidationMessageLocalizer : IStringLocalizer
{
    public LocalizedString this[string name] => Translate(name, name);

    public LocalizedString this[string name, params object[] arguments] =>
        Translate(name, string.Format(CultureInfo.CurrentCulture, Translate(name, name).Value, arguments));

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
        ModelStateLocalizer.Translations.Select(pair => new LocalizedString(pair.Key, pair.Value));

    private static LocalizedString Translate(string name, string fallback)
    {
        var isEnglish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("en", StringComparison.OrdinalIgnoreCase);

        if (isEnglish && ModelStateLocalizer.Translations.TryGetValue(name, out var translated))
        {
            return new LocalizedString(name, translated, resourceNotFound: false);
        }

        return new LocalizedString(name, fallback, resourceNotFound: true);
    }
}
