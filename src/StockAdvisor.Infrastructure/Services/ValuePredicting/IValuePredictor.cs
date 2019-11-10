using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services.ValuePredicting
{
    public interface IValuePredictor
    {
        CompanyValueStatus PredictValue(CompanyValueStatus historical);
        string GetName();
    }
}