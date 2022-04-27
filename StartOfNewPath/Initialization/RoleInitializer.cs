using Microsoft.AspNetCore.Identity;
using StartOfNewPath.Consts;
using StartOfNewPath.DataAccessLayer.Entities.User;
using System.Threading.Tasks;

namespace StartOfNewPath.Initialization
{
    public class RoleInitializer
    {
        internal static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            const string adminEmail = "admin@gmail.com";
            const string password = "_Aa123456";

            if (await roleManager.FindByNameAsync(Roles.Admin) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(Roles.Admin));
            }
            if (await roleManager.FindByNameAsync(Roles.Quest) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(Roles.Quest));
            }
            if (await roleManager.FindByNameAsync(Roles.Mentor) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(Roles.Mentor));
            }
            if (await roleManager.FindByNameAsync(Roles.Mentee) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(Roles.Mentee));
            }
            if (await roleManager.FindByNameAsync(Roles.CourseOwner) == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(Roles.CourseOwner));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }
    }
}
