using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.Extensions;

namespace StockAdvisor.Api.Controllers
{
    public class LoginController : ApiControllerBase
    {
        private readonly IMemoryCache _cache;
        
        public LoginController(ICommandDispatcher commandDispatcher,
            IMemoryCache cache) : base(commandDispatcher)
        {
            _cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCommand loginCommand)
        {
            if (loginCommand != null)
            {
                loginCommand.TokenId = Guid.NewGuid();
            }

            await DispatchAsync(loginCommand);

            var token = _cache.GetJwt(loginCommand.TokenId);
            return Ok(token);
        }
    }
}