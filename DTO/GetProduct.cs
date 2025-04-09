namespace e_commerce_backend.DTO
{
    public class GetProduct
    {
        public Guid ProductId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public string category { get; set; }

        public string image { get; set; }

        public decimal rating_rate { get; set; }

        public int rating_count { get; set; }
    }
}
