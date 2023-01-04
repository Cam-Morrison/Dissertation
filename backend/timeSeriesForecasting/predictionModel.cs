using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TimeSeries.Model
{
    public class predictionModel
    {
        public predictionModel(string prices) {
            
            JObject obj = JObject.Parse(prices);
            IList<JToken> results = obj["items"].Children().ToList();
            Console.WriteLine(results[0]);
            IList<predictionInput>? modelInput = null;
            foreach(var token in results) {
                try {
                var item = new predictionInput 
                {
                    Date = (string)token["date"],
                    DateUtc = (int)token["date_utc"],
                    Open = (double)token["open"],
                    High = (double)token["high"],
                    Low = (double)token["low"],
                    Close = (double)token["close"],
                    Volume = (int)token["volume"],
                    Adjclose = (double)token["adjclose"]
                };
                modelInput.Add(item);
                } catch (Exception) {}
            }
            
            Console.WriteLine(modelInput);
        
            // MLContext mlContext = new MLContext(seed: 0);
            // IDataView newData = mlContext.Data.LoadFromEnumerable<predictionInput>(data);
        }
    }
}