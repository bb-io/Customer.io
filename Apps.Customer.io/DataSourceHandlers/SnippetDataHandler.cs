using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Response.Snippet;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers;

public class SnippetDataHandler(InvocationContext invocationContext)
    : CustomerIoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new CustomerIoRequest("v1/snippets", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<ListSnippetsResponse>(request);

        return response.Snippets
            .Where(message => string.IsNullOrWhiteSpace(context.SearchString) ||
                              message.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(message => new DataSourceItem(message.Name, message.Name));
    }
}