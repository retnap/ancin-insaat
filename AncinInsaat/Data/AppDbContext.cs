using AncinInsaat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AncinInsaat.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectImage> ProjectImages => Set<ProjectImage>();
    public DbSet<FloorPlan> FloorPlans => Set<FloorPlan>();
    public DbSet<FloorPlanRoom> FloorPlanRooms => Set<FloorPlanRoom>();
    public DbSet<ProjectConceptVideo> ProjectConceptVideos => Set<ProjectConceptVideo>();
    public DbSet<ProjectConceptImage> ProjectConceptImages => Set<ProjectConceptImage>();
    public DbSet<ProjectSitePlanImage> ProjectSitePlanImages => Set<ProjectSitePlanImage>();
    public DbSet<ProjectNearbyPlace> ProjectNearbyPlaces => Set<ProjectNearbyPlace>();
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<CareerPosition> CareerPositions => Set<CareerPosition>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<SeoMetadata> SeoMetadata => Set<SeoMetadata>();
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Slug).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasIndex(p => p.Slug).IsUnique();
            entity.HasIndex(p => p.Status);
            entity.HasIndex(p => p.IsFeatured);

            entity.HasMany(p => p.Images)
                .WithOne(i => i.Project)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.FloorPlans)
                .WithOne(f => f.Project)
                .HasForeignKey(f => f.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Partners)
                .WithOne(pt => pt.Project)
                .HasForeignKey(pt => pt.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.NearbyPlaces)
                .WithOne(n => n.Project)
                .HasForeignKey(n => n.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.ConceptVideos)
                .WithOne(v => v.Project)
                .HasForeignKey(v => v.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.ConceptImages)
                .WithOne(i => i.Project)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.SitePlanImages)
                .WithOne(s => s.Project)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProjectConceptVideo>(entity =>
        {
            entity.Property(v => v.VideoPath).IsRequired();
            entity.Property(v => v.PosterPath).IsRequired();
            entity.Property(v => v.Eyebrow).IsRequired().HasMaxLength(100);
            entity.Property(v => v.Title).IsRequired().HasMaxLength(200);
            entity.Property(v => v.Description).IsRequired();

            entity.HasIndex(v => new { v.ProjectId, v.DisplayOrder });
        });

        modelBuilder.Entity<ProjectConceptImage>(entity =>
        {
            entity.Property(i => i.ImagePath).IsRequired();
            entity.Property(i => i.Eyebrow).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Title).IsRequired().HasMaxLength(200);
            entity.Property(i => i.Description).IsRequired();

            entity.HasIndex(i => new { i.ProjectId, i.DisplayOrder });
        });

        modelBuilder.Entity<ProjectSitePlanImage>(entity =>
        {
            entity.Property(s => s.ImagePath).IsRequired();

            entity.HasIndex(s => new { s.ProjectId, s.DisplayOrder });
        });

        modelBuilder.Entity<ProjectImage>(entity =>
        {
            entity.Property(i => i.ImagePath).IsRequired();
        });

        modelBuilder.Entity<ProjectNearbyPlace>(entity =>
        {
            entity.Property(n => n.Name).IsRequired().HasMaxLength(200);
            entity.Property(n => n.Distance).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<FloorPlan>(entity =>
        {
            entity.Property(f => f.ApartmentType).IsRequired().HasMaxLength(50);
            entity.Property(f => f.ImagePath).IsRequired();
            entity.Property(f => f.NetAreaM2).HasPrecision(6, 2);
            entity.Property(f => f.GrossAreaM2).HasPrecision(6, 2);
            entity.Property(f => f.SalesGrossAreaM2).HasPrecision(6, 2);

            entity.HasMany(f => f.Rooms)
                .WithOne(r => r.FloorPlan)
                .HasForeignKey(r => r.FloorPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FloorPlanRoom>(entity =>
        {
            entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            entity.Property(r => r.AreaM2).HasPrecision(6, 2);
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<CareerPosition>(entity =>
        {
            entity.Property(c => c.Title).IsRequired().HasMaxLength(200);

            entity.HasIndex(c => c.IsPublished);
        });

        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.Property(m => m.FullName).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(320);
            entity.Property(m => m.Message).IsRequired();
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.Property(a => a.Position).IsRequired().HasMaxLength(200);
            entity.Property(a => a.FullName).IsRequired().HasMaxLength(200);
            entity.Property(a => a.Email).IsRequired().HasMaxLength(320);
            entity.Property(a => a.CVPath).IsRequired();
        });

        modelBuilder.Entity<SeoMetadata>(entity =>
        {
            entity.Property(s => s.Page).IsRequired().HasMaxLength(200);

            entity.HasIndex(s => s.Page).IsUnique();
        });

        modelBuilder.Entity<SiteSettings>(entity =>
        {
            entity.Property(s => s.CompanyName).IsRequired().HasMaxLength(200);
        });
    }
}
