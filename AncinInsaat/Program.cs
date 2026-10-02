using System.Globalization;
using AncinInsaat.Data;
using AncinInsaat.Data.Seed;
using AncinInsaat.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Render assigns the container's listen port via the PORT environment
// variable at container start (not known at build/publish time) and scans
// for an open TCP port on that value — it does not read the Dockerfile's
// EXPOSE, and nothing guarantees ASPNETCORE_HTTP_PORTS ends up honored by
// every possible way this service could be run/redeployed. Binding
// explicitly here keeps the app discoverable by Render's port scan
// regardless. 10000 is Render's own documented default, used only when
// PORT isn't set. Scoped to non-Development so plain `dotnet run`/IIS
// Express keep using Properties/launchSettings.json's existing local ports
// (PORT is never set in those profiles) exactly as before.
if (!builder.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

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

// English localization (2026-10-02) — the request culture is derived
// purely from the URL's leading "/en" segment (see
// RouteSegmentRequestCultureProvider/docs/14_Decisions.md), never from a
// cookie or Accept-Language. tr-TR stays the default/fallback for any path
// that provider doesn't recognize. Every page that reads
// CultureInfo.CurrentUICulture (views, ViewComponents, SeoService) picks
// its Turkish or English content from this single per-request culture.
var supportedCultures = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-US") };
var requestLocalizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr-TR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
};
requestLocalizationOptions.RequestCultureProviders.Clear();
requestLocalizationOptions.RequestCultureProviders.Add(new RouteSegmentRequestCultureProvider());

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

app.UseRequestLocalization(requestLocalizationOptions);

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
