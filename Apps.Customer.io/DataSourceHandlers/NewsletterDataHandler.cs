using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Response.Newsletter;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers;

public class NewsletterDataHandler(InvocationContext invocationContext)
    : CustomerIoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new CustomerIoRequest("v1/newsletters", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<ListNewslettersResponse>(request);

        return response.Newsletters
           .Where(newsletter => string.IsNullOrWhiteSpace(context.SearchString) ||
                                newsletter.DisplayName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
           .Select(newsletter => new DataSourceItem(newsletter.Id, newsletter.DisplayName));
    }
}