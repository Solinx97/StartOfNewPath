using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StartOfNewPath.Identity.Interfaces
{
    public interface IIdentityTokenService
    {
        string GenerateAccessToken(IdentityUser user, IList<string> roles);

        Task<string> GenerateRefreshTokenAsync(string userId);

        IEnumerable<Claim> ValidateAccessToken(string token, out SecurityToken validatedToken);

        Task<bool> RefreshAsync(string refreshToken);
    }
}
