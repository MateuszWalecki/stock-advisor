using System.Threading.Tasks;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(string email);
        Task RegisterAsync(string email, string firstName, string surName,
            string password);
    }
}