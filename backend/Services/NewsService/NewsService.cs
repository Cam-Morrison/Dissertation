namespace backend.services 
{
    using System.Net.Http.Headers;
    using Sentiment.Model;

    public class NewsService : INewsService
    {

        private readonly IConfiguration Configuration;
        private static string? TodaysNews;
        private string newsToken;
 
        public NewsService(IConfiguration configuration)
        {
            Configuration = configuration;
            this.newsToken = Configuration.GetSection("ClientConfiguration").GetValue<string>("finnHubKey");
        }

        public string GetSentiment(string articleText) {

            ModelInput sampleData = new ModelInput()
            {
                Text = articleText
            };
            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);
            
            return $"Predicted sentiment of {predictionResult.Prediction}, Predicted Sentiment scores: {String.Join(",", predictionResult.Score)}.";
        }

        public string GetDailyNews() {

            if(TodaysNews == null){
                var url = $"https://finnhub.io/api/v1/news?category=general&token={newsToken}";
                TodaysNews = callUrl(url);
            } 
            return TodaysNews;
        }

        private string callUrl(string inputUrl)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(inputUrl);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            // Blocking call! Program will wait here until a response is received or a timeout occurs.
            HttpResponseMessage response = client.GetAsync(inputUrl).Result;  
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