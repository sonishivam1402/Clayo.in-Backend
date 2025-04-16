namespace e_commerce_backend.DTO.Order
{
    public class ShippingOrder
    {
        public Guid ShippingOrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }

        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public string Status { get; set; }
        public DateTime ShippingAt { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime DeliveredAt { get; set; }
    }
}
