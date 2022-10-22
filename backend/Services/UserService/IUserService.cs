namespace backend.services 
{
    using backend.model;
    public interface IUserService
    {
       public Task<bool> Register(Register input);
       public Task<bool> Login(Login input);
    }
}