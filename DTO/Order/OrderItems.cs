namespace e_commerce_backend.DTO.Order
{
    public class OrderItems
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string Title { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
        public string image { get; set; }
    }
}
