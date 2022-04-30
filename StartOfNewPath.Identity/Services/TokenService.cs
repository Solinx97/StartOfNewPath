using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StartOfNewPath.DataAccessLayer.Entities;
using StartOfNewPath.DataAccessLayer.Interfaces;
using StartOfNewPath.Identity.Interfaces;
using StartOfNewPath.Identity.Security;
using StartOfNewPath.Identity.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StartOfNewPath.Identity.Services
{
    internal class TokenService : IIdentityTokenService
    {
        private readonly IGenericRepository<RefreshToken> _repository;
        private readonly IMapper _mapper;
        private readonly TokenSettings _tokenSettings;
        private const int ExpiresTime = 10;

        public TokenService(IOptions<TokenSettings> settings, IGenericRepository<RefreshToken> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _tokenSettings = settings.Value;
        }

        string IIdentityTokenService.GenerateAccessToken(IdentityUser user, IList<string> roles)
        {
            string token = string.Empty;
            if (user != null && roles != null)
            {
                var userClaims = new Dictionary<string, object>
                {
                    { "UserName", user.UserName },
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _tokenSettings.Issuer,
                    Audience = _tokenSettings.Audience,
                    Expires = DateTime.Now.AddMinutes(ExpiresTime),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecret.AccessSecretKey)),
                        SecurityAlgorithms.HmacSha256),
                    Claims = userClaims
                };

                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                var securityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                token = jwtSecurityTokenHandler.WriteToken(securityToken);
            }

            return token;
        }

        async Task<string> IIdentityTokenService.GenerateRefreshToken(string userId)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Expires = DateTime.Now.AddMinutes(ExpiresTime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecret.RefreshSecretKey)),
                    SecurityAlgorithms.HmacSha256)
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var securityToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                Token = token,
                Expires = DateTimeOffset.Now.AddHours(1),
                UserId = userId
            };
            await _repository.CreateAsync(refreshToken);

            return token;
        }

        IEnumerable<Claim> IIdentityTokenService.ValidateAccessToken(string token)
        {
            IEnumerable<Claim> claims;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _tokenSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = _tokenSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecret.AccessSecretKey)),
                        ValidateLifetime = true,
                    },
                    out var validatedToken);
                claims = principal.Claims;
            }
            catch (SecurityTokenException)
            {
                claims = new List<Claim>();
            }

            return claims;
        }

        bool IIdentityTokenService.ValidateRefreshToken(string token)
        {
            var isValidated = true;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _tokenSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = _tokenSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecret.RefreshSecretKey)),
                        ValidateLifetime = true,
                    },
                    out var validatedToken);
            }
            catch (SecurityTokenException)
            {
                isValidated = false;
            }

            return isValidated;
        }
    }
}
