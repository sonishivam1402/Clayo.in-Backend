namespace e_commerce_backend.DTO
{
    public class SetRecentlyViewedRequest
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
