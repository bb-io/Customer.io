using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Response.Broadcast;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers;

public class BroadcastDataHandler(InvocationContext invocationContext)
    : CustomerIoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new CustomerIoRequest("v1/broadcasts", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<ListBroadcastsResponse>(request);

        return response.Broadcasts
            .Where(broadcast => string.IsNullOrWhiteSpace(context.SearchString) ||
                                broadcast.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(broadcast => new DataSourceItem(broadcast.Id, broadcast.Name));
    }
}