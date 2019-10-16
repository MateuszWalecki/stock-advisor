using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;
using Xunit;

namespace StockAdvisor.UnitTests.Infrastructure
{
    public class InvestorServiceTests
    {
        [Fact]
        public async Task register_async_should_invoke_add_async_on_repository()
        {
            //Given
            var userId = Guid.NewGuid();
            var investor = new Investor(userId);

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(userId))
                                  .Returns(Task.FromResult((Investor)null));

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(investorRepositoryMock.Object, mapperMock.Object);

            //When
            await investorService.RegisterAsync(userId);
            
            //Then
            investorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Investor>()), Times.Once);
        }

        [Fact]
        public async Task trying_to_register_investor_related_with_user_id_that_is_in_use_serviceexception_is_thrown()
        {
            //Given
            var userId = Guid.NewGuid();
            var investor = new Investor(userId);

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(userId))
                                  .Returns(Task.FromResult(investor));


            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(investorRepositoryMock.Object, mapperMock.Object);

            //When
            Func<Task> act = () => investorService.RegisterAsync(userId);
            
            //Then
            await Assert.ThrowsAsync<ServiceException>(act);
        } 

        
        [Fact]
        public async Task get_async_should_invoke_get_async_on_repository()
        {
            //Given
            var userId = Guid.NewGuid();
            var investor = new Investor(userId);

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(userId))
                                  .Returns(Task.FromResult(investor));


            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(investorRepositoryMock.Object, mapperMock.Object);

            //When
            await investorService.GetAsync(userId);
            
            //Then
            investorRepositoryMock.Verify(x => x.GetAsync(userId), Times.Once);
        }

        [Fact]
        public async Task get_async_returns_mapped_investor_if_it_exists()
        {
            //Given
            var userId = Guid.NewGuid();
            var investor = new Investor(userId);
            investor.AddToFavouriteCompanies("AAPL");
            investor.AddToFavouriteCompanies("MSFN");

            var expectedInvestorDto = new InvestorDto()
            {
                UserId = investor.UserId,
                UpdatedAt = investor.UpdatedAt,
                FavouriteCompanies = new List<string>(investor.FavouriteCompanies)
            };

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(userId))
                                  .Returns(Task.FromResult(investor));


            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<Investor, InvestorDto>(It.IsAny<Investor>()))
                      .Returns(expectedInvestorDto);

            var investorService = new InvestorService(investorRepositoryMock.Object, mapperMock.Object);

            //When
            var occuredInvestorDto = await investorService.GetAsync(userId);
            
            //Then
            occuredInvestorDto.Should().BeEquivalentTo(expectedInvestorDto);
            mapperMock.Verify(x => x.Map<Investor, InvestorDto>(It.IsAny<Investor>()), Times.Once);
        }

        [Fact]
        public async Task get_async_throws_excpetion_if_repo_does_not_contain_proper_investor()
        {
            //Given
            var userId = Guid.NewGuid();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(userId))
                                  .Returns(Task.FromResult((Investor)null));


            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(investorRepositoryMock.Object, mapperMock.Object);

            //When
            Func<Task<InvestorDto>> act = () => investorService.GetAsync(userId);
            
            //Then
            await Assert.ThrowsAsync<ServiceException>(act);
        }
    }
}