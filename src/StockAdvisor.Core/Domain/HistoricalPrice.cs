using System;
using Newtonsoft.Json;

namespace StockAdvisor.Core.Domain
{
    public class HistoricalPrice
    {
        public DateTime Date { get; set; }
        [JsonProperty("close")]
        decimal Price { get; set; }
    }
}