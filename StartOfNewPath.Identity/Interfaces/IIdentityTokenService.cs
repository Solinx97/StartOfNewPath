using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StartOfNewPath.Identity.Interfaces
{
    public interface IIdentityTokenService
    {
        string GenerateAccessToken(IdentityUser user, IList<string> roles);

        Task<string> GenerateRefreshToken(string userId);

        IEnumerable<Claim> ValidateAccessToken(string token);

        bool ValidateRefreshToken(string token);
    }
}
