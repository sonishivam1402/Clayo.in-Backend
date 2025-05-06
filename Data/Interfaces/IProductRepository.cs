using e_commerce_backend.DTO;

namespace e_commerce_backend.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<GetProduct>> GetAllProductsAsync();

        Task<GetProduct> GetProductByIdAsync(Guid productId);
        Task<StatusMessage> AddOrUpdateProduct(PostProduct product);
    }
}
