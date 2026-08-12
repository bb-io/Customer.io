using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        var content = response.Content;

        if (string.IsNullOrWhiteSpace(content))
        {
            return new PluginApplicationException(response.ErrorMessage
                ?? $"Request failed with status code {(int)response.StatusCode} ({response.StatusDescription}).");
        }

        var error = TryParseError(content);
        if (error?.Errors is not null && error.Errors.Any())
        {
            return new PluginApplicationException(
                string.Join("; ", error.Errors.Select(x => $"Status: {x.Status}, Details: {x.Detail}")));
        }

        return new PluginApplicationException(error?.Meta?.Error ?? content);
    }

    private static ErrorResponse? TryParseError(string content)
    {
        try
        {
            return JToken.Parse(content) is JObject errorObject
                ? errorObject.ToObject<ErrorResponse>(JsonSerializer.Create(JsonConfig.Settings))
                : null;
        }
        catch (JsonException)
        {
            return null;
        }
    }
}