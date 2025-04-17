using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Order;
using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrder order)
        {
            var result = await _orderService.PlaceOrder(order);
            if (result.Status)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("GetOrderDetails/{userId}")]
        public async Task<IActionResult> GetOrderDetails(Guid userId)
        {
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
    }
}
