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
                string marketDataUrl = $"https://api.polygon.io/v2/aggs/grouped/locale/us/market/stocks/{yesterday}?adjusted=true&include_otc=false&apiKey=";
                marketData = callUrl(marketDataUrl);
            }
        }

        public string GetStockPrice(string ticker)
        {        
            // string stockPriceUrl = $"https://api.polygon.io/v1/open-close/{ticker}/{yesterday}?adjusted=true&apiKey=";      
            // return callUrl(stockPriceUrl);
            var data = (JObject)JsonConvert.DeserializeObject(marketData);
            
            // manufacturer with the name 'Acme Co'
            JToken selection = data.SelectToken("$.results[?(@.T == '"+ ticker +"')]");
            return selection.ToString(Formatting.None);
        }

        //change to best performers then add customer search for stock instead?
        public string GetMarketData()
        {
            throw new NullReferenceException("GetMarketData error");
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
                return "GetStockPriceAsync Error";
            }
        }
    }
}