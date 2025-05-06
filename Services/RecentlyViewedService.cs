using e_commerce_backend.Data.Interfaces;
using e_commerce_backend.DTO;
using e_commerce_backend.Services.Interfaces;

namespace e_commerce_backend.Services
{
    public class RecentlyViewedService : IRecentlyViewedService
    {
        private readonly IRecentlyViewedRepository _repository;

        public RecentlyViewedService(IRecentlyViewedRepository repository)
        {
            _repository = repository;
        }

        public async Task<StatusMessage> SetRecentlyViewedItemAsync(Guid userId, Guid productId)
        {
            return await _repository.SetRecentlyViewedItemAsync(userId, productId);
        }

        public async Task<List<RecentlyViewedItem>> GetRecentlyViewedItemsAsync(Guid userId)
        {
            return await _repository.GetRecentlyViewedItemsAsync(userId);
        }
    }
}
