using AutoMapper;
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initailize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<Investor, InvestorDto>();
                cfg.CreateMap<Company, CompanyDto>();
                cfg.CreateMap<CompanyValue, CompanyValueDto>();
                cfg.CreateMap<CompanyValueStatus, CompanyValueStatusDto>();
            })
            .CreateMapper();
    }
}