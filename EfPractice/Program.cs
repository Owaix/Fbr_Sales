using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EfPractice.Areas.Identity.Data;
using EfPractice.Repository.Interface;
using EfPractice.Repository.Class;
using EfPractice.Context;

var builder = WebApplication.CreateBuilder(args);

// MVC / Razor
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// HttpContext accessor (needed for StudentContext company scoping)
builder.Services.AddHttpContextAccessor();

// DbContext
builder.Services.AddDbContext<StudentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Main}/{id?}");
app.MapRazorPages();

app.Run();
