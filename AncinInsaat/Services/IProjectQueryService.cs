using AncinInsaat.Data.Entities;

namespace AncinInsaat.Services;

// Read-only query surface over Project data, shared by every page that
// needs project listings or detail (Home, Projects, future pages) so
// they never query AppDbContext directly. Never returns unpublished
// projects — IsPublished is an editorial gate, not a security boundary,
// but the public site has no path to view draft content regardless.
public interface IProjectQueryService
{
    Task<IReadOnlyList<Project>> GetPublishedProjectsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Project>> GetFeaturedProjectsAsync(CancellationToken cancellationToken = default);

    // The project currently showcased in the Home Hero Banner — the
    // featured project with the lowest DisplayOrder. Promoting a
    // different project to the hero is an editorial change (reorder
    // DisplayOrder / IsFeatured in the data), never a code change.
    Task<Project?> GetLatestFeaturedProjectAsync(CancellationToken cancellationToken = default);

    Task<Project?> GetPublishedProjectBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
