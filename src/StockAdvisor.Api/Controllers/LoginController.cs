using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StockAdvisor.Core.Exceptions;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.Extensions;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Api.Controllers
{
    public class LoginController : ApiControllerBase
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IMemoryCache _cache;
        
        public LoginController(ICommandDispatcher commandDispatcher,
            IMemoryCache cache, IJwtHandler jwtHandler) : base(commandDispatcher)
        {
            _cache = cache;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCommand loginCommand)
        {
            loginCommand.TokenId = Guid.NewGuid();
            
            await DispatchAsync(loginCommand);

            var token = _cache.GetJwt(loginCommand.TokenId);
            return Ok(token);
        }
    }
}