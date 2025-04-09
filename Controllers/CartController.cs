using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost("{userId}")]
        public async Task<IActionResult> GetAllCartItems([FromBody]Guid userId)
        {
            var cartItems = await _cartService.GetAllCartItemsAsync(userId);
            return Ok(cartItems);
        }
        
    }
}
