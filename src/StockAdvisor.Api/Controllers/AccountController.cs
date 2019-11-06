using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Exceptions;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Api.Controllers
{
    public class AccountController : ApiControllerBase
    {
        public AccountController(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }

        [Authorize]
        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody]ChangeUserPasswordCommand command)
        {
            await DispatchAsync(command);

            return NoContent();
        }

        [Authorize]
        [HttpPut]
        [Route("email")]
        public async Task<IActionResult> ChangeEmailAsync([FromBody]ChangeUserEmailCommand command)
        {
            await DispatchAsync(command);

            return NoContent();
        }
    }
}
