using System.Dynamic;
using System.Threading.Tasks;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface IDataInitializer : IService
    {
        Task SeedAsync();

        //For testing purposes
        Task<ExpandoObject> AddAndGetNextUserWithInvestor(); 
        Task<ExpandoObject> AddAndGetNextUserWithoutInvestor();
    }
}