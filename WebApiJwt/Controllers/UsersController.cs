using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiJwt.Model.InputModel;
using WebApiJwt.Services;

namespace WebApiJwt.Controllers
{
    [ApiController]

    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userService;
        public UsersController(IUserServices userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = _userService.Login(login);

            if (!string.IsNullOrEmpty(user))
            {
                response = Ok(new { token = user });
            }

            return response;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAllUsers());
        }
    }
}
