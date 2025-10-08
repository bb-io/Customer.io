using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Polling.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Customer.io.Polling;

[PollingEventList("Newsletters")]
public class NewsletterPollingList(InvocationContext invocationContext) : CustomerIoInvocable(invocationContext)
{
    [PollingEvent("On newsletters created or updated", "Triggered when a newsletters is created or updated")]
    public async Task<PollingEventResponse<NewsletterMemory, NewsletterEventResponse>> OnNewsletterCreatedOrUpdated(
        PollingEventRequest<NewsletterMemory> request)
    {
        if (request.Memory is null)
        {
            return new PollingEventResponse<NewsletterMemory, NewsletterEventResponse>
            {
                FlyBird = false,
                Memory = new NewsletterMemory
                {
                    LastPollingTime = DateTime.UtcNow,
                    Triggered = false
                }
            };
        }

        var lastPollingTime = request.Memory.LastPollingTime ?? DateTime.MinValue;

        var newsletters = new List<Newsletter>();
        string? nextToken = null;
        do
        {
            var requestClient = new CustomerIoRequest(
                $"v1/newsletters?limit=100{(nextToken != null ? $"&start={nextToken}" : "")}",
                Method.Get,
                InvocationContext.AuthenticationCredentialsProviders
            );
            var response = await Client.ExecuteWithErrorHandling<NewsletterEventResponse>(requestClient);

            if (response?.Newsletters != null)
                newsletters.AddRange(response.Newsletters);

            nextToken = response?.Next;
        } while (!string.IsNullOrEmpty(nextToken));

        var newNewsletters = newsletters
            .Where(n => DateTimeOffset.FromUnixTimeSeconds(n.Created) > lastPollingTime)
            .ToList();

        var updatedNewsletters = newsletters
            .Where(n => DateTimeOffset.FromUnixTimeSeconds(n.Updated) > lastPollingTime && n.Created != n.Updated)
            .ToList();

        if (newNewsletters.Any() || updatedNewsletters.Any())
        {
            var latestEventTime = newsletters
                .Max(n => DateTimeOffset.FromUnixTimeSeconds(n.Updated).UtcDateTime);

            return new PollingEventResponse<NewsletterMemory, NewsletterEventResponse>
            {
                FlyBird = true,
                Memory = new NewsletterMemory
                {
                    LastPollingTime = latestEventTime,
                    Triggered = true
                },
                Result = new NewsletterEventResponse
                {
                    Newsletters = newNewsletters.Concat(updatedNewsletters).ToList()
                }
            };
        }

        return new PollingEventResponse<NewsletterMemory, NewsletterEventResponse>
        {
            FlyBird = false,
            Memory = new NewsletterMemory
            {
                LastPollingTime = DateTime.UtcNow,
                Triggered = false
            }
        };
    }
}