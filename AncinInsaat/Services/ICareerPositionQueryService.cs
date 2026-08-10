using AncinInsaat.Data.Entities;

namespace AncinInsaat.Services;

// Read-only accessor for CareerPosition — same "query service" split
// IProjectQueryService already establishes (reads separated from the
// write path, which lives in IJobApplicationService).
public interface ICareerPositionQueryService
{
    Task<IReadOnlyList<CareerPosition>> GetPublishedAsync(CancellationToken cancellationToken = default);
}
