using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using e_commerce_backend.Models;
using e_commerce_backend.DTO;
using e_commerce_backend.Services;
using System.Text.Json;
using e_commerce_backend.Data.Interfaces;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailRepository _emailRepository;

        public AuthController(IAuthService authService, IEmailRepository emailRepository)
        {
            _authService = authService;
            _emailRepository = emailRepository;
        }

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var result = await _authService.VerifyUserEmail(request.Email);
            if (result.Status)
            {
                var modelObj = new { Otp = result.Data.Otp };
                string jsonModel = JsonSerializer.Serialize(modelObj);

                var emailRequest = new SendEmailRequest
                {
                    ToEmail = result.Data.Email,
                    TemplateName = "Otp Verification",
                    ModelJson = jsonModel
                };

                // Trigger the SendEmail method
                await _emailRepository.SendEmailAsync<SendOtpEmailRequest>(emailRequest);
                return Ok(new { Message = result.Message});
            }
            else
            {
                return BadRequest(result.Message);
            }
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
