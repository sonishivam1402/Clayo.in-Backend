using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Cart;
using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetAllCartItems(Guid cartId)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User Id not found");

            var cartItems = await _cartService.GetAllCartItemsAsync(cartId, (Guid)userId);
            if (cartItems.GetType() == typeof(List<GetCartItems>))
            {
                return Ok(cartItems);
            }
            else
            {
                return NotFound(cartItems);
            }    
        }

        [Authorize]
        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCart cartItem)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User Id not found");

            cartItem.UserId = (Guid)userId;

            var result = await _cartService.AddToCartAsync(cartItem);
            if (result.Status)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpDelete("delete/{cartId}/{cartItemId}/{productId}")]
        public async Task<IActionResult> DeleteCartItem( Guid cartId, Guid cartItemId, Guid productId)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User Id not found");

            var result = await _cartService.DeleteCartItemAsync((Guid)userId, cartId, cartItemId, productId);
            if (result.Status)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

    }
}
