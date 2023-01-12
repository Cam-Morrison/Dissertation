namespace TimeSeries.Model
{
    public class predictionOutput {
        public float[] ForecastedPrice { get; init; }
        public float[] LowerBoundPrice { get; init; }
        public float[] UpperBoundPrice { get; init; }
    }
}