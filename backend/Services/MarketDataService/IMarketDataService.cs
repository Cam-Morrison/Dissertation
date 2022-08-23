namespace backend.services 
{
    using backend.DTOs;
    public interface IMarketDataService
    {
        IEnumerable<StockDTO> GetStockPriceAsync(string ticker);
    }
}