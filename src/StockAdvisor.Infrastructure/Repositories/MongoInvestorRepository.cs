using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Mongo;

namespace StockAdvisor.Infrastructure.Repositories
{
    public class MongoInvestorRepository : IInvestorRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public MongoInvestorRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Investor> GetAsync(Guid userId)
            => await Investors.AsQueryable().FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<IEnumerable<Investor>> BrowseAsync()
            => await Investors.AsQueryable().ToListAsync();

        public async Task AddAsync(Investor investor)
            => await Investors.InsertOneAsync(investor);

        public async Task UpdateAsync(Investor investor)
            => await Investors.ReplaceOneAsync(x => x.UserId == investor.UserId, investor);

        public async Task RemoveAsync(Guid userId)
            => await Investors.DeleteOneAsync(x => x.UserId == userId);

        private IMongoCollection<Investor> Investors
            => _database.GetCollection<Investor>("Investors");
    }
}