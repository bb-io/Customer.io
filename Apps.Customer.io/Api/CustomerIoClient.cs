using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Customer.io.Api;

public class CustomerIoClient(IEnumerable<AuthenticationCredentialsProvider> credentials) : BlackBirdRestClient(new()
{
    BaseUrl = credentials.First(c => c.KeyName == CredsNames.BaseUrl).Value.ToUri()
})
{
    protected override JsonSerializerSettings? JsonSettings => JsonConfig.Settings;

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        if (string.IsNullOrEmpty(response.Content))
        {
            if (response.ErrorMessage != null)
            {
                throw new PluginApplicationException(response.ErrorMessage);
            }
        }
        
        var content = response.Content!;
        var error = JsonConvert.DeserializeObject<ErrorResponse>(content)!;

        return error.Errors is not null 
            ? new PluginApplicationException(string.Join(';', error.Errors.Select(x => $"Status: {x.Status}, Details: {x.Detail}; "))) 
            : new PluginApplicationException(error.Meta?.Error ?? content);
    }
}