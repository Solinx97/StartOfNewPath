using Microsoft.AspNetCore.Identity;

namespace StartOfNewPath.Models.User
{
    public class ApplicationUserModel : IdentityUser
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }
    }
}
