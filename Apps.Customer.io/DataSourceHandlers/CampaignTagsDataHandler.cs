using RestSharp;
using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Response.Campaigns;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Customer.io.DataSourceHandlers;

public class CampaignTagsDataHandler(InvocationContext context) : CustomerIoInvocable(context),
    IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken ct)
    {
        var request = new CustomerIoRequest("v1/campaigns", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<SearchCampaignsResponse>(request);

        var tags = response.Campaigns
            .Where(c => c.Tags != null)
            .SelectMany(c => c.Tags)
            .Distinct()
            .OrderBy(t => t);

        return tags.Select(tag => new DataSourceItem(tag, tag));
    }
}
