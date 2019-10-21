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
        
        public InvestorsController(ICommandDispatcher commandDistatcher,
            IInvestorService investorService) : base(commandDistatcher)
        {
            _investorService = investorService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var investors = await _investorService.BrowseAsync();

            return Ok(investors);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateInvestorCommand command)
        {
            await CommandDistatcher.DispatchAsync(command);

            return NoContent();
        }
    }
}
