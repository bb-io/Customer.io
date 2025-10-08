using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Polling.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.Customer.io.Polling;

[PollingEventList("Content")]
public class ContentPollingList(InvocationContext invocationContext) : CustomerIoInvocable(invocationContext)
{
    [PollingEvent("On content created or updated",
        Description = "Checks and returns all content that was created or updated within the specified time interval")]
    public async Task<PollingEventResponse<ContentMemory, SearchContentResponse>> OnContentCreatedOrUpdated(
        PollingEventRequest<ContentMemory> request,
        [PollingEventParameter] ContentTypesRequest contentTypesRequest,
        [PollingEventParameter] ContentOptionalRequest contentOptionalRequest)
    {
        if (request.Memory == null)
        {
            return new PollingEventResponse<ContentMemory, SearchContentResponse>
            {
                Result = null,
                Memory = new ContentMemory
                {
                    LastPollingTime = DateTime.UtcNow
                },
                FlyBird = false
            };
        }

        var content = new List<ContentResponse>();

        var lastPollingTime = request.Memory.LastPollingTime ?? DateTime.MinValue;

        if (contentTypesRequest.ContentTypes == null ||
            contentTypesRequest.ContentTypes.Contains(ContentTypes.Newsletter))
        {
            var newsletterPollingList = new NewsletterPollingList(InvocationContext);
            var result = await newsletterPollingList.OnNewsletterCreatedOrUpdated(new()
            {
                Memory = new NewsletterMemory { LastPollingTime = lastPollingTime }
            });

            if (result.FlyBird)
            {
                content.AddRange(result.Result?.Newsletters.Select(x => new ContentResponse
                {
                    ContentId = x.Id,
                    Name = x.Name,
                    ContentType = ContentTypes.Newsletter,
                    CreatedAt = DateTimeOffset.FromUnixTimeSeconds(x.Created).UtcDateTime,
                    UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(x.Updated).UtcDateTime,
                }) ?? []);
            }
        }

        if (contentTypesRequest.ContentTypes == null ||
            contentTypesRequest.ContentTypes.Contains(ContentTypes.BroadcastMessage))
        {
            var broadcastPollingList = new BroadcastPollingList(InvocationContext);
            var result = await broadcastPollingList.OnBroadcastCreatedOrUpdated(new()
            {
                Memory = new BroadcastMemory { LastPollingTime = lastPollingTime }
            });

            if (result.FlyBird)
            {
                content.AddRange(result.Result?.Broadcasts.Select(x => new ContentResponse
                {
                    ContentId = x.Id.ToString(),
                    Name = x.Name,
                    ContentType = ContentTypes.BroadcastMessage,
                    CreatedAt = DateTimeOffset.FromUnixTimeSeconds(x.Created).UtcDateTime,
                    UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(x.Updated).UtcDateTime,
                }) ?? []);
            }
        }

        if (contentTypesRequest.ContentTypes == null ||
            contentTypesRequest.ContentTypes.Contains(ContentTypes.CampaignMessage))
        {
            var campaignPollingList = new CampaignMessagePollingList(InvocationContext);
            var result = await campaignPollingList.OnCampaignMessageCreatedOrUpdated(new()
            {
                Memory = new CampaignMemory { LastPollingTime = lastPollingTime }
            }, new());

            if (result.FlyBird)
            {
                content.AddRange(result.Result?.Actions.Select(x => new ContentResponse
                {
                    ContentId = x.Id,
                    Name = x.Name,
                    ContentType = ContentTypes.CampaignMessage,
                    CreatedAt = DateTimeOffset.FromUnixTimeSeconds(x.Created).UtcDateTime,
                    UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(x.Updated).UtcDateTime,
                }) ?? []);
            }
        }

        if (contentTypesRequest.ContentTypes == null ||
            contentTypesRequest.ContentTypes.Contains(ContentTypes.TransactionalMessage))
        {
            var transactionalPollingList = new TransactionalMessagePollingList(InvocationContext);
            var result = await transactionalPollingList.OnTransactionalMessageCreatedOrUpdated(new()
            {
                Memory = new TransactionalMemory { LastPollingTime = lastPollingTime }
            });

            if (result.FlyBird)
            {
                content.AddRange(result.Result?.Messages.Select(x => new ContentResponse
                {
                    ContentId = x.Id.ToString(),
                    Name = x.Name,
                    ContentType = ContentTypes.TransactionalMessage,
                    CreatedAt = DateTimeOffset.FromUnixTimeSeconds(x.CreatedAt).UtcDateTime,
                    UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(x.UpdatedAt).UtcDateTime,
                }) ?? []);
            }
        }

        if (!string.IsNullOrEmpty(contentOptionalRequest.ContentId))
        {
            content = content.Where(x => x.ContentId == contentOptionalRequest.ContentId).ToList();
        }

        return new PollingEventResponse<ContentMemory, SearchContentResponse>
        {
            FlyBird = content.Any(),
            Result = new SearchContentResponse
            {
                Content = content
            },
            Memory = new ContentMemory
            {
                LastPollingTime = DateTime.UtcNow
            }
        };
    }
}