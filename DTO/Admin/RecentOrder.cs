namespace e_commerce_backend.DTO.Admin
{
    public class RecentOrder
    {
        public Guid OrderId { get; set; }
        public string OrderNo { get; set; }
        public string Status { get; set; }
        public DateTime PlacedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserName { get; set; }
     
    }
}
