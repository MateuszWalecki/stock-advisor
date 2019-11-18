using System.Dynamic;
using System.Threading.Tasks;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public interface IDataInitializer : IService
    {
        Task SeedDefaultAsync();

        //For testing purposes
        Task<ExpandoObject> AddAndGetNextUserWithInvestor(); 
        Task<ExpandoObject> AddAndGetNextUserWithoutInvestor();
    }
}