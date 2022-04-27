using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StartOfNewPath.DataAccessLayer.Entities;
using StartOfNewPath.DataAccessLayer.Entities.User;

namespace StartOfNewPath.DataAccessLayer.Data
{
    public class SONPContext : DbContext
    {
        public SONPContext(
            DbContextOptions<SONPContext> options) : base(options)
        {
        }

        public DbSet<Course> Course { get; set; }

        public DbSet<ApplicationRole> Role { get; set; }
    }
}