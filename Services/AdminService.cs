using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Admin;
using e_commerce_backend.DTO.Order;
using e_commerce_backend.Services.Interfaces;

namespace e_commerce_backend.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<ServiceResponse<Dashboard>> GetDashboardData(Guid userId, Guid roleId)
        {
            var response = await _adminRepository.GetDashboardData(userId, roleId);
            return response;
        }

        public async Task<StatusMessage> UpdateUserAccess(Guid userId)
        {
            var response = await _adminRepository.UpdateUserAccess(userId);
            return response;
        }

        public async Task<StatusMessage> UpdateOrderStatus(Guid orderItemId, Guid status)
        {
            var response = await _adminRepository.UpdateOrderStatus(orderItemId, status);
            return response;
        }

        public async Task<List<OrderStatus>> GetAllOrderStatus()
        {
            var response = await _adminRepository.GetAllOrderStatus();
            return response;
        }
    }
}
