using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Request.Content;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Customer.io.DataSourceHandlers;

public class ActionDataHandler(InvocationContext invocationContext, [ActionParameter] ContentRequest contentRequest)
    : CustomerIoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(contentRequest.ContentType))
        {
            throw new Exception("Please, provide 'Content type' input first");
        }

        if (string.IsNullOrEmpty(contentRequest.ContentId))
        {
            throw new Exception("Please, provide 'Content ID' input first");
        }

        var dataHandler = GetDataHandlerForContentType(contentRequest.ContentType, contentRequest.ContentId);
        return dataHandler.GetDataAsync(context, cancellationToken);
    }

    private IAsyncDataSourceItemHandler GetDataHandlerForContentType(string contentType, string contentId)
    {
        return contentType switch
        {
            "newsletter" => throw new Exception(
                "You don't need to specify 'Action ID' optional input for 'Newsletter' content type"),
            "broadcast_message" => new BroadcastActionDataHandler(InvocationContext, new() { BroadcastId = contentId }),
            "campaign_message" => new CampaignActionDataHandler(InvocationContext, new() { CampaignId = contentId }),
            "transactional_message" => throw new Exception(
                "You don't need to specify 'Action ID' optional input for 'Transactional message' content type"),
            _ => throw new Exception($"Couldn't find data handler for given content type: {contentType}")
        };
    }
}