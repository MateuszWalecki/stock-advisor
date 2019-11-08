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

namespace StockAdvisor.Tests.Unit.Infrastructure
{
    public class InvestorServiceTests
    {
        [Fact]
        public async Task get_async_should_invoke_get_async_on_repository()
        {
        //Given
            var investor = GetDefaultInvestor();

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));

            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            await investorService.GetAsync(investor.UserId);

        //Then
            investorRepositoryMock.Verify(x => x.GetAsync(investor.UserId), Times.Once);
        }

        [Fact]
        public async Task get_async_returns_mapped_investor_if_it_exists()
        {
        //Given
            var investor = GetDefaultInvestor();
            investor.AddToFavouriteCompanies("AAPL");
            investor.AddToFavouriteCompanies("MSFN");

            var expectedInvestorDto = new InvestorDto()
            {
                UserId = investor.UserId,
                UpdatedAt = investor.UpdatedAt,
                FavouriteCompanies = new List<string>(investor.FavouriteCompanies)
            };

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<InvestorDto>(It.IsAny<Investor>()))
                      .Returns(expectedInvestorDto);

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            var occuredInvestorDto = await investorService.GetAsync(investor.UserId);

        //Then
            occuredInvestorDto.Should().BeEquivalentTo(expectedInvestorDto);
            mapperMock.Verify(x => x.Map<InvestorDto>(It.IsAny<Investor>()), Times.Once);
        }

        [Fact]
        public async Task get_async_returns_null_if_repo_does_not_contain_proper_investor()
        {
        //Given
            var userId = Guid.NewGuid();

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(userId))
                                  .Returns(Task.FromResult((Investor)null));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            var investor = await investorService.GetAsync(userId);

        //Then
            investor.Should().BeNull();
        }

        [Fact]
        public async Task register_async_should_invoke_add_async_on_repository()
        {
        //Given
            var user = GetDefaultUser();
            var investor = new Investor(user);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .Returns(Task.FromResult(user));

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult((Investor)null));

            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            await investorService.RegisterAsync(investor.UserId);

        //Then
            investorRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Investor>()), Times.Once);
        }

        [Fact]
        public async Task trying_to_register_investor_related_with_user_id_that_is_in_use_exception_is_thrown()
        {
        //Given
            var user = GetDefaultUser();
            var investor = new Investor(user);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .Returns(Task.FromResult(user));

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            Func<Task> act = () => investorService.RegisterAsync(investor.UserId);

        //Then
            await Assert.ThrowsAsync<InvestorExistsSerExc>(act);
        }

        [Fact]
        public async Task trying_to_register_investor_related_with_fake_user_exception_is_thrown()
        {
        //Given
            var investor = GetDefaultInvestor();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                              .Returns(Task.FromResult((User)null));

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            Func<Task> act = () => investorService.RegisterAsync(investor.UserId);

        //Then
            await Assert.ThrowsAsync<UserNotFoundSerExc>(act);
        }

        [Fact]
        public async Task adding_new_favourite_company_adds_company_to_set()
        {
        //Given
            var company = GetDefaultCompany();
            var investor = GetDefaultInvestor();
            var favouriteComapniesBeforeAdd = new List<string>(investor.FavouriteCompanies);

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.BrowseAsync())
                                 .ReturnsAsync(new List<Company>(){company});

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            await investorService.AddToFavouriteCompaniesAsync(investor.UserId, company.Symbol);
            
        //Then
            favouriteComapniesBeforeAdd.Should().NotContain(company.Symbol);
            investor.FavouriteCompanies.Should().Contain(company.Symbol);
        }

        [Fact]
        public async Task exception_is_thrown_if_related_investor_does_not_exist_while_adding_to_favourite_companies()
        {
        //Given
            var company = GetDefaultCompany();
            var user = GetDefaultUser();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .Returns(Task.FromResult(user));

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(user.Id))
                                  .Returns(Task.FromResult((Investor)null));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            Func<Task> act = () => 
                investorService.AddToFavouriteCompaniesAsync(user.Id, company.Symbol);
            
        //Then
            await Assert.ThrowsAsync<InvestorNotFoundSerExc>(act);
        }

        [Fact]
        public async Task add_to_favourite_companies_invokes_get_async_on_investors_repo()
        {
        //Given
            var company = GetDefaultCompany();
            var investor = GetDefaultInvestor();

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.BrowseAsync())
                                 .ReturnsAsync(new List<Company>(){company});

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            await investorService.AddToFavouriteCompaniesAsync(investor.UserId, company.Symbol);
            
        //Then
            investorRepositoryMock.Verify(x => x.GetAsync(investor.UserId), Times.Once);
        }

        [Fact]
        public async Task add_to_favourite_companies_invokes_update_async_on_investors_repo()
        {
        //Given
            var company = GetDefaultCompany();
            var investor = GetDefaultInvestor();

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.BrowseAsync())
                                 .ReturnsAsync(new List<Company>(){company});

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            await investorService.AddToFavouriteCompaniesAsync(investor.UserId, company.Symbol);
            
        //Then
            investorRepositoryMock.Verify(x => x.UpdateAsync(investor), Times.Once);
        }

        [Fact]
        public async Task add_to_favourite_companies_throws_exception_if_given_symbol_is_present()
        {
        //Given
            var company = GetDefaultCompany();

            var investor = GetDefaultInvestor();
            investor.AddToFavouriteCompanies(company.Symbol);

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.BrowseAsync())
                                 .ReturnsAsync(new List<Company>(){company});

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            Func<Task> act = () => 
                investorService.AddToFavouriteCompaniesAsync(investor.UserId, company.Symbol);

        //Then
            await Assert.ThrowsAsync<CompanySymbolInUseSerExc>(act);
        }

        [Fact]
        public async Task add_to_favourite_companies_throws_exception_if_given_company_symbol_is_invalid()
        {
            // invalid means related company symbol is not in repo. 
        //Given
            var company = GetDefaultCompany();

            var investor = GetDefaultInvestor();

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.BrowseAsync())
                                 .ReturnsAsync(new List<Company>(){company});

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            Func<Task> act = () => 
                investorService.AddToFavouriteCompaniesAsync(investor.UserId, "BAD_COMPANY_SYMBOL");

        //Then
            await Assert.ThrowsAsync<InvalidCompanySymbolSerExc>(act);
            companyRepositoryMock.Verify(x => x.BrowseAsync(), Times.Once);
        }

        [Fact]
        public async Task removing_from_favourite_companies_removes_company_from_investors_set()
        {
        //Given
            string companySymbol = "AAPL";
            var investor = GetDefaultInvestor();

            investor.AddToFavouriteCompanies(companySymbol);
            var favouriteComapniesBeforeAdd = new List<string>(investor.FavouriteCompanies);

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            await investorService.RemoveFromFavouriteCompaniesAsync(investor.UserId, companySymbol);
            
        //Then
            favouriteComapniesBeforeAdd.Should().Contain(companySymbol);
            investor.FavouriteCompanies.Should().NotContain(companySymbol);
        }

        [Fact]
        public async Task exception_is_thrown_if_related_investor_does_not_exist_while_removing_from_favourite_companies()
        {
        //Given
            string companySymbol = "AAPL";
            var user = GetDefaultUser();

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(user.Id))
                                  .Returns(Task.FromResult((Investor)null));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            Func<Task> act = () => investorService.RemoveFromFavouriteCompaniesAsync(user.Id, companySymbol);
            
        //Then
            await Assert.ThrowsAsync<InvestorNotFoundSerExc>(act);
        }

        [Fact]
        public async Task exception_is_thrown_if_related_company_symbol_does_not_exist_while_removing_from_favourite_companies()
        {
        //Given
            string companySymbol = "AAPL";
            var investor = GetDefaultInvestor();

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            Func<Task> act = () 
                => investorService.RemoveFromFavouriteCompaniesAsync(investor.UserId,
                                                                     companySymbol);
            
        //Then
            await Assert.ThrowsAsync<CompanySymbolNotFoundSerExc>(act);
        }

        [Fact]
        public async Task remove_from_favourite_companies_invokes_get_async_on_investors_repo()
        {
        //Given
            string companySymbol = "AAPL";
            var investor = GetDefaultInvestor();
            investor.AddToFavouriteCompanies(companySymbol);

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            await investorService.RemoveFromFavouriteCompaniesAsync(investor.UserId, companySymbol);
            
        //Then
            investorRepositoryMock.Verify(x => x.GetAsync(investor.UserId), Times.Once);
        }

        [Fact]
        public async Task remove_from_favourite_companies_invokes_update_async_on_investors_repo()
        {
        //Given
            string companySymbol = "AAPL";
            var investor = GetDefaultInvestor();
            investor.AddToFavouriteCompanies(companySymbol);

            var userRepositoryMock = new Mock<IUserRepository>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(investor.UserId))
                                  .Returns(Task.FromResult(investor));


            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            await investorService.RemoveFromFavouriteCompaniesAsync(investor.UserId, companySymbol);
            
        //Then
            investorRepositoryMock.Verify(x => x.UpdateAsync(investor), Times.Once);
        }

        [Fact]
        public async Task browse_async_invokes_browseasync_on_repo()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            
            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            var returnedInvestors = await investorService.BrowseAsync();
            
        //Then
            investorRepositoryMock.Verify(x => x.BrowseAsync(), Times.Once);
        }

        [Fact]
        public async Task browse_async_invokes_map_on_mapper()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            
            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            var returnedInvestors = await investorService.BrowseAsync();
            
        //Then
            mapperMock.Verify(x =>
                x.Map<IEnumerable<InvestorDto>>(It.IsAny<IEnumerable<Investor>>()),
                                                Times.Once);
        }
        
        [Fact]
        public async Task browse_async_returns_mapping_result()
        {
        //Given
            IEnumerable<InvestorDto> investors = new List<InvestorDto>();

            var userRepositoryMock = new Mock<IUserRepository>();

            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x =>
                x.Map<IEnumerable<InvestorDto>>(It.IsAny<IEnumerable<Investor>>()))
                      .Returns(investors);

            var investorRepositoryMock = new Mock<IInvestorRepository>();

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            var returnedInvestors = await investorService.BrowseAsync();
            
        //Then
            returnedInvestors.Should().BeSameAs(investors);
        }

        [Fact]
        public async Task get_async_using_mail_returns_null_if_user_does_not_exist()
        {
        //Given
            var user = GetDefaultUser();
            var investor = new Investor(user);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Email))
                              .Returns(Task.FromResult((User)null));

            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(user.Id))
                                  .Returns(Task.FromResult(investor));

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            var returnedInvestorDto = await investorService.GetAsync(user.Email);
            
        //Then
            userRepositoryMock.Verify(x => x.GetAsync(user.Email), Times.Once);
            returnedInvestorDto.Should().BeNull();
        }

        [Fact]
        public async Task get_async_using_mail_returns_null_if_related_investor_does_not_exist()
        {
        //Given
            var user = GetDefaultUser();
            var investor = new Investor(user);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Email))
                              .Returns(Task.FromResult(user));

            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(user.Id))
                                  .Returns(Task.FromResult((Investor)null));

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);

        //When
            var returnedInvestorDto = await investorService.GetAsync(user.Email);
            
        //Then
            userRepositoryMock.Verify(x => x.GetAsync(user.Email), Times.Once);
            investorRepositoryMock.Verify(x => x.GetAsync(user.Id), Times.Once);
            returnedInvestorDto.Should().BeNull();
        }

        [Fact]
        public async Task get_async_using_mail_returns_mapping_result()
        {
        //Given
            var user = GetDefaultUser();
            var investor = new Investor(user);
            var investorDto = new InvestorDto();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Email))
                              .Returns(Task.FromResult(user));

            var investorRepositoryMock = new Mock<IInvestorRepository>();
            investorRepositoryMock.Setup(x => x.GetAsync(user.Id))
                                  .Returns(Task.FromResult(investor));
            
            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<InvestorDto>(investor))
                      .Returns(investorDto);

            var investorService = new InvestorService(userRepositoryMock.Object,
                investorRepositoryMock.Object, companyRepositoryMock.Object,
                mapperMock.Object);
            
        //When
            var returnedInvestorDto = await investorService.GetAsync(user.Email);
            
        //Then
            userRepositoryMock.Verify(x => x.GetAsync(user.Email), Times.Once);
            investorRepositoryMock.Verify(x => x.GetAsync(user.Id), Times.Once);
            mapperMock.Verify(x => x.Map<InvestorDto>(investor), Times.Once);
            returnedInvestorDto.Should().BeSameAs(investorDto);
        }

        [Fact]
        public async Task remove_async_calls_get_investor_from_repo()
        {
        //Given
            var investor = GetDefaultInvestor();

            var userRepoMock = new Mock<IUserRepository>();

            var investorRepoMock = new Mock<IInvestorRepository>();
            investorRepoMock.Setup(x => x.GetAsync(investor.UserId))
                            .ReturnsAsync(investor);

            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepoMock.Object,
                investorRepoMock.Object, companyRepositoryMock.Object, 
                mapperMock.Object);
                
        //When
            await investorService.RemoveAsync(investor.UserId);

        //Then
            investorRepoMock.Verify(x => x.GetAsync(investor.UserId), Times.Once);
        }

        [Fact]
        public async Task remove_async_throws_exception_if_investor_does_not_exist()
        {
        //Given
            var investor = GetDefaultInvestor();

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(x => x.GetAsync(investor.UserId))
                        .ReturnsAsync((User)null);

            var investorRepoMock = new Mock<IInvestorRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepoMock.Object,
                investorRepoMock.Object, companyRepositoryMock.Object, 
                mapperMock.Object);
                
        //When
            Func<Task> act = () => investorService.RemoveAsync(investor.UserId);

        //Then
            await Assert.ThrowsAsync<InvestorNotFoundSerExc>(act);
        }

        [Fact]
        public async Task remove_async_calls_remove_async_on_investor_repo()
        {
        //Given
            var investor = GetDefaultInvestor();

            var userRepoMock = new Mock<IUserRepository>();

            var investorRepoMock = new Mock<IInvestorRepository>();
            investorRepoMock.Setup(x => x.GetAsync(investor.UserId))
                            .ReturnsAsync(investor);

            var companyRepositoryMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();

            var investorService = new InvestorService(userRepoMock.Object,
                investorRepoMock.Object, companyRepositoryMock.Object, 
                mapperMock.Object);
                
        //When
            await investorService.RemoveAsync(investor.UserId);

        //Then
            investorRepoMock.Verify(x => x.RemoveAsync(investor), Times.Once);
        }

        private Investor GetDefaultInvestor()
        {
            var user = GetDefaultUser();
            return new Investor(user);
        }

        private User GetDefaultUser()
            => new User(Guid.NewGuid(), "email@email.com", "Firstname",
                "Surname", "PasswordHash1", "Salt1", UserRole.User);

        private Company GetDefaultCompany()
            => new Company("AAPL", "Apple Inc.", 256.0m);
    }
}