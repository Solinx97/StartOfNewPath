using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StartOfNewPath.Identity.Interfaces;
using StartOfNewPath.Identity.Security;
using StartOfNewPath.Models.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StartOfNewPath.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly IIdentityTokenService _tokenService;

        public AuthenticationController(UserManager<ApplicationUserModel> userManager, IIdentityTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized();
            }

            var claims = _tokenService.ValidateToken(refreshToken, JWTSecret.RefreshSecretKey, out var validateToken);
            if (!claims.Any())
            {
                return Unauthorized();
            }

            var foundToken = await _tokenService.FindRefreshTokenAsync(refreshToken);
            if (foundToken == null)
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");

                return Unauthorized();
            }

            var isExpiresed = DateTimeOffset.Now.UtcDateTime > validateToken.ValidTo;
            if (isExpiresed)
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");
                await _tokenService.RemoveRefreshTokenAsync(foundToken);

                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(foundToken.UserId);
            if (user == null)
            {
                return Unauthorized();
            }

            await _tokenService.CheckRefreshTokensByUserAsync(user.Id);

            if (!HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken))
            {
                await _tokenService.GenerateTokensAsync(HttpContext.Response.Cookies, user.Id);
            }

            return Ok(user);
        }
    }
}