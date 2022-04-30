using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StartOfNewPath.Identity.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace StartOfNewPath.Identity.Authentication
{
    public class AuthHandler : AuthenticationHandler<AuthOptions>
    {
        private readonly IIdentityTokenService _tokenService;

        public AuthHandler(
            IOptionsMonitor<AuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IIdentityTokenService tokenService)
            : base(options, logger, encoder, clock)
        {
            _tokenService = tokenService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.TryGetValue("accessToken", out string accessToken))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var isValidate = IsValidate(accessToken);
            if (isValidate)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(accessToken);
                var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Unauthorized");
        }

        private bool IsValidate(string accessToken)
        {
            var result = _tokenService.ValidateAccessToken(accessToken);
            if (result.Any())
            {
                return true;
            }

            return false;
        }
    }
}
