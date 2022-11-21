namespace backend.services 
{
    using backend.model;
    public interface IUserService
    {
       public Task<string> Register(Register input);
       public Task<string> Login(Login input);
       public string AddToWatchlist(string username, string ticker);
    }
}