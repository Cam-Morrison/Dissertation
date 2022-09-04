using ConfigCat.Client;
using LogLevel = ConfigCat.Client.LogLevel;

namespace backend.services 
{
    public class FeatureFlagService : IFeatureFlagService
    {
        private readonly IConfiguration Configuration;

        public FeatureFlagService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Task<bool> GetFeatureFlagAsync(string featureFlag)
        {
            var featureFlagSdkKey = Configuration.GetSection("ClientConfiguration").GetValue<string>("FeatureFlagSdkKey");

            var client = new ConfigCatClient(featureFlagSdkKey);
            client.LogLevel = LogLevel.Info;

            return client.GetValueAsync(featureFlag, false);
        }
    }

}
