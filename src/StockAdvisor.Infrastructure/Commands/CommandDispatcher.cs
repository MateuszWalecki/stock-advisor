using System;
using System.Threading.Tasks;
using Autofac;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _componentContext;

        public CommandDispatcher(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public async Task DispatchAsync<T>(T command) where T : ICommand
        {
            if (command == null)
            {
                throw new CommandNullSerExc($"Command {typeof(T).Name} cannot be null.");
            }

            var handler = _componentContext.Resolve<ICommandHandler<T>>();
            await handler.HandleAsync(command);
        }
    }
}