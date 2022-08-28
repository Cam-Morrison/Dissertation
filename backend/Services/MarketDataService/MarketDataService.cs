namespace backend.services 
{
    using System.Net.Http;
    using System.Net.Http.Headers; 
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    public class MarketDataService : IMarketDataService
    {
        private readonly IConfiguration Configuration;
        private static string marketData;
        private string marketDataKey;
        private string allStocksUrl = "https://api.polygon.io/v2/aggs/grouped/locale/us/market/stocks/2022-08-17?adjusted=true&include_otc=true&apiKey=";

        public MarketDataService(IConfiguration configuration)
        {
            Configuration = configuration;
            if(marketData == null)
            {
                initialise();
            } 
        }

        public void initialise()
        {
            this.marketDataKey = Configuration.GetSection("ClientConfiguration").GetValue<string>("marketDataKey");
            
            string url = allStocksUrl + marketDataKey;
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
            // Display the status.
            Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream ();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            marketData = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(marketData);
            // Cleanup the streams and the response.
            reader.Close ();
            dataStream.Close ();
            response.Close ();
        }

        public HttpResponseMessage GetStockPriceAsync(string ticker)
        {
            return null;
        }

        //change to best performers then add customer search for stock instead?
        public string GetMarketData()
        {
            if(marketData == null){
                throw new NullReferenceException("GetMarketData error");
            }
            return marketData;
        }

    }
}