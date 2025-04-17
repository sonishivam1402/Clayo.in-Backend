using e_commerce_backend.DTO;
using e_commerce_backend.Models;

namespace e_commerce_backend.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUserById(Guid id);

        Task<AuthResponse> AuthenticateUser(string email, string password);

        Task<string> RegisterUser(RegisterUser request);

        //Task UpdateAsync(User user);
        //Task DeleteAsync(Guid id);
    }
}
