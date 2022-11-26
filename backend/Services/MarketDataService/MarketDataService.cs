namespace backend.services 
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers; 
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class MarketDataService : IMarketDataService
    {
        private readonly IConfiguration Configuration;
        private string marketDataKey;
        private string finnHubKey;
        private static string marketData;
        private static string homePagePrices;
        private static string mostRecentPriceHistory; //Used for AI prediction
        private string yesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        private string today = DateTime.Today.AddDays(0).ToString("yyyy-MM-dd");

        public MarketDataService(IConfiguration configuration)
        {
            Configuration = configuration;
            initialise();
        }

        public void initialise()
        {
           this.marketDataKey = Configuration.GetSection("ClientConfiguration").GetValue<string>("marketDataKey");
           this.finnHubKey = Configuration.GetSection("ClientConfiguration").GetValue<string>("finnHubKey");
            // if(marketData == null)
            // {            
            //     UpdateMarketData();
            // }
        }

        // public void UpdateMarketData(){
        //     string marketDataUrl = $"https://api.polygon.io/v2/aggs/grouped/locale/us/market/stocks/{yesterday}?adjusted=true&include_otc=false&apiKey=";
        //     string updatedMarketData = callUrl(marketDataUrl);
        //     if(updatedMarketData != "Issue with API Call")
        //     {
        //         marketData = updatedMarketData;
        //     } 
        // }

        public string GetStockPrice(string ticker)
        {       
            var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{ticker}";
            return callUrl(url, false);
        }

        public string GetListPrices(string stocks)
        {       
            var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{stocks}";
            return callUrl(url, false);
        }

        public string GetPriceHistory(string ticker)
        {
            //Currently set to one update per week
            string updateInterval = "1wk";
            var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/hi/history/{ticker}/{updateInterval}?diffandsplits=false";
            mostRecentPriceHistory = callUrl(url, false);
            return mostRecentPriceHistory;
        }

        public string GetActiveStocks() {
            var url = "https://yahoo-finance15.p.rapidapi.com/api/yahoo/co/collections/day_gainers";
            return callUrl(url, false);
        }

        public string GetStockDetail(string ticker)
        {
           var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{ticker}/asset-profile";
           return callUrl(url, false);
        }

        public string GetStockFinancials(string ticker)
        {
           var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{ticker}/default-key-statistics";
           return callUrl(url, false);
        }

        public string findStockTickerByName(string name)
        {
            var url = $"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={finnHubKey}";
            return callUrl(url, true);
        }

        public bool isStockValid(string ticker) {
            var call = callUrl($"https://finnhub.io/api/v1/search?q={ticker}&token={finnHubKey}", true);
            JObject json = JObject.Parse(call);
            if(json["count"].Value<int>() == 0)
            {
                return false;
            } 
            return true;     
        }

        public string getPricePrediction()
        {
            return "£430";
        }

        private string callUrl(string inputUrl, bool keyTwo)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(inputUrl);
            if(!keyTwo) {
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", this.marketDataKey);
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "yahoo-finance15.p.rapidapi.com");
            } 
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            // List data response.
            HttpResponseMessage response = client.GetAsync(inputUrl).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var responseString = response.Content.ReadAsStringAsync().Result;
                client.Dispose();
                return responseString;
            }
            else
            {
                client.Dispose();
                return "Issue with API Call";
            }
        }
    }
}