using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.Identity.Context;
using Project.Identity.Entities;
using Project.Identity.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddIdentity<AppUser, AppRole > (opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequiredLength = 1;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Lockout.MaxFailedAccessAttempts = 3;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddErrorDescriber<CustomIdentityErrorDescriber>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.HttpOnly = true; // Cookie bilgilerini javascript ile taray�c� �zerinden eri�mesini engeller (Document.Cookie ile eri�ilemez)
    opt.Cookie.SameSite = SameSiteMode.Strict; // Sadece ilgili Domainde kullan�l�r
    opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // always => Sadece Https ile �al���r // SameAsRequest => Http-Https 2' si ilede �al���r
    opt.Cookie.Name = "IdentityAppCookie"; // Cookie Ad�n� de�i�tirir.
    opt.ExpireTimeSpan = TimeSpan.FromDays(7); // �lgili Cookienin ne kadar s�re taray�c�da kalacag�n� belirler.
    opt.LoginPath = new PathString("/Home/SignIn");
    opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
});

builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        opt.UseSqlServer("Server=DESKTOP-FILMLVF\\SQLEXPRESS; Database=ProjectIdentity; Integrated Security=True; Trust Server Certificate=true;");
    }

    );
builder.Services.AddControllersWithViews();


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
