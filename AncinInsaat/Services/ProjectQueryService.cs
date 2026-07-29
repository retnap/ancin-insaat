using AncinInsaat.Data;
using AncinInsaat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AncinInsaat.Services;

public class ProjectQueryService : IProjectQueryService
{
    private readonly AppDbContext _context;

    public ProjectQueryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Project>> GetPublishedProjectsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.IsPublished)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Project>> GetFeaturedProjectsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.IsPublished && p.IsFeatured)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<Project?> GetLatestFeaturedProjectAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.IsPublished && p.IsFeatured)
            .OrderBy(p => p.DisplayOrder)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Project?> GetPublishedProjectBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.IsPublished && p.Slug == slug)
            .Include(p => p.Images.OrderBy(i => i.DisplayOrder))
            .Include(p => p.FloorPlans.OrderBy(f => f.DisplayOrder))
            .Include(p => p.Partners.OrderBy(pt => pt.DisplayOrder))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
