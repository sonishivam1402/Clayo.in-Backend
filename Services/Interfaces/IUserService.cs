using e_commerce_backend.DTO;
using e_commerce_backend.Models;

namespace e_commerce_backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        
        Task<User> GetUserById(Guid id);
        //Task<AuthResponse> AuthUser(string email, string password);

        Task<StatusMessage> AddOrUpdateUsers(AddOrUpdateUsers request);

        Task<StatusMessage> VerifyUser(VerifyAndUseOtp reuest);
    }
}
