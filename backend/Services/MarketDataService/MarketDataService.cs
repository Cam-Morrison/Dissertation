namespace backend.services 
{
    using backend.DTOs;
    public class MarketDataService : IMarketDataService
    {
        public IEnumerable<StockDTO> GetStockPriceAsync(string ticker){
            // return _mapper.Map<List<StockDTO>>(jsonData);
            return null;
        }
    }
}