using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace backend.services
{
    public class SecretsManagerService : IDisposable
    {
        private readonly IAmazonSecretsManager secretsManager;
        private readonly SecretsManagerCache cache;

        public SecretsManagerService() 
        {
            this.secretsManager = new AmazonSecretsManagerClient();
            this.cache = new SecretsManagerCache(this.secretsManager);
        }

        public void Dispose() 
        {
            this.secretsManager.Dispose();
            this.cache.Dispose();
        }

        public async Task<(string subject, string secret)> GetSuperSecretPassword(string secretId)
        {
            var sec = await this.cache.GetSecretString(secretId);
            var jo = Newtonsoft.Json.Linq.JObject.Parse(sec);
            return (subject: jo["Subject"].ToString(), secret: jo["Secret"].ToObject<string>());
        }
    }
}