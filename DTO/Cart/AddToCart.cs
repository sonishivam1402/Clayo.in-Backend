namespace e_commerce_backend.DTO.Cart
{
    public class AddToCart
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid UserId { get; set; }
    }
}
