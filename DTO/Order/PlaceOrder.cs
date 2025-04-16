namespace e_commerce_backend.DTO.Order
{
    public class PlaceOrder
    {
        public Guid userId { get; set; }
        public Guid cartId { get; set; }
        public string CartItemIds { get; set; }
    }
}
