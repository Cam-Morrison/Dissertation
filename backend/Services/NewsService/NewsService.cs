namespace backend.services 
{
    using Sentiment.Model;

    public class NewsService : INewsService
    {
        public string getSentiment(string articleText) {

            ModelInput sampleData = new ModelInput()
            {
                Text = articleText
            };
            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);
            
            return $"Predicted sentiment of {predictionResult.Prediction}, Predicted Sentiment scores: {String.Join(",", predictionResult.Score)}.";
        }
    }
}