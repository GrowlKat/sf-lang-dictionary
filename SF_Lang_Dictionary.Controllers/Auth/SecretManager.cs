using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace SF_Lang_Dictionary.Controllers.Auth;
public class SecretManager
{
    IConfiguration config = new ConfigurationBuilder().AddUserSecrets("2f781bee-b429-4f52-a84d-3f36352c147c").Build();
    private readonly SecretClient client;
    private readonly SecretClientOptions options = new()
    {
        Retry =
        {
            Delay = TimeSpan.FromSeconds(2),
            MaxDelay = TimeSpan.FromSeconds(16),
            MaxRetries = 5,
            Mode = RetryMode.Exponential
        }
    };

    public SecretClient Client { get => client; }

    public SecretManager()
    {
        client = new(new Uri(
            config.GetValue<string>("VaultUri") ?? throw new("Vault URI not found")),
            new DefaultAzureCredential(new DefaultAzureCredentialOptions() { ExcludeAzurePowerShellCredential = true }),
            options
        );
    }
}