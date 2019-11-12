namespace StockAdvisor.Infrastructure.Services.ValuePredicting
{
    public interface IValuePredictorProvider : IService
    {
        IValuePredictor GetPredictor(string id);
    }
}