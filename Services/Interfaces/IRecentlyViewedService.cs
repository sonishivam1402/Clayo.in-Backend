using e_commerce_backend.DTO;

namespace e_commerce_backend.Services.Interfaces
{
    public interface IRecentlyViewedService
    {
        Task<StatusMessage> SetRecentlyViewedItemAsync(Guid userId, Guid productId);
        Task<List<RecentlyViewedItem>> GetRecentlyViewedItemsAsync(Guid userId);
    }
}
