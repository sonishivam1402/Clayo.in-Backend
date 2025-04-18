using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Cart;

namespace e_commerce_backend.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<object>> GetAllCartItemsAsync(Guid cartId, Guid userId);
        Task<StatusMessage> AddToCartAsync(AddToCart cartItem);
        Task<StatusMessage> DeleteCartItemAsync(Guid cartId, Guid productId);

        //Task<GetCartItems> GetCartItemByIdAsync(Guid cartId);
        //Task<bool> UpdateCartItemAsync(GetCartItems cartItem);
        //Task<bool> ClearCartAsync(Guid userId);
    }
}
