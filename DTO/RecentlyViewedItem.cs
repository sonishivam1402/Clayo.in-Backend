namespace e_commerce_backend.DTO
{
    public class RecentlyViewedItem
    {
        public Guid userId { get; set; }
        public Guid productId { get; set; }
        public DateTime ViewedAt { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
