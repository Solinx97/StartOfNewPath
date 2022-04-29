using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StartOfNewPath.Models.User;

namespace StartOfNewPath.Data
{
    public class SONPAuthContext : IdentityDbContext<ApplicationUserModel>
    {
        public SONPAuthContext(
            DbContextOptions<SONPAuthContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<ApplicationRoleModel> Role { get; set; }
    }
}
