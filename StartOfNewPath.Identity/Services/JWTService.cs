using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StartOfNewPath.Identity.Interfaces;
using StartOfNewPath.Identity.Security;
using StartOfNewPath.Identity.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StartOfNewPath.Identity.Services
{
    public class JWTService : IJWTService
    {
        private readonly JWTSettings _settings;
        private readonly string _jwtSecretKey;

        public JWTService(IOptions<JWTSettings> options)
        {
            if (options != null)
            {
                _settings = options.Value;
            }

            _jwtSecretKey = JWTSecret.SecretKey;
        }

        string IJWTService.GetToken(IdentityUser user)
        {
            string encodedJwtToken = string.Empty;

            if (user != null)
            {
                var userClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                };

                var jwtToken = new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey)),
                        SecurityAlgorithms.HmacSha256),
                    claims: userClaims);

                encodedJwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }

            return encodedJwtToken;
        }

        string IJWTService.GetToken(IdentityUser user, string role)
        {
            string encodedJwtToken = string.Empty;

            if (user != null && role != null)
            {
                var userClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Actor, user.UserName),
                    new Claim(ClaimTypes.Role, role),
                };

                var jwtToken = new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey)),
                        SecurityAlgorithms.HmacSha256),
                    claims: userClaims);

                encodedJwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }

            return encodedJwtToken;
        }

        string IJWTService.GetToken(IdentityUser user, IList<string> roles)
        {
            string encodedJwtToken = string.Empty;

            if (user != null && roles != null)
            {
                var userClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                };

                foreach (var role in roles)
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var jwtToken = new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey)),
                        SecurityAlgorithms.HmacSha256),
                    claims: userClaims);

                encodedJwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }

            return encodedJwtToken;
        }

        bool IJWTService.ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _settings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _settings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey)),
                    ValidateLifetime = true,
                },
                out var _);
            }
            catch (SecurityTokenException)
            {
                return false;
            }

            return true;
        }
    }
}
