using e_commerce_backend.DTO;
using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.AuthUser(request.Email, request.Password);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized(new {result.Message});
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser request)
        {
            var result = await _userService.RegisterUser(request);
            return Ok(new { Message = result });
        }
    }
}
