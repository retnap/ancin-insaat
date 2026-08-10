using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AncinInsaat.Models;

// Bound from the Career Form POST — same validation shape/convention as
// ContactFormViewModel (Turkish messages, server-side ModelState
// regardless of native browser validation, per docs/12_Security.md
// "Never trust client-side validation"). CV file-content validation
// (extension/MIME/PDF signature/size) happens in IJobApplicationService,
// not here — DataAnnotations has no built-in way to inspect file bytes.
public class CareerFormViewModel
{
    [Required(ErrorMessage = "Lütfen bir pozisyon seçiniz.")]
    [Display(Name = "Pozisyon")]
    public int? CareerPositionId { get; set; }

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

    [StringLength(2000, ErrorMessage = "Mesaj çok uzun.")]
    [Display(Name = "Mesajınız")]
    public string? Message { get; set; }

    [Required(ErrorMessage = "Lütfen CV dosyanızı (PDF) yükleyiniz.")]
    [Display(Name = "CV")]
    public IFormFile? CV { get; set; }

    // Honeypot — same convention as ContactFormViewModel.Website, checked
    // and discarded in CareerController before validation/persistence.
    [Display(Name = "Website")]
    public string? Website { get; set; }
}
