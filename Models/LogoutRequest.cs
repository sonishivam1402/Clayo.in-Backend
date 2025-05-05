namespace e_commerce_backend.Models
{
    public class LogoutRequest
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
