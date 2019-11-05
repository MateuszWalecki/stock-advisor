using System;
using System.Threading.Tasks;

namespace StockAdvisor.Infrastructure.Services.TasksHandling
{
    public interface IHandlerTaskRunner
    {
        IHandlerTask Run(Func<Task> run);
    }
}