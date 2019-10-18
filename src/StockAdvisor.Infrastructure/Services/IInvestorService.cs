using System;
using System.Threading.Tasks;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface IInvestorService : IService
    {
        Task<InvestorDto> GetAsync(Guid userId);
        Task RegisterAsync(Guid userId);
    }
}