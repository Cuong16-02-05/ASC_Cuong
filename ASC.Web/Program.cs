using ASC.Model;
using ASC.Web;
using ASC.Web.Configuration;
using ASC.Web.Data;
using ASC.Web.Infrastructure;
using ASC.Web.Services; // Đã thêm using cho IEmailSender và AuthMessageSender
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Bind ApplicationSettings from appsettings.json
builder.Services.Configure<ApplicationSettings>(
    builder.Configuration.GetSection("ApplicationSettings"));

// MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// 👉 ĐÃ THÊM DÒNG NÀY ĐỂ FIX LỖI QUÊN MẬT KHẨU
builder.Services.AddTransient<IEmailSender, AuthMessageSender>();

// All custom DI (DB, Identity, Business, Session, Cache, etc.)
builder.Services.AddMyDependencyGroup(builder.Configuration);

var app = builder.Build();

// ── Middleware Pipeline ──────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ── Seed DB + Navigation Cache on startup ────────────────────
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var options = services.GetRequiredService<IOptions<ApplicationSettings>>();
        var seed = services.GetRequiredService<IIdentitySeed>();
        await seed.Seed(userManager, roleManager, options);

        var navCache = services.GetRequiredService<INavigationCacheOperations>();
        await navCache.CreateNavigationCacheAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error during startup seed.");
    }
}

// ── Routing ───────────────────────────────────────────────────
// Area routes first
app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Dashboard}/{id?}");

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Razor Pages (Identity)
app.MapRazorPages();

app.Run();