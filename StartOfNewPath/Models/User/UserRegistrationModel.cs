using System.ComponentModel.DataAnnotations;

namespace StartOfNewPath.Models.User
{
    public class UserRegistrationModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}
