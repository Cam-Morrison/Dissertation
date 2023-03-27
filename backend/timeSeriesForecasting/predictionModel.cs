using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms.TimeSeries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Microsoft.ML.DataOperationsCatalog;

namespace TimeSeries.Model
{
    public class predictionModel
    {
        public string getForecast(string prices)
        {
            //Clean data and parse to correct format
            JObject json = JObject.Parse(prices);
            List<predictionInput> modelInput = new List<predictionInput>();
            foreach (JProperty x in (JToken)json["items"])
            {
                JToken value = x.Value;
                //Avoid dodgy error
                if(value.Value<float>("close") == 0) {
                    continue;
                }
                predictionInput pi = new predictionInput
                {
                    date = value.Value<DateTime>("date"),
                    price = value.Value<float>("close"),
                };
                modelInput.Add(pi);
            }

            MLContext mlContext = new MLContext(seed: 0);
            IDataView data = mlContext.Data.LoadFromEnumerable<predictionInput>(modelInput);

            //Data splits
            TrainTestData dataSplit = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);
            IDataView trainData = dataSplit.TrainSet;
            IDataView testData = dataSplit.TestSet;
            /*
            Time series forecasting pipeline:
            Windows size otherwise known as length ensures the signal and noise components of SSA are seperated.
            Horizon size is the number of days to predict
            Confidence level is the percentage of future values that will fall within the prediction interval.
            Confidence levels are important as they limit how much an estimate can vary from trend. 
            */
            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(predictionOutput.ForecastedPrice),
                inputColumnName: nameof(predictionInput.price),
                windowSize: 12,
                seriesLength: modelInput.Count,
                trainSize: modelInput.Count,
                horizon: 30,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: nameof(predictionOutput.LowerBoundPrice),
                confidenceUpperBoundColumn: nameof(predictionOutput.UpperBoundPrice));

            var forecaster = forecastingPipeline.Fit(trainData);
            var forecastingEngine = forecaster.CreateTimeSeriesEngine<predictionInput, predictionOutput>(mlContext);

            //Evaluate model performance using test data split
            Evaluate(testData, forecaster, mlContext);

            //Forecast using all the data
            var entireForecaster = forecastingPipeline.Fit(data);
            var forecastEngine = entireForecaster.CreateTimeSeriesEngine<predictionInput, predictionOutput>(mlContext);
            var forecasts = forecastEngine.Predict();

            // Format data and return
            List<float> forecastList = new List<float>();
            foreach (var forecast in forecasts.ForecastedPrice)
            {
                forecastList.Add(forecast);
            }
            var Json = JsonConvert.SerializeObject(forecastList);

            return Json.ToString();
        }


        static void Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
        {
            IDataView predictions = model.Transform(testData);
            IEnumerable<float> actual =
            mlContext.Data.CreateEnumerable<predictionInput>(testData, true)
                .Select(observed => observed.price);
            IEnumerable<float> forecast =
            mlContext.Data.CreateEnumerable<predictionOutput>(predictions, true)
                .Select(prediction => prediction.ForecastedPrice[0]);

            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);
            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error
            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");
        }
    }
}