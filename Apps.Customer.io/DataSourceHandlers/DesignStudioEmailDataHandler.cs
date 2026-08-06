using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Response.DesignStudio;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers;

public class DesignStudioEmailDataHandler(InvocationContext context) : CustomerIoInvocable(context), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new CustomerIoRequest("v1/design_studio/emails", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<ListDesignStudioEmailsResponse>(request);

        return response.Emails
            .Where(x => string.IsNullOrWhiteSpace(context.SearchString) ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Id, x.Name))
            .ToList();
    }
}