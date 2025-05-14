using e_commerce_backend.DTO;
using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecentlyViewedController : BaseController
    {
        private readonly IRecentlyViewedService _service;

        public RecentlyViewedController(IRecentlyViewedService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("set")]
        public async Task<IActionResult> SetRecentlyViewed([FromBody] SetRecentlyViewedRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User Id not found");

            var result = await _service.SetRecentlyViewedItemAsync((Guid)userId, request.ProductId);
            if (result.Status)
            {
                return Ok(new { message = result.Message });
            }
            return BadRequest(new { message = result.Message });
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetRecentlyViewed()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User Id not found");

            var items = await _service.GetRecentlyViewedItemsAsync((Guid)userId);
            return Ok(items);
        }
    }
}
