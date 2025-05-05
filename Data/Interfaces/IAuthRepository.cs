using e_commerce_backend.Models;

namespace e_commerce_backend.Data.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthResponse> ValidateUserAsync(string email, string password);
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime expiresAt);
        Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
        Task RevokeAndSaveNewRefreshTokenAsync(Guid userId, string oldToken, string newToken, DateTime expiresAt);
        Task RevokeRefreshTokenAsync(Guid userId, string refreshToken);
    }
}
