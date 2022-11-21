namespace backend.services 
{
    public interface INewsService
    {
        string GetSentiment(string articleText);
        string GetDailyNews();
    }
}