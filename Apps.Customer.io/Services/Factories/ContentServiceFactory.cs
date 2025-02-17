using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Customer.io.Services.Factories;

public class ContentServiceFactory(InvocationContext invocationContext)
{
    public IContentService GetService(string contentType)
    {
        return contentType switch
        {
            "newsletter" => new NewsletterService(invocationContext),
            "campaign_message" => new CampaignMessageService(invocationContext),
            "broadcast_message" => new BroadcastMessageService(invocationContext),
            "transactional_message" => new TransactionalMessageService(invocationContext),
            _ => throw new Exception($"Couldn't find content type service for given content type: {contentType}")
        };
    }
}