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
                .ThenInclude(f => f.Rooms.OrderBy(r => r.DisplayOrder))
            .Include(p => p.Partners.OrderBy(pt => pt.DisplayOrder))
            .Include(p => p.NearbyPlaces.OrderBy(n => n.DisplayOrder))
            .Include(p => p.ConceptVideos.OrderBy(v => v.DisplayOrder))
            .Include(p => p.ConceptImages.OrderBy(i => i.DisplayOrder))
            .Include(p => p.SitePlanImages.OrderBy(s => s.DisplayOrder))
            // Split into one query per collection (Performance investigation,
            // 2026-08-10) — this query's 7 simultaneous collection Includes
            // otherwise compile to a single SQL statement with 6 LEFT JOINs,
            // whose row count is the product of every collection's size
            // (844,992 rows for Nysa Gold: 326 images × 54 floor-plan/room
            // pairs × 4 nearby places × 4 concept images × 3 site-plan images).
            // AsSplitQuery() issues one simple SELECT per collection instead,
            // each bounded by its own natural row count, eliminating that
            // Cartesian-product materialization cost entirely.
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
    }
}
