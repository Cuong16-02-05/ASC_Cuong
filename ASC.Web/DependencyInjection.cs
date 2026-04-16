using ASC.Business;
using ASC.DataAccess;
using ASC.Model;
using ASC.Web.Data;
using ASC.Web.Infrastructure;
using ASC.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASC.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMyDependencyGroup(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ── Database ─────────────────────────────────────────────────
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<DbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            // ── ASP.NET Core Identity ────────────────────────────────────
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Cookie config
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            // ── Data Access ───────────────────────────────────────────────
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ── Business Layer ────────────────────────────────────────────
            services.AddScoped<IServiceRequestOperations, ServiceRequestOperations>();
            services.AddScoped<IMasterDataOperations, MasterDataOperations>();

            // ── Infrastructure ────────────────────────────────────────────
            services.AddScoped<IIdentitySeed, IdentitySeed>();
            services.AddTransient<IEmailSender, AuthMessageSender>();

            // ── Lab 1 - Section 10: DI Lifetime Demo ─────────────────────
            // Transient: new instance every call
            services.AddTransient<TransientLoggerService>();
            // Scoped: one instance per HTTP request
            services.AddScoped<ScopedLoggerService>();
            // Singleton: one instance for entire app lifetime
            services.AddSingleton<SingletonLoggerService>();

            // ── Cache & Navigation ────────────────────────────────────────
            services.AddMemoryCache();
            services.AddScoped<INavigationCacheOperations, NavigationCacheOperations>();

            // ── Session ───────────────────────────────────────────────────
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            return services;
        }
    }
}
