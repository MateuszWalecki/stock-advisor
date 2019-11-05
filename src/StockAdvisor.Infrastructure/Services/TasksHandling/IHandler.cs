using System;
using System.Threading.Tasks;

namespace StockAdvisor.Infrastructure.Services.TasksHandling
{
    public interface IHandler : IService
    {
        IHandlerTask Run(Func<Task> run);
        IHandlerTaskRunner Validate(Func<Task> validate);
        Task ExecuteAllAsync();
    }
}