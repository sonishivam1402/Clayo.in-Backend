using e_commerce_backend.Models;

namespace e_commerce_backend.Data.Interfaces
{
    public interface IEmailRepository
    {
        Task SendEmailAsync<TModel>(SendEmailRequest request);
    }
}
