using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartOfNewPath.DataAccessLayer.Entities
{
    [NotMapped]
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }
    }
}
