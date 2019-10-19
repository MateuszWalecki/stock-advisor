using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockAdvisor.Core.Exceptions;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;
using StockAdvisor.Infrastructure.Settings;

namespace StockAdvisor.Api.Controllers
{
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ICommandDispatcher commandDistatcher)
            : base(commandDistatcher)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var user = await _userService.GetAsync(email);

            if (user == null)
            {
                return NotFound($"User with email {email} does not exists.");
            }

            return Ok(user);
        }

        // FromBody attribute provides conversion from the json request to the CreateUser command
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]CreateUserCommand command)
        {
            try
            {
                await CommandDistatcher.DispatchAsync(command);
            }
            catch (ServiceException e)
            {
                if (e.Code == Infrastructure.Exceptions.ErrorCodes.EmailInUse)
                {
                    return base.Conflict(e.Message);
                }
            }

            // can add created user in the last parameter that is null in current implementation
            return CreatedAtAction(nameof(Get), new { email = command.Email }, null);
        }
    }
}
