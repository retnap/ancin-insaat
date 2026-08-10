using AncinInsaat.Data;
using AncinInsaat.Data.Seed;
using AncinInsaat.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IProjectQueryService, ProjectQueryService>();
builder.Services.AddScoped<ISeoService, SeoService>();
builder.Services.AddScoped<ISiteSettingsService, SiteSettingsService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<ICareerPositionQueryService, CareerPositionQueryService>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();

// Global Search Service (docs/14_Decisions.md, Global Navigation & Search
// milestone) — every ISearchIndexProvider registered here is combined by
// ISearchService. A future content type becomes searchable by adding one
// more registration, without touching SearchController or the overlay.
builder.Services.AddScoped<ISearchIndexProvider, StaticPageSearchProvider>();
builder.Services.AddScoped<ISearchIndexProvider, ProjectSearchProvider>();
builder.Services.AddScoped<ISearchIndexProvider, CareerPositionSearchProvider>();
builder.Services.AddScoped<ISearchService, SearchService>();

var app = builder.Build();

// SQLite does not create missing parent directories for its data file;
// App_Data/ is intentionally excluded from git (runtime data), so it must
// be created here before the first migration attempt.
var dataDirectory = Path.GetDirectoryName(new SqliteConnectionStringBuilder(connectionString).DataSource);
if (!string.IsNullOrEmpty(dataDirectory))
{
    Directory.CreateDirectory(Path.Combine(app.Environment.ContentRootPath, dataDirectory));
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    await DbSeeder.SeedAsync(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
