using e_commerce_backend.DTO;
using e_commerce_backend.DTO.Cart;

namespace e_commerce_backend.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<GetCartItems>> GetAllCartItemsAsync(Guid userId);
        Task<StatusMessage> AddToCartAsync(AddToCart cartItem);
        Task<StatusMessage> DeleteCartItemAsync(Guid cartId);

        //Task<GetCartItems> GetCartItemByIdAsync(Guid cartId);
        //Task<bool> UpdateCartItemAsync(GetCartItems cartItem);
        //Task<bool> ClearCartAsync(Guid userId);
    }
}
