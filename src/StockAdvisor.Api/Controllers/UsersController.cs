using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

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
        public async Task<IActionResult> Post([FromBody]CreateUserCommand user)
        {
            var repositoryUser = await _userService.GetAsync(user.Email);

            if (repositoryUser != null)
            {
                return Conflict($"Given email {user.Email} is already used.");
            }
            
            await _userService.RegisterAsync(user.Email, user.FirstName, user.SurName,
                user.Password);
                
            // can add created user in the last parameter that is null in current implementation
            return CreatedAtAction(nameof(Get), new { email = user.Email }, null);
        }
    }
}
