using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Customer.io.Api;

public class CustomerIoClient : BlackBirdRestClient
{
    protected override JsonSerializerSettings? JsonSettings => JsonConfig.Settings;

    public CustomerIoClient(AuthenticationCredentialsProvider[] creds) : base(new()
    {
        BaseUrl = creds.First(c => c.KeyName == CredsNames.BaseUrl).Value.ToUri()
    })
    {
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var content = response.Content!;
        var error = JsonConvert.DeserializeObject<ErrorResponse>(content)!;

        if (error.Errors is not null)
            return new(string.Join(';', error.Errors.Select(x => x.Detail)));
        
        return new(error.Meta.Error);
    }
}