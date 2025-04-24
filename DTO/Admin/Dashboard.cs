namespace e_commerce_backend.DTO.Admin
{
    public class Dashboard
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        
        public List<RecentOrder> RecentOrders { get; set; } = new List<RecentOrder>();
        public List<MostOrderProduct> MostOrderedProducts { get; set; } = new List<MostOrderProduct>();
    }
}
