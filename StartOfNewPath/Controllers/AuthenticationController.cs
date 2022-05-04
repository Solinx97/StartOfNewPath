using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StartOfNewPath.Identity.Interfaces;
using StartOfNewPath.Models.User;
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

            var refreshTokenIsValidated = await _tokenService.RefreshAsync(refreshToken);
            if (!refreshTokenIsValidated)
            {
                Response.Cookies.Delete("refreshToken");
                return Unauthorized();
            }

            var users = _userManager.Users.ToList();
            if (!users.Any())
            {
                return Unauthorized();
            }

            var currentUser = users.First();
            if (!HttpContext.Request.Cookies.TryGetValue("accessToken", out var accessToken))
            {
                var roles = await _userManager.GetRolesAsync(currentUser);
                var newAccessToken = _tokenService.GenerateAccessToken(currentUser, roles);
                HttpContext.Response.Cookies.Append("accessToken", newAccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });
            }

            return Ok(currentUser);
        }
    }
}