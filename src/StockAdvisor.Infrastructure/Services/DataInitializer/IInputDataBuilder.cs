using System.Threading.Tasks;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public interface IInputDataBuilder : IService
    {
        Task<UserWrapperForTesting> Build(); 
    }
}