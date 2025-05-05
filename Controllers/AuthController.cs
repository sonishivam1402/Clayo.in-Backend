using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using e_commerce_backend.Models;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.LoginRequest request)
        {
            var response = await _authService.LoginAsync(request.Email, request.Password);

            if (response.isSuccess)
                return Ok(response);

            return Unauthorized(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Models.RefreshRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);

            if (response.isSuccess)
                return Ok(response);

            return Unauthorized(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await _authService.LogoutAsync(request.UserId, request.RefreshToken);
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
