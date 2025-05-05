using e_commerce_backend.DTO;

namespace e_commerce_backend.Models
{
    public class AuthResponse
    {
        //public StatusMessage? StatusMessage { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public string email { get; set; }
        public Guid roleId { get; set; }
        public string? profileImage { get; set; }

        public Guid? cartId { get; set; }
        public string Token { get; internal set; }
        public string RefreshToken { get; set; }
        public bool isSuccess { get; set; }
        public string Message { get; set; }

    }
}
