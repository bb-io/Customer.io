using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Response.Newsletter;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers;

public class NewsletterDataHandler : CustomerIoInvocable, IAsyncDataSourceHandler
{
    public NewsletterDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = new CustomerIoRequest("v1/newsletters", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<ListNewslettersResponse>(request);

        return response.Newsletters
            .Where(x => context.SearchString is null ||
                        x.DisplayName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Id, x => x.DisplayName);
    }
}