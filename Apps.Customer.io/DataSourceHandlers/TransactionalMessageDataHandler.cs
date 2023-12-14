using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Response.TransactionalMessage;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers;

public class TransactionalMessageDataHandler : CustomerIoInvocable, IAsyncDataSourceHandler
{
    public TransactionalMessageDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = new CustomerIoRequest("v1/transactional", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<ListMessagesResponse>(request);

        return response.Messages
            .Where(x => context.SearchString is null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x.Id, x => x.Name);
    }
}