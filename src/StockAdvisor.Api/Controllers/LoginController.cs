using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockAdvisor.Core.Exceptions;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Api.Controllers
{
    public class LoginController : ApiControllerBase
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserService _userService;
        
        public LoginController(ICommandDispatcher commandDistatcher,
            IUserService userService, IJwtHandler jwtHandler) : base(commandDistatcher)
        {
            _userService = userService;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCommand loginCommand)
        {
            try
            {
                await CommandDistatcher.DispatchAsync(loginCommand);
            }
            catch(StockAdvisorException e)
            {
                return Unauthorized(e.Message);    
            }

            var user = await _userService.GetAsync(loginCommand.Email);
            var token = _jwtHandler.CreateToken(loginCommand.Email, user.Role);

            return Ok(token);
        }
    }
}