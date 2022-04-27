using Microsoft.AspNetCore.Identity;

namespace StartOfNewPath.DataAccessLayer.Entities.User
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
