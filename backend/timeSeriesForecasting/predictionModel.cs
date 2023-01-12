using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Microsoft.ML.DataOperationsCatalog;

namespace TimeSeries.Model
{
    public class predictionModel
    {
        public predictionModel(string prices)
        {
            JObject json = JObject.Parse(prices);
            List<predictionInput> modelInput = new List<predictionInput>();
            foreach (JProperty x in (JToken)json["items"])
            {
                JToken value = x.Value;
                // predictionInput pi = new predictionInput
                // {
                //     date_utc = value.Value<int>("date_utc"),
                //     close = value.Value<double>("close"),
                //     volume = value.Value<int>("volume")
                // };

                predictionInput pi = new predictionInput
                {
                    date = value.Value<DateTime>("date"),
                    price = value.Value<float>("close"),
                };
                modelInput.Add(pi);
            }

            MLContext mlContext = new MLContext(seed: 0);

            IDataView newData = mlContext.Data.LoadFromEnumerable<predictionInput>(modelInput);

            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(predictionOutput.ForecastedPrice),
                inputColumnName: nameof(predictionInput.price),
                windowSize: 5,
                seriesLength: 10,
                trainSize: modelInput.Count,
                horizon: 5,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: "LowerBoundPrice",
                confidenceUpperBoundColumn: "UpperBoundPrice");

            var forecaster = forecastingPipeline.Fit(newData);
            var forecastingEngine = forecaster.CreateTimeSeriesEngine<predictionInput, predictionOutput>(mlContext);
            var forecasts = forecastingEngine.Predict();
            foreach(var forecast in forecasts.ForecastedPrice) {
                Console.WriteLine(forecast);
            }
            Console.ReadLine();
            
//ATTEMPT 1
            // TrainTestData dataSplit = mlContext.Data.TrainTestSplit(newData, testFraction: 0.2);

            // IDataView trainData = dataSplit.TrainSet;
            // IDataView testData = dataSplit.TestSet;

            // var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
            //     outputColumnName: "ForecastedPrices",
            //     inputColumnName: "close",
            //     windowSize: 7,
            //     seriesLength: 30,
            //     trainSize: (int)testData.GetRowCount(),
            //     horizon: 7,
            //     confidenceLevel: 0.95f,
            //     confidenceLowerBoundColumn: "LowerBoundPrices",
            //     confidenceUpperBoundColumn: "UpperBoundPrices");

            // SsaForecastingTransformer forecaster = forecastingPipeline.Fit(trainData);
            // Evaluate(testData, forecaster, mlContext);

//ATTEMPT 2
            // var pipeline = mlContext.Forecasting.ForecastBySsa(
            //     nameof(predictionOutput.ForecastedPrices),
            //     nameof(predictionInput.close),
            //     windowSize: 5,
            //     seriesLength: 10,
            //     trainSize: 100,
            //     horizon: 4
            // );

            // var model = pipeline.Fit(newData);
            // var forecastingEngine = model.CreateTimeSeriesEngine<predictionInput, predictionOutput>(mlContext);
            // var forecasts = forecastingEngine.Predict();
            // foreach(var forecast in forecasts.ForecastedPrices) {
            //     Console.WriteLine(forecast);
            // }
            // Console.ReadLine();
        }

        // public void Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
        // {
        //     IDataView predictions = model.Transform(testData);
        //     IEnumerable<double> actual = mlContext.Data.CreateEnumerable<predictionInput>(testData, true).Select(observed => observed.close);

        //     IEnumerable<float> forecast = mlContext.Data.CreateEnumerable<predictionOutput>(predictions, true).Select(prediction => prediction.ForecastedPrices[0]);

        //     var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

        //     var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
        //     var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

        //     Console.WriteLine("Evaluation Metrics");
        //     Console.WriteLine("---------------------");
        //     Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
        //     Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");
        // }
    }
}