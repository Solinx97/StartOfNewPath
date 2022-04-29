using Microsoft.AspNetCore.Identity;

namespace StartOfNewPath.Models.User
{
    public class ApplicationUserModel : IdentityUser
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }
    }
}