using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Order;
using e_commerce_backend.Models;
using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IEmailRepository _emailRepository;
        public OrderController(IOrderService orderService, IEmailRepository emailRepository)
        {
            _orderService = orderService;
            _emailRepository = emailRepository;
        }

        [Authorize]
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrder order)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User Id not found");

            order.userId = (Guid)userId;
            var result = await _orderService.PlaceOrder(order);
            if (result.Status)
            {
                // Get Order details to send email

                var orderDetails = (await _orderService.GetOrderDetails(order.userId)).FirstOrDefault();
                Console.WriteLine(orderDetails);
                if (orderDetails.GetType() == typeof(List<StatusMessage>))
                {
                    return NotFound(orderDetails);
                }
                else
                {
                    var modelJson = JsonSerializer.Serialize(orderDetails);
                    Console.WriteLine(modelJson);
                    // Send email with order details
                    var emailRequest = new SendEmailRequest
                    {
                        ToEmail = order.Email,
                        TemplateName = "OrderStatus",
                        ModelJson = modelJson
                    };

                    // Trigger the SendEmail method
                    await _emailRepository.SendEmailAsync<GetOrderDetails>(emailRequest);

                    return Ok(result);
                }
            }
            else
            {
                return BadRequest(result);
            }
        }

        [Authorize]
        [HttpGet("GetOrderDetails")]
        public async Task<IActionResult> GetOrderDetails()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User Id not found");

            var result = await _orderService.GetOrderDetails(userId);
            if(result.GetType() == typeof(List<StatusMessage>))
            {
                return NotFound(result);
            }
            else
            {
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("CancelOrder")]
        public async Task<IActionResult> CancelOrder([FromQuery] Guid orderId)
        {
            var result = await _orderService.CancelOrder(orderId);
            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
