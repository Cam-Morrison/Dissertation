namespace backend.services 
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers; 
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using TimeSeries.Model;

    public class MarketDataService : IMarketDataService
    {
        private readonly IConfiguration Configuration;
        private string marketDataKey;
        private static string tickerList;
        private string finnHubKey;
        private string polygonKey;
        private static string activeStocks;
        private static string homePagePrices;
        private static string mostRecentPriceHistory; //Used for AI prediction
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
           this.polygonKey = Configuration.GetSection("ClientConfiguration").GetValue<string>("polygonKey");
            if(tickerList == null)
            {            
                UpdateMarketData();
            }
        }

        private void UpdateMarketData(){
            var valid = false;
            var i = -1; //Needs to be yesterday
            string yesterday = DateTime.Today.AddDays(i).ToString("yyyy-MM-dd");
                do{
                yesterday = DateTime.Today.AddDays(i).ToString("yyyy-MM-dd");
                DateTime dt = Convert.ToDateTime(yesterday);
                DayOfWeek day = dt.DayOfWeek;
                if(day == DayOfWeek.Sunday || day == DayOfWeek.Saturday) {
                    i--; //Go back a day
                } else {
                    valid = true;
                }
            }while(valid == false);

            string marketDataUrl = $"https://api.polygon.io/v2/aggs/grouped/locale/us/market/stocks/{yesterday}?adjusted=true&include_otc=false&apiKey={polygonKey}";
            string updatedMarketData = CallUrl(marketDataUrl, true);
            if(updatedMarketData != "Issue with API Call")
            {
                Console.WriteLine(updatedMarketData);
                tickerList = updatedMarketData;
                Console.Write(tickerList.ToString());
            } 
            Console.WriteLine(updatedMarketData);
        }

        public List<String> SearchForStock(string userEntry) {
            var results = new List<string>();

            if(tickerList.Contains(userEntry)) {
                JObject json = JObject.Parse(tickerList);
                var list = json["results"]!.Where(t => t["T"].Value<string>().Contains(userEntry))!.ToList();
                Console.WriteLine(list);
                Console.WriteLine(list[0].ToString());
                int i = 0;
                foreach(var stock in list) {
                    i++;
                    results.Add((string)stock["T"]);
                    //Limit results to 5 options
                    if(i == 5) {
                        break;
                    }
                }
                return (List<string>)results;

            } else { 
                 results.Add("No results");
                 return results;
                // return getStockTicker(userEntry);;
            }
        }

        public string GetStockPrice(string ticker)
        {       
            var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{ticker}";
            return CallUrl(url, false);
        }

        public string GetListPrices(string stocks)
        {       
            var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{stocks}";
            return CallUrl(url, false);
        }

        public string GetPriceHistory(string ticker)
        {
            //Currently set to one update per week
            string updateInterval = "1wk";
            var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/hi/history/{ticker}/{updateInterval}?diffandsplits=false";
            mostRecentPriceHistory = CallUrl(url, false);
            return mostRecentPriceHistory;
        }

        public string GetActiveStocks() {
            if(activeStocks == null) {
                var url = "https://yahoo-finance15.p.rapidapi.com/api/yahoo/co/collections/day_gainers";
                activeStocks = CallUrl(url, false);
            } 
            return activeStocks!.ToString();
        }

        public string GetStockDetail(string ticker)
        {
           var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{ticker}/asset-profile";
           return CallUrl(url, false);
        }

        public string GetStockFinancials(string ticker)
        {
           var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/qu/quote/{ticker}/default-key-statistics";
           return CallUrl(url, false);
        }

        // public List<String> getStockTicker(string name) {
        //     var call = CallUrl($"https://finnhub.io/api/v1/search?q={name.ToLower()}&token={finnHubKey}", true);
        //     JObject json = JObject.Parse(call);
        //     var results = new List<string>();
        //     if(json["count"].Value<int>() > 0)
        //     {
        //         for(int i = 0; i < json["count"].Value<int>(); i++) {
        //             results.Add(json["result"][i]["symbol"].ToString());
        //             if(i == 5) {
        //                 break;
        //             }
        //         }
        //     } else {
        //         results.Add("No results");
        //     }
        //     return results;  
        // }

        public bool IsStockValid(string ticker) {
            var call = CallUrl($"https://finnhub.io/api/v1/search?q={ticker}&token={finnHubKey}", true);
            JObject json = JObject.Parse(call);
            if(json["count"].Value<int>() == 0)
            {
                return false;
            } 
            return true;     
        }

        public string GetPricePrediction()
        {
            // var prices = GetPriceHistory("tsla");
            var prices = File.ReadAllText("C:/Users/Cam-M/Documents/Dissertation/backend/Services/MarketDataService/testdata.txt");
            predictionModel pm = new predictionModel();
            return pm.getForecast(prices);;
        }

        private string CallUrl(string inputUrl, bool keyTwo)
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