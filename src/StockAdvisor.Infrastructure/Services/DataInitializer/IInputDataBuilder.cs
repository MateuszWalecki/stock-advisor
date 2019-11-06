using System.Threading.Tasks;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public interface IInputDataBuilder
    {
        Task<dynamic> Build(); 
    }
}