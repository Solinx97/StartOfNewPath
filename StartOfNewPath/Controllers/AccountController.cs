using Microsoft.AspNetCore.Authorization;
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
        private readonly IJWTService _jwtTokenService;

        public AccountController(SignInManager<ApplicationUserModel> signInManager, 
            UserManager<ApplicationUserModel> userManager, IJWTService jwtTokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
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

            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.User);

                return Ok(_jwtTokenService.GetToken(user));
            }
            else
            {
                var errorCodes = new List<string>();
                foreach (var item in result.Errors)
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

            var result = await _signInManager.PasswordSignInAsync(userLogin.UserName, userLogin.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);

                if (user == null)
                {
                    return NotFound();
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles == null)
                {
                    return NotFound();
                }

                return Ok(_jwtTokenService.GetToken(user, roles));
            }

            return Forbid();
        }
    }
}
