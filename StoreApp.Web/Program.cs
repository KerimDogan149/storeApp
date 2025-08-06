using StoreApp.Data.Concrete;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data.Abstract;
using StoreApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using StoreApp.Data.Entities;
using StoreApp.Web.Services;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddDbContext<StoreDbContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionStrings:StoreDbConnection"], b => b.MigrationsAssembly("StoreApp.Web"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<StoreDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // default fallback
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.Events.OnRedirectToLogin = context =>
    {
        var requestPath = context.Request.Path;

        if (requestPath.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Redirect("/Admin/Account/Login");
        }
        else
        {
            context.Response.Redirect("/Account/Login");
        }

        return Task.CompletedTask;
    };
});


var app = builder.Build();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "search",
    pattern: "search",
    defaults: new { controller = "Home", action = "Search" });

//  Daha genel olan: Kategori Listesi (category/telefon)
app.MapControllerRoute(
    name: "products_in_category",
    pattern: "category/{category}",
    defaults: new { controller = "Home", action = "Category" });

//En genel en alta en özel en üste
// Daha özel olan: Ürün Detay (products/iphone-15)
app.MapControllerRoute(
    name: "product_details",
    pattern: "products/{url}",
    defaults: new { controller = "Home", action = "Details" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// En genel: varsayılan route
app.MapDefaultControllerRoute();

app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await StoreApp.Data.DataSeeder.SeedAdminAsync(services);
}

app.Run();
