namespace backend.services 
{
    public interface IFeatureFlagService
    {
        Task<bool> GetFeatureFlagAsync(string featureFlag);
    }
}