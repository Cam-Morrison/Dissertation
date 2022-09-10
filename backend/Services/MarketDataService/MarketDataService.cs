namespace backend.services 
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers; 
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class MarketDataService : IMarketDataService
    {
        private readonly IConfiguration Configuration;
        private string marketDataKey;
        private static string marketData;
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
            if(marketData == null)
            {            
                UpdateMarketData();
            }
        }

        public void UpdateMarketData(){
            string marketDataUrl = $"https://api.polygon.io/v2/aggs/grouped/locale/us/market/stocks/{yesterday}?adjusted=true&include_otc=false&apiKey=";
            string updatedMarketData = callUrl(marketDataUrl);
            if(updatedMarketData != "Issue with API Call")
            {
                marketData = updatedMarketData;
            } 
        }

        public string GetStockPrice(string ticker)
        {       
            try
            {
                var data = (JObject)JsonConvert.DeserializeObject(marketData);
                JToken selection = data.SelectToken("$.results[?(@.T == '"+ ticker +"')]");
                return selection.ToString(Formatting.None);
            }
            catch(Exception ex) 
            {
                return ex.ToString();
            }
        }

        public string GetPriceHistory(string ticker)
        {
            var url = $"https://api.polygon.io/v2/aggs/ticker/{ticker}/range/1/day/2018-01-01/{today}?adjusted=true&sort=asc&apiKey=";
            return callUrl(url);
        }

        public string GetStockDetail(string ticker)
        {
           string stockDetailUrl = $"https://api.polygon.io/v3/reference/tickers/{ticker}?apiKey=";
           return callUrl(stockDetailUrl);
        }

        // public string doesStockExist()

        private string callUrl(string inputUrl)
        {
            string url = inputUrl + marketDataKey;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            // List data response.
            HttpResponseMessage response = client.GetAsync(url).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
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