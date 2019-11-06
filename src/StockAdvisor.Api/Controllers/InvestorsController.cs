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

        // TODO: for admin only
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
        
        [Authorize]
        [HttpGet]
        [Route("me")]
        public async Task<IActionResult> GetCurrentIvestor()
        {
            var userId = UserId;

            if (userId == Guid.Empty)
            {
                return NotFound($"User with id {userId} cannot be found.");
            }

            var investor = await _investorService.GetAsync(userId);

            if (investor == null)
            {
                return NotFound($"Investor realted with user with id {userId} " +
                    "cannot be found.");
            }

            return Ok(investor);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNewInvestor()
        {
            var command = new CreateInvestorCommand();

            await DispatchAsync(command);

            return NoContent();
        }

        [Authorize]
        [Route("me")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCurrentInvestor()
        {
            await DispatchAsync(new DeleteInvestorCommand());

            return NoContent();
        }

        [Authorize]
        [Route("companies")]
        [HttpPost]
        public async Task<IActionResult> AddFavouriteCompany(
            [FromBody]AddFavouriteCompanyCommand command)
        {
            await DispatchAsync(command);

            return NoContent();
        }

        [Authorize]
        [Route("companies/{symbol}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFavouriteCompany(
            string symbol)
        {
            var command = new DeleteFavouriteCompanyCommand()
            {
                CompanySymbol = symbol
            };

            await DispatchAsync(command);

            return NoContent();
        }
    }
}
