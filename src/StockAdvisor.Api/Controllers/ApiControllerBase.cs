using Microsoft.AspNetCore.Mvc;
using StockAdvisor.Infrastructure.Commands;

namespace StockAdvisor.Api.Controllers
{
    [Route("[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        protected readonly ICommandDispatcher CommandDispatcher;

        protected ApiControllerBase(ICommandDispatcher commandDispatcher)
        {
            CommandDispatcher = commandDispatcher;
        }
    }
}