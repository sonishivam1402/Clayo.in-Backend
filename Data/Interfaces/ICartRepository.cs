using e_commerce_backend.DTO.Cart;

namespace e_commerce_backend.Data.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<GetCartItems>> GetAllCartItemsAsync(Guid userId);
        //Task<GetCartItems> GetCartItemByIdAsync(Guid cartId);
        //Task<bool> AddToCartAsync(GetCartItems cartItem);
        //Task<bool> UpdateCartItemAsync(GetCartItems cartItem);
        //Task<bool> DeleteCartItemAsync(Guid cartId);
        //Task<bool> ClearCartAsync(Guid userId);
    }
}
