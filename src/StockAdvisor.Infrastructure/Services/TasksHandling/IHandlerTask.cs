using System;
using System.Threading.Tasks;
using StockAdvisor.Core.Exceptions;

namespace StockAdvisor.Infrastructure.Services.TasksHandling
{
    public interface IHandlerTask
    {
        IHandlerTask Always(Func<Task> always);
        IHandlerTask OnCustomError(Func<StockAdvisorException, Task> onCustomError,
            bool propagateException = false, bool executeOnError = false);
        IHandlerTask OnError(Func<Exception, Task> onError,
            bool propagateException = false, bool executeOnError = false);
        IHandlerTask OnSuccess(Func<Task> OnSuccess);
        IHandlerTask PropagateException();
        IHandlerTask DoNotPropagateException();
        IHandler Next();
        Task ExecuteAsync();
    }
}