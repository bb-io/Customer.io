using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Request.Newsletter;
using Apps.Customer.io.Models.Response.Campaigns;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.DataSourceHandlers;

public class CampaignActionDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] CampaignTranslationRequest campaignTranslationRequest)
    : CustomerIoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(campaignTranslationRequest.CampaignId))
        {
            throw new("Please, provide 'Campaign ID' input first");
        }

        var request = new CustomerIoRequest($"v1/campaigns/{campaignTranslationRequest.CampaignId}/actions", Method.Get,
            Creds);
        var response = await Client.ExecuteWithErrorHandling<ListCampaignActionsResponse>(request);

        return response.Actions
            .Where(action => string.IsNullOrWhiteSpace(context.SearchString) ||
                             action.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(action => new DataSourceItem(action.Id, $"[{action.Language}] {action.Name}"));
    }
}