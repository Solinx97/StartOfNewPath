using Microsoft.AspNetCore.Identity;
using StartOfNewPath.Consts;
using StartOfNewPath.Models.User;
using System.Threading.Tasks;

namespace StartOfNewPath.Initialization
{
    public class RoleInitializer
    {
        internal static async Task InitializeAsync(UserManager<ApplicationUserModel> userManager, RoleManager<ApplicationRoleModel> roleManager)
        {
            const string adminEmail = "solinxAdmin@mail.com";
            const string password = "1D56r$s445";

            if (await roleManager.FindByNameAsync(Roles.Admin) == null)
            {
                await roleManager.CreateAsync(new ApplicationRoleModel(Roles.Admin));
            }
            if (await roleManager.FindByNameAsync(Roles.Quest) == null)
            {
                await roleManager.CreateAsync(new ApplicationRoleModel(Roles.Quest));
            }
            if (await roleManager.FindByNameAsync(Roles.Mentor) == null)
            {
                await roleManager.CreateAsync(new ApplicationRoleModel(Roles.Mentor));
            }
            if (await roleManager.FindByNameAsync(Roles.Mentee) == null)
            {
                await roleManager.CreateAsync(new ApplicationRoleModel(Roles.Mentee));
            }
            if (await roleManager.FindByNameAsync(Roles.CourseOwner) == null)
            {
                await roleManager.CreateAsync(new ApplicationRoleModel(Roles.CourseOwner));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new ApplicationUserModel { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }
    }
}
