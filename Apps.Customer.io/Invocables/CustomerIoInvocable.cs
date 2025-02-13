using Apps.Customer.io.Api;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Customer.io.Invocables;

public class CustomerIoInvocable : BaseInvocable
{
    protected AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    protected CustomerIoClient Client { get; }

    protected CustomerIoInvocable(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new(Creds);
    }
}