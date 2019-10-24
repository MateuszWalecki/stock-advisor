using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Exceptions;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Investors;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Api.Controllers
{
    public class InvestorsController : ApiControllerBase
    {
        private readonly IInvestorService _investorService;
        
        public InvestorsController(ICommandDispatcher commandDispatcher,
            IInvestorService investorService) : base(commandDispatcher)
        {
            _investorService = investorService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var investors = await _investorService.BrowseAsync();

            if (investors == null || !investors.Any())
            {
                return NotFound($"Not found any investor.");
            }

            return Ok(investors);
        }
        
        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var investor = await _investorService.GetAsync(email);

            if (investor == null)
            {
                return NotFound($"Investor realted with user with email {email } " +
                    "cannot be found.");
            }

            return Ok(investor);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateInvestorCommand command)
        {
            await DispatchAsync(command);

            return NoContent();
        }
    }
}
