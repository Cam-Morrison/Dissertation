namespace backend.services 
{
    public interface IAdminService
    {
        public string getTaskHistory(string username);
        public string toggleAccountlock(string username, int userId);
    }
}