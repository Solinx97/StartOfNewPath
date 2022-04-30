using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StartOfNewPath.Models.User;
using System.Threading.Tasks;

namespace StartOfNewPath.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUserModel> _userManager;

        public AuthenticationController(UserManager<ApplicationUserModel> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }
    }
}