using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StartOfNewPath.Consts;
using StartOfNewPath.Identity.Interfaces;
using StartOfNewPath.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StartOfNewPath.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUserModel> _signInManager;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly IIdentityTokenService _tokenService;

        public AccountController(SignInManager<ApplicationUserModel> signInManager, 
            UserManager<ApplicationUserModel> userManager, IIdentityTokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationModel userRegistration)
        {
            var user = new ApplicationUserModel
            {
                UserName = userRegistration.UserName,
                FirstName = userRegistration.FirstName,
                Surname = userRegistration.Surname,
                Email = userRegistration.Email
            };

            var response = await _userManager.CreateAsync(user, userRegistration.Password);
            if (response.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Quest);
                return Ok();
            }
            else
            {
                var errorCodes = new List<string>();
                foreach (var item in response.Errors)
                {
                    errorCodes.Add(item.Code);
                }

                return BadRequest(errorCodes);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel userLogin)
        {
            var response = await _signInManager.PasswordSignInAsync(userLogin.UserName, userLogin.Password, false, false);
            if (response.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);
                if (user == null)
                {
                    return BadRequest();
                }

                var roles = await _userManager.GetRolesAsync(user);

                var accessToken = _tokenService.GenerateAccessToken(user, roles);
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);

                var cookies = HttpContext.Response.Cookies;
                cookies.Append("accessToken", accessToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });
                cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });

                return Ok(user);
            }

            return Forbid();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Response.Cookies.Delete("accessToken");
            HttpContext.Response.Cookies.Delete("refreshToken");

            return Ok();
        }
    }
}
