using e_commerce_backend.DTO;
using e_commerce_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;
        private readonly IOrderService _orderService;
        public AdminController(IAdminService adminService, IOrderService orderService)
        {
            _adminService = adminService;
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            var hasAccess = GetRoleAccess();
            if (!hasAccess) return Unauthorized("You are not authorized to access this page.");

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("User Id not found");

            var roleId = GetRoleId();
            if (roleId == null)
                return Unauthorized("Role Id not found");

            var response = await _adminService.GetDashboardData((Guid)userId, (Guid)roleId);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize]
        [HttpPost("updateUserAccess/{userId}")]
        public async Task<IActionResult> UpdateUserAccess(Guid userId)
        {
            var hasAccess = GetRoleAccess();
            if (!hasAccess) return Unauthorized("You are not authorized to access this page.");

            var response = await _adminService.UpdateUserAccess(userId);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize]
        [HttpPost("updateOrderStatus/{orderItemId}/{status}")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderItemId, Guid status)
        {
            var hasAccess = GetRoleAccess();
            if (!hasAccess) return Unauthorized("You are not authorized to access this page.");

            var response = await _adminService.UpdateOrderStatus(orderItemId, status);
            if (response.Status)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("getAllOrderStatus")]
        public async Task<IActionResult> GetAllOrderStatus()
        {
            var hasAccess = GetRoleAccess();
            if (!hasAccess) return Unauthorized("You are not authorized to access this page.");

            var response = await _adminService.GetAllOrderStatus();
            if (response != null)
            {
                return Ok(response);
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet("GetOrderDetails")]
        public async Task<IActionResult> GetOrderDetails()
        {
            var hasAccess = GetRoleAccess();
            if (!hasAccess) return Unauthorized("You are not authorized to access this page.");

            var result = await _orderService.GetOrderDetails(null);
            if (result.GetType() == typeof(List<StatusMessage>))
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
