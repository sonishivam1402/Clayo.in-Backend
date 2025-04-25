using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Order;

namespace e_commerce_backend.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<StatusMessage> PlaceOrder(PlaceOrder order);
        Task<IEnumerable<object>> GetOrderDetails(Guid? userId);

        Task<StatusMessage> CancelOrder(Guid orderId);
    }
}
