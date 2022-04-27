using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace StartOfNewPath.Identity.Interfaces
{
    public interface IJWTService
    {
        string GetToken(IdentityUser user);

        string GetToken(IdentityUser user, string role);

        string GetToken(IdentityUser user, IList<string> roles);

        bool ValidateToken(string token);
    }
}
