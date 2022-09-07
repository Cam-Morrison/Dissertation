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
                updateMarketData();
            }
        }

        public void updateMarketData(){
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
                return "Ticker cannot be found";
            }
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
                return "Issue with API Call";
            }
        }
    }
}