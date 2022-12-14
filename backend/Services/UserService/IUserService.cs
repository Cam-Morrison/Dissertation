namespace backend.services 
{
    using backend.model;
    public interface IUserService
    {
       public Task<string> Register(Register input);
       public Task<string> Login(Login input);
       public string AddToWatchlist(string user, string ticker);
       public string GetWatchlist(string user);
       public string RemoveFromWatchList(string user, string ticker);
       public string UpdateWatchListTitle(string username, string newTitle);
    }
}