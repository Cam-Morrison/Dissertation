using Newtonsoft.Json;

namespace TimeSeries.Model
{
    public class predictionInput
    {
       [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("date_utc")]
        public int DateUtc { get; set; }

        [JsonProperty("open")]
        public double Open { get; set; }

        [JsonProperty("high")]
        public double High { get; set; }

        [JsonProperty("low")]
        public double Low { get; set; }

        [JsonProperty("close")]
        public double Close { get; set; }

        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("adjclose")]
        public double Adjclose { get; set; }
    }
}