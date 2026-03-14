using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EfPractice.Areas.Identity.Data;
using EfPractice.Repository.Interface;
using EfPractice.Repository.Class;
using EfPractice.Context;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add Azure App Configuration (using environment variable or managed identity)
var appConfigConnectionString = builder.Configuration.GetConnectionString("AppConfiguration");
Console.WriteLine($"AppConfiguration Connection String Found: {!string.IsNullOrEmpty(appConfigConnectionString)}");

if (!string.IsNullOrEmpty(appConfigConnectionString))
{
    try
    {
        builder.Configuration.AddAzureAppConfiguration(options =>
        {
            options.Connect(appConfigConnectionString)
                   .Select("fbrSales.staging.*", LabelFilter.Null) // Only load staging keys
                   .TrimKeyPrefix("fbrSales.staging.") // Remove prefix so keys work with existing code
                   .ConfigureRefresh(refresh =>
                   {
                       refresh.Register("fbrSales.staging.App:Sentinel", refreshAll: true)
                              .SetCacheExpiration(TimeSpan.FromSeconds(30));
                   });
        });
        builder.Services.AddAzureAppConfiguration();
        Console.WriteLine("✅ Azure App Configuration loaded successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Failed to load Azure App Configuration: {ex.Message}");
    }
}
else
{
    Console.WriteLine("⚠️ No Azure App Configuration connection string found. Using local configuration only.");
}

// MVC / Razor
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// HttpContext accessor (needed for StudentContext company scoping)
builder.Services.AddHttpContextAccessor();

// DbContext - Connection string now comes from Azure App Configuration
Console.WriteLine("🔍 Debugging Configuration Keys:");
Console.WriteLine($"All config keys loaded:");
foreach (var item in builder.Configuration.AsEnumerable())
{
    if (item.Key.Contains("Connection") || item.Key.Contains("dbcs"))
    {
        Console.WriteLine($"  - {item.Key} = {(!string.IsNullOrEmpty(item.Value) ? "***MASKED***" : "null")}");
    }
}

var dbConnectionString = builder.Configuration.GetConnectionString("dbcs");
Console.WriteLine($"Database Connection String Found: {!string.IsNullOrEmpty(dbConnectionString)}");

// Try alternative ways to get the connection string
if (string.IsNullOrEmpty(dbConnectionString))
{
    Console.WriteLine("⚠️ Trying alternative connection string keys...");

    // Try with double underscore (as it appears in Azure App Config after trimming)
    dbConnectionString = builder.Configuration["ConnectionStrings__dbcs"];
    Console.WriteLine($"ConnectionStrings__dbcs: {!string.IsNullOrEmpty(dbConnectionString)}");

    // Try without trimming (full key)
    if (string.IsNullOrEmpty(dbConnectionString))
    {
        dbConnectionString = builder.Configuration["fbrSales.staging.ConnectionStrings__dbcs"];
        Console.WriteLine($"fbrSales.staging.ConnectionStrings__dbcs: {!string.IsNullOrEmpty(dbConnectionString)}");
    }
}

if (string.IsNullOrEmpty(dbConnectionString))
{
    Console.WriteLine("❌ Database connection string not found! Make sure Azure App Configuration is properly configured.");
    Console.WriteLine("Required key: fbrSales.staging.ConnectionStrings__dbcs");
    throw new InvalidOperationException("Database connection string 'dbcs' not found in configuration. Check Azure App Configuration setup.");
}

Console.WriteLine("✅ Using database connection string successfully!");
builder.Services.AddDbContext<StudentContext>(options =>
    options.UseSqlServer(dbConnectionString));

// Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<StudentContext>();

// Repos / Http clients
builder.Services.AddScoped<IMaster, Master>();
builder.Services.AddHttpClient<FbrApiClient>();

var app = builder.Build();

// Apply pending migrations automatically (safe for dev/test)
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<StudentContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log or handle migration exceptions (avoid crashing silently)
        Console.WriteLine($"Migration error: {ex.Message}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use Azure App Configuration middleware for dynamic configuration refresh
app.UseAzureAppConfiguration();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Main}/{id?}");
app.MapRazorPages();

app.MapGet("/config", (IConfiguration config) =>
{
    var html = "<h1>🔧 fbrSales.staging Configuration</h1>";

    // Show only staging configuration values (from Azure App Config)
    html += "<h2>⚙️ Staging Configuration Settings:</h2><ul>";

    var dbcs = config.GetConnectionString("dbcs");
    var apiUrl = config["ApiUrl"];
    var sentinel = config["App:Sentinel"];

    html += $"<li>✅ <strong>ConnectionStrings:dbcs</strong> = <code>{(!string.IsNullOrEmpty(dbcs) ? "***MASKED***" : "❌ NOT FOUND")}</code></li>";
    html += $"<li>✅ <strong>ApiUrl</strong> = <code>{apiUrl ?? "❌ NOT FOUND"}</code></li>";
    html += $"<li>✅ <strong>App:Sentinel</strong> = <code>{sentinel ?? "❌ NOT FOUND"}</code></li>";

    html += "</ul>";
    html += "<p><small>✅ = Loaded from Azure App Configuration | ❌ = Missing</small></p>";
    return Results.Content(html, "text/html");
});

app.Run();