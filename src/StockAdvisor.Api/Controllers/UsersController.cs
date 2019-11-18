using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Api.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
            _userService = userService;
        }

        //TODO: for admin only
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.BrowseAsync();

            return Ok(users);
        }

        //TODO: for admin only
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

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]CreateUserCommand command)
        {
            await DispatchAsync(command);

            // can add created user in the last parameter that is null in current implementation
            return CreatedAtAction(nameof(Get), new { email = command.Email }, null);
        }
    }
}
