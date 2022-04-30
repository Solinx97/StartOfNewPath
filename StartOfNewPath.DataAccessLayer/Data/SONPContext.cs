using Microsoft.EntityFrameworkCore;
using StartOfNewPath.DataAccessLayer.Entities;

namespace StartOfNewPath.DataAccessLayer.Data
{
    public class SONPContext : DbContext
    {
        public SONPContext(
            DbContextOptions<SONPContext> options) : base(options)
        {
        }

        public DbSet<Course> Course { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}