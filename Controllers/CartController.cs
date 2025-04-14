﻿using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Cart;
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


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllCartItems(Guid userId)
        {
            var cartItems = await _cartService.GetAllCartItemsAsync(userId);
            if (cartItems.GetType() == typeof(List<GetCartItems>))
            {
                return Ok(cartItems);
            }
            else
            {
                return NotFound(cartItems);
            }
            
        }

        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCart cartItem)
        {
            var result = await _cartService.AddToCartAsync(cartItem);
            if (result.Status)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("delete/{cartId}")]
        public async Task<IActionResult> DeleteCartItem( Guid cartId)
        {
            var result = await _cartService.DeleteCartItemAsync(cartId);
            if (result.Status)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

    }
}
