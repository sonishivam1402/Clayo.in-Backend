using e_commerce_backend.DTO;

namespace e_commerce_backend.Data.Interfaces
{
    public interface IRecentlyViewedRepository
    {
        Task<StatusMessage> SetRecentlyViewedItemAsync(Guid userId, Guid productId);
        Task<List<RecentlyViewedItem>> GetRecentlyViewedItemsAsync(Guid userId);
    }
}
