using e_commerce_backend.Models;

namespace e_commerce_backend.DTO
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public User? User { get; set; }
    }
}
