namespace backend.services 
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers; 
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
        private static JObject recentStockDetailsPageCall; 
        private static string recentTickerCall; 

        public MarketDataService(IConfiguration configuration)
        {
            Configuration = configuration;
            initialise();
        }

        public MarketDataService()
        {
            UpdateMarketData();
            GetActiveStocks(true);
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

        private static string TickerToCompanyMap(string userEntry) 
        {
            var results = new List<string>();
            string csvFile = @"../backend/Services/MarketDataService/companyMapping.csv";
            try {
                string[] rows = System.IO.File.ReadAllLines(@csvFile);

                for(int i = 0; i < rows.Length; i++) {
                    string[] columns = rows[i].Split(',');
                    if(columns[1].ToLower().Contains(userEntry.ToLower())) {
                        return columns[0] + " (" + columns[1] + ") "  + columns[2];
                    }
                }
            } catch (Exception ex) {}
            return "No results";
        }

        private void UpdateMarketData(){
            string updatedMarketData;
            var i = 0; //Day
            do {
                i -= 1; //Needs to be yesterday
                if(i == -3) 
                {
                    tickerList = "";
                    return;
                }
                var valid = false;
                
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
                updatedMarketData = CallUrl(marketDataUrl, true);
            }while(updatedMarketData != "Issue with API Call");
            
            tickerList = updatedMarketData;
        }

        public List<String> SearchForStock(string userEntry) {
            userEntry = userEntry.Trim();
            var results = new List<string>();

            if(tickerList.Contains(userEntry)) {
                JObject json = JObject.Parse(tickerList);
                var list = json["results"]!.Where(t => t["T"].Value<string>().Contains(userEntry))!.ToList();
                int i = 0;
                foreach(var stock in list) {
                    i++;
                    var moreDescriptiveName = TickerToCompanyMap((string)stock["T"]);
                    if(moreDescriptiveName != "No results")
                    {
                        results.Add(moreDescriptiveName);
                    }
                    else 
                    {
                        results.Add((string)stock["T"]);
                    }
                    //Limit results to 5 options
                    if(i == 5) {
                        break;
                    }
                }
                return (List<string>)results;

            } else { 
                results.Add(TickerToCompanyMap(userEntry)); 
                return results;  
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

        public string GetDetailsPageContent(string ticker)
        {
            if(ticker != recentTickerCall || recentStockDetailsPageCall == null) {

                string updateInterval = "1wk";
                var url = $"https://yahoo-finance15.p.rapidapi.com/api/yahoo/hi/history/{ticker}/{updateInterval}?diffandsplits=false";
                var resp = CallUrl(url, false);

                JObject json = JObject.Parse(resp);
                predictionModel pm = new predictionModel();
                var forecast = pm.getForecast(resp);
                json.Add("Prediction", forecast);

                var metrics = GetStockFinancials(ticker);
                json.Add("metrics", metrics);

                var details = GetStockDetail(ticker);
                JObject detailsJson = JObject.Parse(details);
                json.Add("details", detailsJson);

                recentStockDetailsPageCall = json;
                recentTickerCall = ticker;
            }
            return recentStockDetailsPageCall.ToString();
        }

        public string GetActiveStocks(Boolean updating) {
            if(updating == true || activeStocks == null) {
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
            return pm.getForecast(prices);
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