using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Services;
using Cinema.Extensions;
using Cinema.Interfaces;
using Cinema.Helpers;

var builder = WebApplication.CreateBuilder(args);

string? connStr = builder.Configuration.GetConnectionString("SomeDb");

if (string.IsNullOrEmpty(connStr))
{
    throw new InvalidOperationException("The connection string 'SomeDb' is not defined.");
}

builder.Services.AddControllersWithViews();

// Configure MailJet settings and email sender
builder.Services.Configure<MailJetSettings>(builder.Configuration.GetSection("MailJet"));
builder.Services.AddScoped<IEmailSender, MailJetEmailSender>();

builder.Services.AddDbContext<MovieDbContext>(opts =>
    opts.UseSqlServer(connStr));

// Configure Identity with the custom User class
builder.Services.AddIdentity<User, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<MovieDbContext>();

builder.Services.AddScoped<FavoritesServiceOptimized>();
builder.Services.AddScoped<FavoritesServiceDb>();
builder.Services.AddScoped<FavoritesServiceLocal>();
builder.Services.AddScoped<ISeatService, SeatService>();

builder.Services.AddScoped<IFavoriteService>(provider =>
{
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var user = httpContextAccessor.HttpContext?.User;

    var isAuthenticated = user?.Identity?.IsAuthenticated ?? false;

    return isAuthenticated
        ? provider.GetRequiredService<FavoritesServiceDb>()
        : provider.GetRequiredService<FavoritesServiceOptimized>();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(7);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services);
        await services.SeedRoles();
        await services.SeedAdmin();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the database.");
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
app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
