using AncinInsaat.Data.Entities;
using AncinInsaat.Models;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AncinInsaat.Controllers;

// Route is explicit ("career") to match docs/03_PageBlueprints.md's "/career"
// — same explicit-attribute-route precedent as AboutController/ValuesController.
public class CareerController : Controller
{
    // Same PRG key convention as ContactController.SuccessTempDataKey.
    private const string SuccessTempDataKey = "CareerFormSuccess";

    private readonly ICareerPositionQueryService _careerPositionQueryService;
    private readonly IJobApplicationService _jobApplicationService;
    private readonly ISeoService _seoService;
    private readonly ILogger<CareerController> _logger;

    public CareerController(
        ICareerPositionQueryService careerPositionQueryService,
        IJobApplicationService jobApplicationService,
        ISeoService seoService,
        ILogger<CareerController> logger)
    {
        _careerPositionQueryService = careerPositionQueryService;
        _jobApplicationService = jobApplicationService;
        _seoService = seoService;
        _logger = logger;
    }

    [HttpGet("career")]
    public async Task<IActionResult> Index()
    {
        var showSuccess = TempData[SuccessTempDataKey] is not null;

        var positions = await _careerPositionQueryService.GetPublishedAsync();
        var model = BuildPageModel(new CareerFormViewModel(), positions, showSuccess);

        ViewData["Seo"] = await _seoService.GetPageSeoAsync("career", Request);
        ViewData["SeoPageType"] = "WebPage";

        return View(model);
    }

    [HttpPost("career")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(CareerFormViewModel form)
    {
        // Honeypot — identical convention/rationale to ContactController.Submit.
        if (!string.IsNullOrWhiteSpace(form.Website))
        {
            _logger.LogWarning("Career form honeypot triggered — submission discarded.");
            TempData[SuccessTempDataKey] = true;
            return RedirectToAction(nameof(Index));
        }

        var positions = await _careerPositionQueryService.GetPublishedAsync();

        if (positions.Count == 0)
        {
            // Nothing left to apply to — a submission reaching here has no
            // valid CareerPositionId to bind to, so treat it the same as an
            // invalid model rather than attempting to persist it.
            ModelState.AddModelError(string.Empty, "Şu anda açık bir pozisyon bulunmamaktadır.");
        }

        if (!ModelState.IsValid)
        {
            var invalidModel = BuildPageModel(form, positions, showSuccess: false);

            ViewData["Seo"] = await _seoService.GetPageSeoAsync("career", Request);
            ViewData["SeoPageType"] = "WebPage";

            return View(nameof(Index), invalidModel);
        }

        var result = await _jobApplicationService.SubmitAsync(new JobApplicationSubmission
        {
            CareerPositionId = form.CareerPositionId!.Value,
            FullName = form.FullName.Trim(),
            Email = form.Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(form.Phone) ? null : form.Phone.Trim(),
            Message = string.IsNullOrWhiteSpace(form.Message) ? null : form.Message.Trim(),
            Cv = form.CV!
        });

        if (result.Status != JobApplicationSubmitStatus.Success)
        {
            AddSubmitFailureModelError(result.Status);

            var invalidModel = BuildPageModel(form, positions, showSuccess: false);

            ViewData["Seo"] = await _seoService.GetPageSeoAsync("career", Request);
            ViewData["SeoPageType"] = "WebPage";

            return View(nameof(Index), invalidModel);
        }

        TempData[SuccessTempDataKey] = true;
        return RedirectToAction(nameof(Index));
    }

    private void AddSubmitFailureModelError(JobApplicationSubmitStatus status)
    {
        var message = status switch
        {
            JobApplicationSubmitStatus.InvalidPosition => "Seçilen pozisyon artık geçerli değil. Lütfen sayfayı yenileyip tekrar deneyin.",
            JobApplicationSubmitStatus.InvalidCvType => "CV yalnızca PDF formatında yüklenebilir.",
            JobApplicationSubmitStatus.InvalidCvSignature => "Yüklenen dosya geçerli bir PDF değil.",
            JobApplicationSubmitStatus.CvTooLarge => "CV dosyası 5 MB'tan büyük olamaz.",
            _ => "Başvurunuz gönderilemedi. Lütfen tekrar deneyin."
        };

        ModelState.AddModelError("Form.CV", message);
    }

    private static CareerPageViewModel BuildPageModel(CareerFormViewModel form, IReadOnlyList<CareerPosition> positions, bool showSuccess)
    {
        var informationCardItems = positions
            .Select(p => new InformationCardItem
            {
                IconMarkup = PositionIconMarkup,
                Label = p.Title,
                Value = BuildPositionSummary(p)
            })
            .ToList();

        var positionOptions = positions
            .Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = string.IsNullOrWhiteSpace(p.Department) ? p.Title : $"{p.Title} — {p.Department}"
            })
            .ToList();

        return new CareerPageViewModel
        {
            Form = form,
            Positions = positions,
            OpenPositions = new InformationCardModel
            {
                Layout = InformationCardLayout.Grid,
                Columns = 3,
                Items = informationCardItems
            },
            PositionOptions = positionOptions,
            ShowSuccess = showSuccess
        };
    }

    private static string BuildPositionSummary(CareerPosition position)
    {
        var location = string.Join(" · ", new[] { position.Department, position.Location }
            .Where(part => !string.IsNullOrWhiteSpace(part)));

        return string.IsNullOrWhiteSpace(location)
            ? position.Description
            : $"{location}\n\n{position.Description}";
    }

    // Single shared briefcase icon — every Open Positions card represents
    // the same kind of thing (a job listing), unlike Areas of Expertise's
    // per-category icons, so one icon reused across cards reads more
    // consistently than inventing a distinct icon per department.
    private const string PositionIconMarkup = """
        <rect x="3.5" y="7.5" width="17" height="11.5" rx="1.5" fill="none" stroke="currentColor" stroke-width="1.6" />
        <path d="M8.5 7.5V6a2 2 0 012-2h3a2 2 0 012 2v1.5" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        <path d="M3.5 12.5h17" stroke="currentColor" stroke-width="1.6" />
        <path d="M10.5 12v1.5h3V12" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round" />
        """;
}
