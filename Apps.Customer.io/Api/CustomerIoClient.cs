using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Customer.io.Api;

public class CustomerIoClient : BlackBirdRestClient
{
    protected override JsonSerializerSettings? JsonSettings => JsonConfig.Settings;

    public CustomerIoClient() : base(new()
    {
        BaseUrl = Urls.Api.ToUri()
    })
    {
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var content = response.Content!;
        var error = JsonConvert.DeserializeObject<ErrorResponse>(content)!;

        return new(error.Meta.Error);
    }
}