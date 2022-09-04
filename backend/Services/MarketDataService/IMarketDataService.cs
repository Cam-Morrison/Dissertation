namespace backend.services 
{
    using backend.DTOs;
    public interface IMarketDataService
    {
        string GetStockPrice(string ticker);
        string GetStockDetail(string ticker);
        string GetMarketData();
    }
}