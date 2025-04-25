namespace e_commerce_backend.DTO.Order
{
    public class GetOrderDetails
    {
        public Guid OrderId { get; set; }  
        public string Status { get; set; }
        
        public string customer { get; set; }
        public DateTime PlacedAt { get; set; }
        
        public decimal TotalAmount { get; set; }

        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
        
    }
}
