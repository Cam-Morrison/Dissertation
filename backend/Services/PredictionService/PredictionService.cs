using Microsoft.ML;
using Microsoft.ML.Data;
using System.Text.Json;

namespace backend.services.prediction
{
    class Prediction
    {
        public string timeSeriesForecasting(string dataPoints)
        {
             // 1. Import or create training data
            Root cleanedData = JsonSerializer.Deserialize<Root>(dataPoints);
            List<Result> dailyActivity = cleanedData.results;
            Console.WriteLine(dailyActivity[1].c.ToString());
            
            // foreach(var day in dailyActivity){
            //     Console.WriteLine(day.c.ToString());
            // }
            if(dataPoints == null)
            {
                return "This is exclusively for the stock details page, we use the most recent stocks historical prices to predict forecast";
            }
            List<Result> inMemoryCollection = dailyActivity;

            MLContext mlContext = new MLContext();

            //Load Data
            IDataView data = mlContext.Data.LoadFromTextFile<Result>(inMemoryCollection);
            Console.WriteLine(data);
            // // 2. Specify data preparation and model training pipeline
            // IDataView trainingData = context.Data.LoadFromTextFile
            // var pipeline = context.Transforms.Concatenate("Features", new[] { "t" })
            //    .Append(context.Regression.Trainers.Sdca(labelColumnName: "c", maximumNumberOfIterations: 1000));
            // // 3. Train model
            // var model = pipeline.Fit(trainingData);

            // var timestamp = new Result() { t = 1694976591 };
            // var price = context.Model.CreatePredictionEngine<Result, Prediction>(model).Predict(timestamp);
            // Console.WriteLine(price);
            return cleanedData.results.First().c.ToString();
        } 
    }
}