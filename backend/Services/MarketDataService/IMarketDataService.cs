namespace backend.services 
{
    public interface IMarketDataService
    {
        List<String> SearchForStock(string userEntry);
        string GetStockPrice(string ticker);
        string GetStockDetail(string ticker);
        string GetDetailsPageContent(string ticker);
        string GetActiveStocks(Boolean updating);
        string GetPricePrediction();
    }
}