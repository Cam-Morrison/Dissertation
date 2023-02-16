namespace backend.services
{
    using Sentiment.Model;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using backend.entity;

    public class NewsService : INewsService
    {

        private readonly IConfiguration Configuration;
        private DbContext _context;
        private static string? TodaysNews;
        private string newsToken;

        public NewsService(IConfiguration configuration)
        {
            Configuration = configuration;
            _context = new dbContext(Configuration);
            this.newsToken = Configuration.GetSection("ClientConfiguration").GetValue<string>("finnHubKey");
        }

        public string GetSentiment(string articleText)
        {

            ModelInput sampleData = new ModelInput()
            {
                Text = articleText
            };
            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            return predictionResult.Prediction;
        }

        public string GetDailyNews()
        {

            if (TodaysNews == null)
            {
                var url = $"https://finnhub.io/api/v1/news?category=general&token={newsToken}";
                var call = callUrl(url);

                JArray newsArticles = (JArray)JsonConvert.DeserializeObject(call);
                int i = 1;
                foreach (JObject item in newsArticles)
                {
                    
                    var sentimentScore = GetSentiment(item["summary"].ToString());
                    var textSentiment = "Neutral";
                    if(sentimentScore == "-1") {
                        textSentiment = "Negative";
                    } else {
                        textSentiment = "Positive";
                    }
                    item.Add(new JProperty("sentiment", textSentiment));
                    item.Add(new JProperty("dailyArticleID", i));
                    i++;
                }
                JArray sortedBySentiment = new JArray(newsArticles.OrderByDescending(obj => (string)obj["sentiment"]));

                TodaysNews = sortedBySentiment.ToString();
            }

            return TodaysNews;
        }

        public string getDailyArticleByID(int articleID){
            if(TodaysNews == null) {
                GetDailyNews();
            }
            
            JArray jArr = (JArray)JsonConvert.DeserializeObject(TodaysNews);
            foreach (JObject item in jArr)
            {
                if(item["dailyArticleID"]!.Value<int>() == articleID) {
                    return item.ToString();
                }
            }
            return null;
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