using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Payments.Infrastructure.Identity;

namespace Payments.Infrastructure.Persistence
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedEssentialsAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            var checkoutUser = new ApplicationUser { UserName = "checkout", Email = "checkout@checkout.com", EmailConfirmed = true, PhoneNumberConfirmed = true };
            if (userManager.Users.All(u => u.Id != checkoutUser.Id))
            {
                await userManager.CreateAsync(checkoutUser, "Pa$$w0rd.");
                await userManager.AddToRoleAsync(checkoutUser, Roles.Administrator.ToString());
            }

            var defaultUser = new ApplicationUser { UserName = "user", Email = "user@checkout.com", EmailConfirmed = true, PhoneNumberConfirmed = true };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, "Pa$$w0rd.");
                await userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
            }
        }
    }
}
