using System;
using FluentAssertions;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Exceptions;
using Xunit;

namespace StockAdvisor.UnitTests.Core
{
    public class InvestorTests
    {
        [Fact]
        public void when_company_is_not_in_favourites_it_could_be_added()
        {
            //Given
            var investor = GetValidInvestor();
            string companySymbolToAdd = "ABCD";
            DateTime userUpdateTime = investor.UpdatedAt;

            //When
            investor.AddToFavouriteCompanies(companySymbolToAdd);

            //Then
            Assert.Contains(companySymbolToAdd, investor.FavouriteCompanies);
            Assert.NotEqual(investor.UpdatedAt, userUpdateTime);
        }

        [Fact]
        public void when_company_is_in_favourites_trying_to_add_it_one_more_time_throws_domain_exception()
        {
            //Given
            var investor = GetValidInvestor();
            string companySymbolToAdd = "ABCD";
            investor.AddToFavouriteCompanies(companySymbolToAdd);
            DateTime userUpdateTime = investor.UpdatedAt;

            //When
            Action act = () => investor.AddToFavouriteCompanies(companySymbolToAdd);

            //Then
            Assert.Throws<DomainException>(act);
            Assert.Equal(investor.UpdatedAt, userUpdateTime);
        }

        [Fact]
        public void when_company_is_in_favourites_it_can_be_removed()
        {
            //Given
            var investor = GetValidInvestor();
            string companySymbol = "ABCD";
            investor.AddToFavouriteCompanies(companySymbol);
            DateTime userUpdateTime = investor.UpdatedAt;

            //When
            investor.RemoveFromFavouriteCompanies(companySymbol);

            //Then
            investor.FavouriteCompanies.Should().NotContain(companySymbol);
            Assert.NotEqual(investor.UpdatedAt, userUpdateTime);
        }

        [Fact]
        public void when_company_is_not_in_favourites_trying_to_remove_it_throws_domain_exception()
        {
            //Given
            var investor = GetValidInvestor();
            string companySymbol = "ABCD";
            DateTime userUpdateTime = investor.UpdatedAt;

            //When
            Action act = () => investor.RemoveFromFavouriteCompanies(companySymbol);

            //Then
            Assert.Throws<DomainException>(act);
            Assert.Equal(investor.UpdatedAt, userUpdateTime);
        }
        

        private Investor GetValidInvestor()
            => new Investor(Guid.NewGuid());
    }
}