using AncinInsaat.Data;
using AncinInsaat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AncinInsaat.Services;

public class SiteSettingsService : ISiteSettingsService
{
    private readonly AppDbContext _context;

    public SiteSettingsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SiteSettings?> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SiteSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
}
