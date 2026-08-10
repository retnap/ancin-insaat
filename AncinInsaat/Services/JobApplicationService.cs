using AncinInsaat.Data;
using AncinInsaat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AncinInsaat.Services;

// See IJobApplicationService for the validation contract. Storage location
// (Claude.md "CV UPLOAD" decision): App_Data/uploads/cv/ — outside
// wwwroot, so no route ever serves it (App_Data is also excluded from
// git as runtime data, same as the SQLite file it already holds). Only
// the relative path (relative to App_Data) is stored on JobApplication;
// the original filename is never trusted or kept.
public class JobApplicationService : IJobApplicationService
{
    private const long MaxCvSizeBytes = 5 * 1024 * 1024; // 5 MB
    private const string UploadsRelativeDirectory = "uploads/cv";
    private static readonly byte[] PdfSignature = "%PDF-"u8.ToArray();

    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public JobApplicationService(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<JobApplicationSubmitResult> SubmitAsync(JobApplicationSubmission submission, CancellationToken cancellationToken = default)
    {
        var position = await _context.CareerPositions
            .FirstOrDefaultAsync(c => c.Id == submission.CareerPositionId && c.IsPublished, cancellationToken);

        if (position is null)
        {
            return JobApplicationSubmitResult.Failure(JobApplicationSubmitStatus.InvalidPosition);
        }

        if (submission.Cv.Length > MaxCvSizeBytes)
        {
            return JobApplicationSubmitResult.Failure(JobApplicationSubmitStatus.CvTooLarge);
        }

        var extension = Path.GetExtension(submission.Cv.FileName);
        var hasValidExtension = string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase);
        var hasValidContentType = string.Equals(submission.Cv.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase);

        if (!hasValidExtension || !hasValidContentType)
        {
            return JobApplicationSubmitResult.Failure(JobApplicationSubmitStatus.InvalidCvType);
        }

        if (!await HasPdfSignatureAsync(submission.Cv, cancellationToken))
        {
            return JobApplicationSubmitResult.Failure(JobApplicationSubmitStatus.InvalidCvSignature);
        }

        var uploadsDirectory = Path.Combine(_environment.ContentRootPath, "App_Data", UploadsRelativeDirectory);
        Directory.CreateDirectory(uploadsDirectory);

        // Never derived from the client-supplied filename — a random name
        // both avoids path traversal / overwrite risk and matches
        // Claude.md's "Rename uploaded files" requirement.
        var storedFileName = $"{Guid.NewGuid():N}.pdf";
        var physicalPath = Path.Combine(uploadsDirectory, storedFileName);

        await using (var destination = File.Create(physicalPath))
        {
            await using var source = submission.Cv.OpenReadStream();
            source.Position = 0;
            await source.CopyToAsync(destination, cancellationToken);
        }

        _context.JobApplications.Add(new JobApplication
        {
            CareerPositionId = position.Id,
            FullName = submission.FullName,
            Email = submission.Email,
            Phone = submission.Phone,
            CVPath = $"{UploadsRelativeDirectory}/{storedFileName}",
            Message = submission.Message,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);

        return JobApplicationSubmitResult.Success();
    }

    private static async Task<bool> HasPdfSignatureAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var buffer = new byte[PdfSignature.Length];

        await using var stream = file.OpenReadStream();
        var bytesRead = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken);

        return bytesRead == PdfSignature.Length && buffer.AsSpan().SequenceEqual(PdfSignature);
    }
}
