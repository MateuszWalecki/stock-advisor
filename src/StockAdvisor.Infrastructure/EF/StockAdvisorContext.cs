using System;
using Microsoft.EntityFrameworkCore;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.EF
{
    public class StockAdvisorContext : DbContext
    {
        private readonly SqlSettings _settings;

        public DbSet<User> Users { get; set; }

        public StockAdvisorContext(DbContextOptions<StockAdvisorContext> options,
            SqlSettings settings) : base(options)
        {
            _settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(_settings.InMemory)
            {
                optionsBuilder.UseInMemoryDatabase("StockAdvisor");

                return;
            }
            optionsBuilder.UseSqlServer(_settings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userBuilder = modelBuilder.Entity<User>();
            userBuilder.HasKey(x => x.Id);
            userBuilder.Property(x => x.Role)
                .HasConversion(
                    x => x.ToString(),
                    x => (UserRole)Enum.Parse(typeof(UserRole), x));
        }
    }
}