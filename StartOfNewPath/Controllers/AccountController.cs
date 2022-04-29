using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StartOfNewPath.Consts;
using StartOfNewPath.Identity.DTO;
using StartOfNewPath.Identity.Interfaces;
using StartOfNewPath.Models.Auth;
using StartOfNewPath.Models.User;
using System;
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
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(SignInManager<ApplicationUserModel> signInManager, 
            UserManager<ApplicationUserModel> userManager, ITokenService tokenService,
            IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationModel userRegistration)
        {
            if (userRegistration == null)
            {
                return BadRequest();
            }

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
            if (userLogin == null)
            {
                return BadRequest();
            }

            var response = await _signInManager.PasswordSignInAsync(userLogin.UserName, userLogin.Password, false, false);
            if (response.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);

                if (user == null)
                {
                    return NotFound();
                }

                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = _tokenService.GenerateAccessToken(user, roles);
                HttpContext.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });

                var refreshToken = _tokenService.GenerateRefreshToken();
                var refreshTokenOptions = new RefreshTokenModel
                {
                    Id = "123tes23",
                    Token = refreshToken,
                    Expires = DateTimeOffset.Now.AddHours(1),
                    UserId = user.Id
                }; 

                var map = _mapper.Map<RefreshTokenDto>(refreshTokenOptions);
                await _tokenService.SaveRefreshToken(map);
                HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });

                //var result = new { roles = roles, user = user };
                return Ok();
            }

            return Forbid();
        }

        //[Authorize]
        //[HttpPost("refresh")]
        //public async Task<IActionResult> CheckRefreshToken(string refreshToken)
        //{
        //    await _userManager.Get(user, _jwtSettings.Value.AuthScheme, "RefresjToken", "jwt_refresh_token");

        //    return null;
        //}
    }
}
