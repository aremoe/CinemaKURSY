using Cinema;
using Cinema.Data;
using Cinema.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connStr = builder.Configuration.GetConnectionString("SomeeDb");


builder.Services.AddControllersWithViews();

//builder.Services.AddIdentity<User, IdentityRole>(options =>
//        options.SignIn.RequireConfirmedAccount = false)
//    .AddDefaultTokenProviders()
//    //.AddDefaultUI()
//    .AddEntityFrameworkStores<CinemaDbContext>();



builder.Services.AddDbContext<CinemaDbContext>(opts =>
    opts.UseSqlServer(connStr));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<CinemaDbContext>();



builder.Services.AddScoped<FavouritesService>();
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

app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
