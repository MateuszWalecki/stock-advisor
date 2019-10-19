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
        
        public LoginController(ICommandDispatcher commandDistatcher,
            IMemoryCache userService, IJwtHandler jwtHandler) : base(commandDistatcher)
        {
            _cache = userService;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCommand loginCommand)
        {
            loginCommand.TokenId = Guid.NewGuid();
            
            try
            {
                await CommandDistatcher.DispatchAsync(loginCommand);
            }
            catch(StockAdvisorException e)
            {
                return Unauthorized(e.Message);    
            }

            var token = _cache.GetJwt(loginCommand.TokenId);
            return Ok(token);
        }
    }
}