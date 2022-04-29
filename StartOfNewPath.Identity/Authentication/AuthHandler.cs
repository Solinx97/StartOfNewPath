 using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StartOfNewPath.Identity.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace StartOfNewPath.Identity.Authentication
{
    public class AuthHandler : AuthenticationHandler<AuthOptions>
    {
        private readonly IOptions<TokenSettings> _tokenSettings;
        private readonly IOptions<UserApiSettings> _userApiSettings;

        public AuthHandler(
            IOptionsMonitor<AuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<TokenSettings> tokenSettings,
            IOptions<UserApiSettings> userApiSettings)
            : base(options, logger, encoder, clock)
        {
            _tokenSettings = tokenSettings;
            _userApiSettings = userApiSettings;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var token = Request.Headers["Authorization"].ToString().Substring(_tokenSettings.Value.AuthScheme.Length);
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_userApiSettings.Value.Port);
            var response = await httpClient.GetAsync($"authentication/validate?token={token}");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Unauthorized");
        }
    }
}
