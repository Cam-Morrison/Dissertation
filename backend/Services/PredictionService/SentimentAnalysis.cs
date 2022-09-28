using System;
using DissertationML.Model;

namespace DissertationML.integration
{
    public class SentimentAnalysis
    {
        public String Predict(string input)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Text = input,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);
            return $"Predicted sentiment of {predictionResult.Prediction}, Predicted Sentiment scores: {String.Join(",", predictionResult.Score)}.";
        }
    }
}