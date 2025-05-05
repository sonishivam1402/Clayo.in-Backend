using e_commerce_backend.DTO;
using e_commerce_backend.Models;

namespace e_commerce_backend.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUserById(Guid id);

        //Task<AuthResponse> AuthenticateUser(string email, string password);

        Task<ServiceResponse<SendOtpEmailRequest>> AddOrUpdateUsers(AddOrUpdateUsers request);

        Task<StatusMessage> VerifyUser(VerifyAndUseOtp reuest);

        //Task UpdateAsync(User user);
        //Task DeleteAsync(Guid id);
    }
}
