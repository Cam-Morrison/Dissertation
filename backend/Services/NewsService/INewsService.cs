namespace backend.services 
{
    public interface INewsService
    {
        string getSentiment(string articleText);
    }
}