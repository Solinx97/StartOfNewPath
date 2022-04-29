using Microsoft.AspNetCore.Identity;
using StartOfNewPath.Identity.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StartOfNewPath.Identity.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IdentityUser user, IList<string> roles);

        string GenerateRefreshToken();

        Task SaveRefreshToken(RefreshTokenDto refreshToken);

        bool ValidateToken(string token);
    }
}
