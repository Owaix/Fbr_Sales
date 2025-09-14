using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EfPractice.Areas.Identity.Data;
using EfPractice.Repository.Interface;
using EfPractice.Repository.Class;
using EfPractice.Context;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("EFIdentityDBContextConnection") ?? throw new InvalidOperationException("Connection string 'EFIdentityDBContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

//var provider = builder.Services.BuildServiceProvider();
//var config = provider.GetService<IConfiguration>();
//builder.Services.AddDbContext<StudentContext>(item => item.UseSqlServer(config.GetConnectionString("dbcs")));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<StudentContext>();




// In your Program.cs or Startup.cs (depending on your project setup)
var provider = builder.Services.BuildServiceProvider();
var config = provider.GetService<IConfiguration>();

builder.Services.AddDbContext<StudentContext>(options =>
    options.UseSqlServer(config.GetConnectionString("dbcs")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<StudentContext>();

builder.Services.AddScoped<IMaster, Master>();
builder.Services.AddHttpClient<FbrApiClient>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();

//app.MapGet("/", async context =>
//{
//    context.Response.Redirect("/Identity/Account/Login");
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Main}/{id?}");
app.MapRazorPages();


app.Run();
