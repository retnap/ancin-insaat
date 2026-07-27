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
        });

        modelBuilder.Entity<ProjectImage>(entity =>
        {
            entity.Property(i => i.ImagePath).IsRequired();
        });

        modelBuilder.Entity<FloorPlan>(entity =>
        {
            entity.Property(f => f.Title).IsRequired().HasMaxLength(200);
            entity.Property(f => f.ImagePath).IsRequired();
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<CareerPosition>(entity =>
        {
            entity.Property(c => c.Title).IsRequired().HasMaxLength(200);

            entity.HasIndex(c => c.IsPublished);

            entity.HasMany(c => c.Applications)
                .WithOne(a => a.CareerPosition)
                .HasForeignKey(a => a.CareerPositionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.Property(m => m.FullName).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(320);
            entity.Property(m => m.Message).IsRequired();
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
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
