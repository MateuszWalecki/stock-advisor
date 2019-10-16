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
        public async Task<UserDto> Get(string email)
        {
            return await _userService.GetAsync(email);
        }

        // FromBody attribute provides conversion from the json request to the CreateUser command
        [HttpPost]
        public async Task Post([FromBody]CreateUserCommand request)
        {
            await _userService.RegisterAsync(request.Email, request.FirstName, request.SurName,
                request.Password);
        }
    }
}
