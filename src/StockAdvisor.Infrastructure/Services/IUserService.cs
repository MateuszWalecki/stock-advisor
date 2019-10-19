using System;
using System.Threading.Tasks;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface IUserService : IService
    {
        Task<UserDto> GetAsync(string email);
        Task RegisterAsync(string email, string firstName, string surName,
            string password);
        Task ChangeUserPasswordAsync(Guid userId, string newPassword,
            string oldPassword);
        Task LoginAsync(string email, string Password);
    }
}