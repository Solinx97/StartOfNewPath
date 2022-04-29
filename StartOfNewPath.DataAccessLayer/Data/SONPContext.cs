using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using StartOfNewPath.DataAccessLayer.Entities;

namespace StartOfNewPath.DataAccessLayer.Data
{
    public class SONPContext : IdentityDbContext<ApplicationUser>
    {
        public SONPContext(
            DbContextOptions<SONPContext> options) : base(options)
        {
            Database.EnsureCreated();
            //if (!(Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            //{
            //    Database.EnsureCreated();
            //}
        }

        public DbSet<Course> Course { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}