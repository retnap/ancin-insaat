using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AncinInsaat.Services;

/// <summary>
/// Translates the fixed Turkish DataAnnotations ErrorMessage strings on
/// ContactFormViewModel/CareerFormViewModel into English for the English
/// site (English localization, 2026-10-02). A plain lookup rather than
/// ErrorMessageResourceType/.resx satellite resources: every message here
/// is a short, static literal (no format placeholders), so one dictionary
/// pass over ModelState after validation is simpler than standing up a
/// second resource-file mechanism for ~a dozen strings. A message not
/// found in the map is left exactly as ModelState produced it, so an
/// unexpected validator (e.g. the file-upload one added in
/// IJobApplicationService) still shows *something* rather than nothing.
/// </summary>
public static class ModelStateLocalizer
{
    private static readonly Dictionary<string, string> Translations = new(StringComparer.Ordinal)
    {
        ["Ad soyad alanı zorunludur."] = "Full name is required.",
        ["Ad soyad 2-200 karakter arasında olmalıdır."] = "Full name must be between 2 and 200 characters.",
        ["E-posta alanı zorunludur."] = "Email address is required.",
        ["Geçerli bir e-posta adresi giriniz."] = "Please enter a valid email address.",
        ["E-posta adresi çok uzun."] = "Email address is too long.",
        ["Geçerli bir telefon numarası giriniz."] = "Please enter a valid phone number.",
        ["Telefon numarası çok uzun."] = "Phone number is too long.",
        ["Konu çok uzun."] = "Subject is too long.",
        ["Mesaj alanı zorunludur."] = "Message is required.",
        ["Mesaj 10-2000 karakter arasında olmalıdır."] = "Message must be between 10 and 2000 characters.",
        ["Mesaj çok uzun."] = "Message is too long.",
        ["Pozisyon alanı zorunludur."] = "Position is required.",
        ["Pozisyon 2-200 karakter arasında olmalıdır."] = "Position must be between 2 and 200 characters.",
        ["Lütfen CV dosyanızı (PDF) yükleyiniz."] = "Please upload your CV (PDF).",

        // CareerController.AddSubmitFailureModelError — added to ModelState
        // directly (not via a DataAnnotations attribute), same lookup.
        ["CV yalnızca PDF formatında yüklenebilir."] = "The CV can only be uploaded as a PDF.",
        ["Yüklenen dosya geçerli bir PDF değil."] = "The uploaded file is not a valid PDF.",
        ["CV dosyası 5 MB'tan büyük olamaz."] = "The CV file cannot be larger than 5 MB.",
        ["Başvurunuz gönderilemedi. Lütfen tekrar deneyin."] = "Your application could not be submitted. Please try again.",
    };

    public static void Localize(ModelStateDictionary modelState, bool isEnglish)
    {
        if (!isEnglish)
        {
            return;
        }

        foreach (var entry in modelState.Values)
        {
            for (var i = 0; i < entry.Errors.Count; i++)
            {
                var error = entry.Errors[i];

                if (Translations.TryGetValue(error.ErrorMessage, out var translated))
                {
                    entry.Errors[i] = new ModelError(translated);
                }
            }
        }
    }
}
