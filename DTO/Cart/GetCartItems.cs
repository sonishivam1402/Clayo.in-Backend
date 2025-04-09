namespace e_commerce_backend.DTO.Cart
{
    public class GetCartItems
    {
        public Guid cartId { get; set; }
        public Guid productId { get; set; }
        public Guid userId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public string category { get; set; }
        public string image { get; set; }
        public decimal rating_rate { get; set; }
        public int rating_count { get; set; }

    }
}
