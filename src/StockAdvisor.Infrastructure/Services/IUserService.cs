using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface IUserService : IService
    {
        Task<UserDto> GetAsync(string email);
        Task<UserDto> GetAsync(Guid userId);
        Task<IEnumerable<UserDto>> BrowseAsync();
        Task RegisterAsync(Guid userId, string email, string firstName, string surName,
            string password, UserRole userRole);
        Task ChangeUserPasswordAsync(Guid userId, string newPassword,
            string oldPassword);
        Task ChangeUserEmailAsync(Guid userId, string password,
            string newEmail);
        Task LoginAsync(string email, string Password);
    }
}