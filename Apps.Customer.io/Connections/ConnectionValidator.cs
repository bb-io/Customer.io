using Apps.Customer.io.Api;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.Customer.io.Connections;

public class ConnectionValidator: IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        CancellationToken cancellationToken)
    {
        var credentialsProviders = authenticationCredentialsProviders as AuthenticationCredentialsProvider[] ?? authenticationCredentialsProviders.ToArray();
        
        var client = new CustomerIoClient(credentialsProviders.ToArray());

        try
        {
            var request = new CustomerIoRequest("v1/transactional", Method.Get, credentialsProviders);
            await client.ExecuteWithErrorHandling(request);

            return new()
            {
                IsValid = true
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                IsValid = false,
                Message = ex.Message
            };
        }
    }
}