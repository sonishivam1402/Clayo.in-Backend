using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Order;
using e_commerce_backend.Services.Interfaces;

namespace e_commerce_backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<StatusMessage> PlaceOrder(PlaceOrder order)
        {
            return await _orderRepository.PlaceOrder(order);
        }
        public async Task<IEnumerable<object>> GetOrderDetails(Guid? userId)
        {
            return await _orderRepository.GetOrderDetails(userId);
        }

        public async Task<StatusMessage> CancelOrder(Guid userId, Guid orderId, Guid orderItemId)
        {
            return await _orderRepository.CancelOrder(userId, orderId, orderItemId);
        }
    }    
    
}
