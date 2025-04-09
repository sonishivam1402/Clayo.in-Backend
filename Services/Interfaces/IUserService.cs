using e_commerce_backend.DTO;
using e_commerce_backend.Models;

namespace e_commerce_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<AuthResponse> AuthUser(string email, string password);

        Task<string> RegisterUser(RegisterUser request);
    }
}
