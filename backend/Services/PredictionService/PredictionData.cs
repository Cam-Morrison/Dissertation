using Microsoft.ML.Data;

namespace backend.services.prediction 
{
    public class prediction 
    {
           [ColumnName("Score")]
           public float Price { get; set; }
    }

    public class Result
    {
        public double v { get; set; }
        public double vw { get; set; }
        public double o { get; set; }
        public double c { get; set; }
        public double h { get; set; }
        public double l { get; set; }
        public object t { get; set; }
        public int n { get; set; }
    }

    public class Root
    {
        public string ticker { get; set; }
        public int queryCount { get; set; }
        public int resultsCount { get; set; }
        public bool adjusted { get; set; }
        public List<Result> results { get; set; }
        public string status { get; set; }
        public string request_id { get; set; }
        public int count { get; set; }
    }
}