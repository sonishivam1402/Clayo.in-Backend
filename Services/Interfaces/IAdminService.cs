using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Admin;
using e_commerce_backend.DTO.Order;

namespace e_commerce_backend.Services.Interfaces
{
    public interface IAdminService
    {
        Task<ServiceResponse<Dashboard>> GetDashboardData(Guid userId, Guid roleId);
        
        Task<StatusMessage> UpdateUserAccess(Guid userId);
        //Task<List<RecentOrder>> GetRecentOrders(int count);
        //Task<List<MostOrderProduct>> GetMostOrderedProducts(int count);
        //Task<List<User>> GetAllUsers();
        //Task<List<Product>> GetAllProducts();
        //Task<List<Order>> GetAllOrders();
        //Task<bool> UpdateUserStatus(string userId, bool isActive);
        //Task<bool> UpdateProductStatus(string productId, bool isActive);
        Task<StatusMessage> UpdateOrderStatus(Guid orderItemId, Guid status);
        Task<List<OrderStatus>> GetAllOrderStatus();
    }
}
