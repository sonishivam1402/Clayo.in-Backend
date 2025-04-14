namespace e_commerce_backend.DTO.Order
{
    public class GetOrderDetails
    {
        public Guid OrderId { get; set; }
        public string Product_title { get; set; }
        public string Product_image { get; set; }
        public decimal Product_price { get; set; }
        public int Product_quantity { get; set; }
        public decimal Product_total_price { get; set; }
        public string Order_status { get; set; }
        public DateTime Order_date { get; set; }
        public DateTime Order_delivery_date { get; set; }

    }
}
