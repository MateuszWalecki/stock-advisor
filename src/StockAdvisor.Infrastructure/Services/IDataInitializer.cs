using System.Threading.Tasks;

namespace StockAdvisor.Infrastructure.Services
{
    public interface IDataInitializer : IService
    {
        Task SeedAsync();
    }
}