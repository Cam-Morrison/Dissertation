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
            tenantId = "REPLACE";
            clientId = "REPLACE";
            clientSecret = "REPLACE";     
        }

        public string GetSecret(string SecretName) {
            var credentials = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var client = new SecretClient(vaultUri: new Uri("REPLACE"), credentials);
            return client.GetSecret(SecretName).Value.Value;
        }
    }
}
