using Apps.Customer.io.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Customer.io.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        new()
        {
            Name = "Developer API key",
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionProperties = new List<ConnectionProperty>
            {
                new(CredsNames.ApiKey)
                {
                    DisplayName = "API Key",
                    Sensitive = true,
                     Description = "The app API key for authenticating requests to Customer.io."
                },
                new(CredsNames.BaseUrl)
                {
                    DisplayName = "Base URL",
                    Description = "Select the base URL for the Customer.io API. Example: https://api.customer.io or https://api-eu.customer.io",
                    DataItems = [new("https://api.customer.io", "Default API (US)"),
                                 new("https://api-eu.customer.io", "Europe API (EU)")]
                }
            }
        }       
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
         Dictionary<string, string> values)
    {
        var apiKey = values.First(v => v.Key == CredsNames.ApiKey);
        yield return new AuthenticationCredentialsProvider(apiKey.Key, apiKey.Value);

        var baseUrl = values.First(v => v.Key == CredsNames.BaseUrl);
        yield return new AuthenticationCredentialsProvider(baseUrl.Key, baseUrl.Value);
    }
}
