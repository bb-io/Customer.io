using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Request.Content;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Customer.io.DataSourceHandlers;

public class ContentDataHandler(InvocationContext invocationContext, [ActionParameter] ContentTypeRequest contentRequest)
    : CustomerIoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(contentRequest.ContentType))
        {
            throw new Exception("Please, provide 'Content type' input first");
        }

        var dataHandler = GetDataHandlerForContentType(contentRequest.ContentType);
        return dataHandler.GetDataAsync(context, cancellationToken);
    }

    private IAsyncDataSourceItemHandler GetDataHandlerForContentType(string contentType)
    {
        return contentType switch
        {
            "newsletter" => new NewsletterDataHandler(InvocationContext),
            "broadcast_message" => new BroadcastDataHandler(InvocationContext),
            "campaign_message" => new CampaignDataHandler(InvocationContext),
            "transactional_message" => new TransactionalMessageDataHandler(InvocationContext),
            _ => throw new Exception($"Couldn't find data handler for given content type: {contentType}")
        };
    }
}