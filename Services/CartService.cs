using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
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

        public async Task<IEnumerable<object>> GetAllCartItemsAsync(Guid userId)
        {
            return await _cartRepository.GetAllCartItemsAsync(userId);
        }

        public async Task<StatusMessage> AddToCartAsync(AddToCart cartItem)
        {
            return await _cartRepository.AddToCartAsync(cartItem);
        }

        public async Task<StatusMessage> DeleteCartItemAsync(Guid cartId)
        {
            return await _cartRepository.DeleteCartItemAsync(cartId);
        }
    }
}
