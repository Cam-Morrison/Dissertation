using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace backend.services
{
    public class SecretsManagerService 
    {
        readonly string tenantId;
        readonly string clientId;
        readonly string clientSecret;
    
        public SecretsManagerService() {
            tenantId = "cc0332ed-edc4-4fe2-b412-ae14ffdcefba";
            clientId = "2de3bdbd-4813-4edb-ad7b-5ccab9a480ed";
            clientSecret = "YoQ8Q~oYIkHyx2uTlK_g2fSJJ_XJXm~jDYny8aLi";     
        }

        public string GetSecret(string SecretName) {
            var credentials = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var client = new SecretClient(vaultUri: new Uri("https://investmentdashboard.vault.azure.net/"), credentials);
            return client.GetSecret(SecretName).Value.Value;
        }
    }
}