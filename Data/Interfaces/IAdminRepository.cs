using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Admin;

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
        //Task<bool> UpdateUserStatus(string userId, bool isActive);
        //Task<bool> UpdateProductStatus(string productId, bool isActive);
        //Task<bool> UpdateOrderStatus(string orderId, string status);
    }
}
