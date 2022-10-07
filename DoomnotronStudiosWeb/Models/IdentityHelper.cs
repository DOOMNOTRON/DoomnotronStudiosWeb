using Microsoft.AspNetCore.Identity;
#nullable disable

namespace DoomnotronStudiosWeb.Models
{
    public static class IdentityHelper
    {
        public const string Creator = "Creator";
        public const string User = "User";

        public static async Task CreateRoles(IServiceProvider provider, params string[] roles)
        {
            RoleManager<IdentityRole> roleManager = provider.GetService<RoleManager<IdentityRole>>();

            foreach(string role in roles)
            {
                bool doesRoleExist = await roleManager.RoleExistsAsync(role);

                if (!doesRoleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task CreateDefaultUser(IServiceProvider provider, string role)
        {
            UserManager<IdentityUser> userManager = provider.GetService<UserManager<IdentityUser>>();

            // If no users are present, then make the default user.
            int numUsers = (await userManager.GetUsersInRoleAsync(role)).Count;
            if(numUsers == 0) // If no users are in the specified role
            {
                var defaultUser = new IdentityUser()
                {
                    Email = "creator@doomnotron.com",
                    UserName = "Admin"
                };

                await userManager.CreateAsync(defaultUser, "Programming01#");

                await userManager.AddToRoleAsync(defaultUser, role);
            }
        }

    }
}
