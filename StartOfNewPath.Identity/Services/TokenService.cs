using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StartOfNewPath.DataAccessLayer.Entities;
using StartOfNewPath.DataAccessLayer.Interfaces;
using StartOfNewPath.Identity.DTO;
using StartOfNewPath.Identity.Interfaces;
using StartOfNewPath.Identity.Security;
using StartOfNewPath.Identity.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StartOfNewPath.Identity.Services
{
    internal class TokenService : IIdentityTokenService
    {
        private readonly ITokenRepository _repository;
        private readonly IMapper _mapper;
        private readonly TokenSettings _tokenSettings;
        private const int AccessExpiresTimeInMinutes = 30;
        private const int RefreshExpiresTimeInHours = 24;

        public TokenService(IOptions<TokenSettings> settings, ITokenRepository repository,
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
                var userClaims = new List<Claim> { new Claim("userName", user.UserName) };
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _tokenSettings.Issuer,
                    Audience = _tokenSettings.Audience,
                    Expires = DateTime.Now.AddMinutes(AccessExpiresTimeInMinutes),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSecret.AccessSecretKey)),
                        SecurityAlgorithms.HmacSha256),
                    Subject = new ClaimsIdentity(userClaims)
                };

                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                var securityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                token = jwtSecurityTokenHandler.WriteToken(securityToken);
            }

            return token;
        }

        async Task<string> IIdentityTokenService.GenerateRefreshTokenAsync(string userId)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Expires = DateTime.Now.AddHours(RefreshExpiresTimeInHours),
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
                Expires = DateTimeOffset.Now.AddMinutes(10).UtcDateTime,
                UserId = userId
            };
            await _repository.CreateAsync(refreshToken);

            return token;
        }

        IEnumerable<Claim> IIdentityTokenService.ValidateAccessToken(string token, out SecurityToken validatedToken)
        {
            IEnumerable<Claim> claims;
            validatedToken = null;
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
                    out validatedToken);
                claims = principal.Claims;
            }
            catch (SecurityTokenException)
            {
                claims = new List<Claim>();
            }

            return claims;
        }

        async Task<bool> IIdentityTokenService.RefreshAsync(string refreshToken)
        {
            var claims = ValidateRefreshToken(refreshToken, out var validateToken);
            if (!claims.Any())
            {
                return false;
            }

            var foundToken = await FindRefreshTokenAsync(refreshToken);
            if (foundToken == null)
            {
                return false;
            }

            var isExpiresed = DateTimeOffset.Now.UtcDateTime > validateToken.ValidTo;
            if (isExpiresed)
            {
                await RemoveRefreshTokenAsync(foundToken);
                return false;
            }

            return true;
        }

        private IEnumerable<Claim> ValidateRefreshToken(string token, out SecurityToken validatedToken)
        {
            IEnumerable<Claim> claims;
            validatedToken = null;
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
                    out validatedToken);
                claims = principal.Claims;
            }
            catch (SecurityTokenException)
            {
                claims = new List<Claim>();
            }

            return claims;
        }

        private async Task<RefreshTokenDto> FindRefreshTokenAsync(string refreshToken)
        {
            var token = await _repository.Get(refreshToken);
            var map = _mapper.Map<RefreshTokenDto>(token);

            return map;
        }

        private async Task<int> RemoveRefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            var map = _mapper.Map<RefreshToken>(refreshToken);
            var result = await _repository.DeleteAsync(map);

           return result;
        }
    }
}
