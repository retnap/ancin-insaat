using System.ComponentModel.DataAnnotations;

namespace AncinInsaat.Models;

// Bound from the Contact Form POST. Error messages are Turkish, matching
// every other user-facing string on the site (status labels, filter
// placeholders — see ProjectsController). Validated server-side
// (ModelState) regardless of the browser's native input validation, per
// docs/12_Security.md "Never trust client-side validation."
public class ContactFormViewModel
{
    [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Ad soyad 2-200 karakter arasında olmalıdır.")]
    [Display(Name = "Ad Soyad")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [StringLength(320, ErrorMessage = "E-posta adresi çok uzun.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [StringLength(50, ErrorMessage = "Telefon numarası çok uzun.")]
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }

    [StringLength(200, ErrorMessage = "Konu çok uzun.")]
    [Display(Name = "Konu")]
    public string? Subject { get; set; }

    [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Mesaj 10-2000 karakter arasında olmalıdır.")]
    [Display(Name = "Mesajınız")]
    public string Message { get; set; } = string.Empty;

    // Honeypot — a real visitor never sees or fills this field (visually
    // hidden + aria-hidden + tabindex="-1" in the view, see
    // _ContactForm markup), so any non-empty value here means a bot
    // filled every input blindly. Not part of ContactMessage; checked
    // and discarded in ContactController before validation/persistence.
    [Display(Name = "Website")]
    public string? Website { get; set; }
}
