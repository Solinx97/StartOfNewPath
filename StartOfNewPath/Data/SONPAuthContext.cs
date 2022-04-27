using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StartOfNewPath.Models.User;

namespace StartOfNewPath.Data
{
    public class SONPAuthContext : ApiAuthorizationDbContext<ApplicationUserModel>
    {
        public SONPAuthContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
            Database.EnsureCreated();
        }
    }
}
