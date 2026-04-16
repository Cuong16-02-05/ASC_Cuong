using ASC.Model;
using ASC.Web.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ASC.Web.Data
{
    public interface IIdentitySeed
    {
        Task Seed(UserManager<ApplicationUser> userManager,
                  RoleManager<IdentityRole> roleManager,
                  IOptions<ApplicationSettings> options);
    }

    public class IdentitySeed : IIdentitySeed
    {
        public async Task Seed(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IOptions<ApplicationSettings> options)
        {
            var settings = options.Value;

            // Seed Roles
            string[] roles = { Constants.Roles.Admin, Constants.Roles.Engineer, Constants.Roles.User };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seed Admin
            if (await userManager.FindByEmailAsync(settings.AdminEmail!) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = settings.AdminEmail,
                    Email = settings.AdminEmail,
                    FirstName = settings.AdminName,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, settings.AdminPassword!);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, Constants.Roles.Admin);
            }

            // Seed Engineer
            if (await userManager.FindByEmailAsync(settings.EngineerEmail!) == null)
            {
                var engineer = new ApplicationUser
                {
                    UserName = settings.EngineerEmail,
                    Email = settings.EngineerEmail,
                    FirstName = settings.EngineerName,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(engineer, settings.EngineerPassword!);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(engineer, Constants.Roles.Engineer);
            }
        }
    }
}
