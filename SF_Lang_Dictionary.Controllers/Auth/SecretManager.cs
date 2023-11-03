using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace SF_Lang_Dictionary.Controllers.Auth;

/// <summary>
/// Initializes an Azure Key Vault client
/// </summary>
public class SecretManager
{
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

    /// <summary>
    /// Gets the Azure Key Vault client
    /// </summary>
    public SecretClient Client { get => client; }

    /// <summary>
    /// Initializes an Azure Key Vault client
    /// </summary>
    public SecretManager()
    {
        // Initialize Azure Key Vault client with the URI and credentials
        client = new(new Uri(
            Environment.GetEnvironmentVariable("VaultUri") ?? throw new("Vault URI not found")),
            new DefaultAzureCredential(new DefaultAzureCredentialOptions() { ExcludeAzurePowerShellCredential = true }),
            options
        );
    }
}