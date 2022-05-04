using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StartOfNewPath.Identity.Interfaces;
using System;
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
            if (!Request.Cookies.TryGetValue("accessToken", out var accessToken))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var claims = _tokenService.ValidateAccessToken(accessToken, out var validatedToken);
            if (!claims.Any())
            {
                Response.Cookies.Delete("accessToken");
                return AuthenticateResult.Fail("Unauthorized");
            }

            var isExpiresed = DateTimeOffset.Now.UtcDateTime > validatedToken.ValidTo;
            if (isExpiresed)
            {
                Response.Cookies.Delete("accessToken");
                return AuthenticateResult.Fail("Unauthorized");
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
