using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Request.Broadcast;
using Apps.Customer.io.Models.Response.Broadcast;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers;

public class BroadcastActionDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] BroadcastActionRequest request)
    : CustomerIoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private string BroadcastId { get; } = request.BroadcastId;

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(BroadcastId))
            throw new("You have to input Broadcast first");

        var request = new CustomerIoRequest($"v1/broadcasts/{BroadcastId}/actions", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<ListBroadcastActionsResponse>(request);

        return response.Actions
            .Where(action => string.IsNullOrWhiteSpace(context.SearchString) ||
                             action.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(action => new DataSourceItem(action.Id, action.Name));
    }
}