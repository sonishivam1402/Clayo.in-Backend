using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Admin;
using e_commerce_backend.DTO.Order;

namespace e_commerce_backend.Data.Interfaces
{
    public interface IAdminRepository
    {
        Task<ServiceResponse<Dashboard>> GetDashboardData(Guid userId, Guid roleId);

        //Task<List<RecentOrder>> GetRecentOrders(int count);
        //Task<List<MostOrderProduct>> GetMostOrderedProducts(int count);
        //Task<List<User>> GetAllUsers();
        //Task<List<Product>> GetAllProducts();
        //Task<List<Order>> GetAllOrders();
        Task<StatusMessage> UpdateUserAccess(Guid userId);
        //Task<bool> UpdateProductStatus(string productId, bool isActive);
        Task<StatusMessage> UpdateOrderStatus(Guid orderItemId, Guid status);

        Task<List<OrderStatus>> GetAllOrderStatus();
    }
}
