namespace backend.services 
{
    using backend.DTOs;
    public interface IMarketDataService
    {
        HttpResponseMessage GetStockPriceAsync(string ticker);
        string GetMarketData();
    }
}