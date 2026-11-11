using Microsoft.AspNetCore.Identity;

namespace TaskManagementSystem.Data
{
    public class DbInitializer
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }
        }

        public static async Task SeedUserAsync(
            UserManager<IdentityUser> userManager,
            string username,
            string email,
            string password,
            string role)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = username,
                    Email = email,
                };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(",", result.Errors.Select(e => e.Description)));
                }
            }
            if (!await userManager.IsInRoleAsync(user , role))
            {
                var result = await userManager.AddToRoleAsync(user , role);
                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(",", result.Errors.Select(e => e.Description)));
                }
            }

        }
    }
}
