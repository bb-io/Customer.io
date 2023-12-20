using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Request.Broadcast;
using Apps.Customer.io.Models.Response.Broadcast;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers;

public class BroadcastActionDataHandler : CustomerIoInvocable, IAsyncDataSourceHandler
{
    private string BroadcastId { get; }

    public BroadcastActionDataHandler(InvocationContext invocationContext,
        [ActionParameter] BroadcastActionRequest request) : base(invocationContext)
    {
        BroadcastId = request.BroadcastId;
    }


    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(BroadcastId))
            throw new("You have to input Broadcast first");

        var request = new CustomerIoRequest($"v1/broadcasts/{BroadcastId}/actions", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<ListBroadcastActionsResponse>(request);

        return response.Actions
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Id, x => x.Name);
    }
}