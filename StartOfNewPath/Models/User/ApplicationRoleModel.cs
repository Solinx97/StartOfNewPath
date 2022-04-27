using Microsoft.AspNetCore.Identity;

namespace StartOfNewPath.Models.User
{
    public class ApplicationRoleModel : IdentityRole
    {
        public ApplicationRoleModel(string roleName): base(roleName)
        {
        }
    }
}
