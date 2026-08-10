using AncinInsaat.Data;
using AncinInsaat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AncinInsaat.Services;

public class CareerPositionQueryService : ICareerPositionQueryService
{
    private readonly AppDbContext _context;

    public CareerPositionQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CareerPosition>> GetPublishedAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CareerPositions
            .AsNoTracking()
            .Where(c => c.IsPublished)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
