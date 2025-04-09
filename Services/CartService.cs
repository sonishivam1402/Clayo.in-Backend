using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO.Cart;
using e_commerce_backend.Services.Interfaces;

namespace e_commerce_backend.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<GetCartItems>> GetAllCartItemsAsync(Guid userId)
        {
            return await _cartRepository.GetAllCartItemsAsync(userId);
        }
    }
}
