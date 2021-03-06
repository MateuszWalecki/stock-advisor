using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface IInvestorService : IService
    {
        Task<InvestorDto> GetAsync(Guid userId);
        Task<InvestorDto> GetAsync(string email);
        Task<IEnumerable<InvestorDto>> BrowseAsync();
        Task RegisterAsync(Guid userId);
        Task AddToFavouriteCompaniesAsync(Guid userId, string company);
        Task RemoveFromFavouriteCompaniesAsync(Guid userId, string company);
        Task RemoveAsync(Guid userId);
    }
}