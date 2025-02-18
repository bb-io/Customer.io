using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers
{
    public class CampaignDataHandler(InvocationContext invocationContext)
        : CustomerIoInvocable(invocationContext), IAsyncDataSourceItemHandler
    {
        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var request = new CustomerIoRequest("v1/campaigns", Method.Get, Creds);
            var response = await Client.ExecuteWithErrorHandling<ListCampaignsResponse>(request);

            return response.Campaigns
                .Where(x => context.SearchString == null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Select(x => new DataSourceItem(x.Id.ToString(), x.Name));
        }
    }
}
