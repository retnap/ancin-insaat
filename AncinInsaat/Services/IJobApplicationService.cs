using Microsoft.AspNetCore.Http;

namespace AncinInsaat.Services;

// The Career Form's write path — mirrors IContactMessageService's role
// (the only other write-path service so far) but additionally owns CV
// file validation/storage, since accepting an untrusted upload safely is
// part of "persisting a job application," not a controller concern.
public interface IJobApplicationService
{
    Task<JobApplicationSubmitResult> SubmitAsync(JobApplicationSubmission submission, CancellationToken cancellationToken = default);
}

public class JobApplicationSubmission
{
    public required string Position { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public string? Phone { get; init; }
    public string? Message { get; init; }
    public required IFormFile Cv { get; init; }
}

public enum JobApplicationSubmitStatus
{
    Success,

    // Wrong extension and/or wrong declared Content-Type.
    InvalidCvType,

    // Extension/MIME looked right but the file's own bytes don't start
    // with the PDF magic number — docs/12_Security.md's third, deepest
    // File Upload check.
    InvalidCvSignature,

    CvTooLarge
}

public class JobApplicationSubmitResult
{
    public required JobApplicationSubmitStatus Status { get; init; }

    public static JobApplicationSubmitResult Success() => new() { Status = JobApplicationSubmitStatus.Success };
    public static JobApplicationSubmitResult Failure(JobApplicationSubmitStatus status) => new() { Status = status };
}
