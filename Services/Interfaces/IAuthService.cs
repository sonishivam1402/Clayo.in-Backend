using e_commerce_backend.DTO;
using e_commerce_backend.Models;

namespace e_commerce_backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<SendOtpEmailRequest>> VerifyUserEmail(string email);
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken);
        Task LogoutAsync(Guid userId, string refreshToken);
    }
}
