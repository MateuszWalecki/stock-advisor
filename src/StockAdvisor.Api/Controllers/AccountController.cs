﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService, ICommandDispatcher commandDistatcher)
            : base(commandDistatcher)
        {
            _userService = userService;
        }

        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangeUserPasswordCommand command)
        {
            await CommandDistatcher.DispatchAsync(command);

            return NoContent();
        }
    }
}
