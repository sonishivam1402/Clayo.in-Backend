using e_commerce_backend.DTO;
using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecentlyViewedController : ControllerBase
    {
        private readonly IRecentlyViewedService _service;

        public RecentlyViewedController(IRecentlyViewedService service)
        {
            _service = service;
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetRecentlyViewed([FromBody] SetRecentlyViewedRequest request)
        {
            var result = await _service.SetRecentlyViewedItemAsync(request.UserId, request.ProductId);
            if (result.Status)
            {
                return Ok(new { message = result.Message });
            }
            return BadRequest(new { message = result.Message });
        }


        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetRecentlyViewed(Guid userId)
        {
            var items = await _service.GetRecentlyViewedItemsAsync(userId);
            return Ok(items);
        }
    }
}
