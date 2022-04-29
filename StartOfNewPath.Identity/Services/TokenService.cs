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
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StartOfNewPath.Identity.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IGenericRepository<RefreshToken> _repository;
        private readonly IMapper _mapper;
        private readonly TokenSettings _tokenSettings;
        private readonly string _secretKey;
        private const int ExpiresTime = 10;

        public TokenService(IOptions<TokenSettings> settings, IGenericRepository<RefreshToken> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _tokenSettings = settings.Value;
            _secretKey = JWTSecret.SecretKey;
        }

        string ITokenService.GenerateAccessToken(IdentityUser user, IList<string> roles)
        {
            string token = string.Empty;
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

                var jwtTokenOptions = new JwtSecurityToken(
                    issuer: _tokenSettings.Issuer,
                    audience: _tokenSettings.Audience,
                    expires: DateTime.Now.AddMinutes(ExpiresTime),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                        SecurityAlgorithms.HmacSha256),
                    claims: userClaims);

                token = new JwtSecurityTokenHandler().WriteToken(jwtTokenOptions);
            }

            return token;
        }

        string ITokenService.GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        async Task ITokenService.SaveRefreshToken(RefreshTokenDto refreshToken)
        {
            var mapCourse = _mapper.Map<RefreshToken>(refreshToken);
            await _repository.CreateAsync(mapCourse);
        }

        bool ITokenService.ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _tokenSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _tokenSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
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

        void CheckRefreshToken(string refreshToken)
        {

        }
    }
}
