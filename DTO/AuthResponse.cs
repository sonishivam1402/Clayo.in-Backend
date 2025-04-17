using e_commerce_backend.Models;

namespace e_commerce_backend.DTO
{
    public class AuthResponse
    {
        public StatusMessage? StatusMessage { get; set; } 
        public Guid id { get; set; }
        public string name { get; set; }
        public Guid roleId { get; set; }
        public string? profileImage { get; set; }

        public Guid? cartId { get; set; }
        public string Token { get; internal set; }
    }
}
