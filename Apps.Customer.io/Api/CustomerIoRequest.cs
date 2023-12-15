using Apps.Customer.io.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;

namespace Apps.Customer.io.Api;

public class CustomerIoRequest : BlackBirdRestRequest
{
    public CustomerIoRequest(string resource, Method method, IEnumerable<AuthenticationCredentialsProvider> creds) :
        base(resource, method, creds)
    {
    }

    protected override void AddAuth(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var token = creds.Get(CredsNames.ApiKey).Value;
        this.AddHeader("Authorization", $"Bearer {token}");
    }
}