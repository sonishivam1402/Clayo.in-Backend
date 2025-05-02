using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO.Order;
using e_commerce_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository _emailRespository;

        public EmailController(IEmailRepository emailRespository)
        {
            _emailRespository = emailRespository;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(SendEmailRequest request)
        {
            await _emailRespository.SendEmailAsync<GetOrderDetails>(request);
            return Ok("Email sent successfully!");
        }
    }
}
