namespace backend.services 
{
    public interface IMarketDataService
    {
        string GetStockPrice(string ticker);
        string GetStockDetail(string ticker);
        string GetPriceHistory(string ticker);
        string GetActiveStocks();
        string getPricePrediction();
    }
}