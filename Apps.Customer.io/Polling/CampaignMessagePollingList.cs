using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Polling.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Customer.io.Polling;

[PollingEventList("Campaigns")]
public class CampaignMessagePollingList(InvocationContext invocationContext) : CustomerIoInvocable(invocationContext)
{
    [PollingEvent("On campaign messages created or updated",
        "Triggered when a campaign messages is created or updated")]
    public async Task<PollingEventResponse<CampaignMemory, CampaignMessageResponse>> OnCampaignMessageCreatedOrUpdated(
        PollingEventRequest<CampaignMemory> request, 
        [PollingEventParameter] CampaignIdentifier input)
    {
        if (request.Memory is null)
        {
            return new PollingEventResponse<CampaignMemory, CampaignMessageResponse>
            {
                FlyBird = false,
                Memory = new CampaignMemory
                {
                    LastPollingTime = DateTime.UtcNow,
                    Triggered = false
                }
            };
        }

        var lastPollingTime = request.Memory.LastPollingTime ?? DateTime.MinValue;

        var campaignId = input.CampaignId;
        if (campaignId == null)
        {
            throw new InvalidOperationException("Campaign ID must be provided.");
        }

        var requestClient = new CustomerIoRequest($"v1/api/campaigns/{campaignId}/actions", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<CampaignMessageResponse>(requestClient);

        if (response == null || response.Actions == null || !response.Actions.Any())
        {
            return new PollingEventResponse<CampaignMemory, CampaignMessageResponse>
            {
                FlyBird = false,
                Memory = new CampaignMemory
                {
                    LastPollingTime = DateTime.UtcNow,
                    Triggered = false
                }
            };
        }

        var newMessages = response.Actions
            .Where(a => a.Type == "email" && DateTimeOffset.FromUnixTimeSeconds(a.Created) > lastPollingTime)
            .ToList();

        var updatedMessages = response.Actions
            .Where(a => a.Type == "email" && DateTimeOffset.FromUnixTimeSeconds(a.Updated) > lastPollingTime &&
                        a.Created != a.Updated)
            .ToList();

        if (newMessages.Any() || updatedMessages.Any())
        {
            var latestEventTime = response.Actions
                .Max(a => DateTimeOffset.FromUnixTimeSeconds(a.Updated).UtcDateTime);

            return new PollingEventResponse<CampaignMemory, CampaignMessageResponse>
            {
                FlyBird = true,
                Memory = new CampaignMemory
                {
                    LastPollingTime = latestEventTime,
                    Triggered = true
                },
                Result = new CampaignMessageResponse
                {
                    Actions = newMessages.Concat(updatedMessages).ToList()
                }
            };
        }

        return new PollingEventResponse<CampaignMemory, CampaignMessageResponse>
        {
            FlyBird = false,
            Memory = new CampaignMemory
            {
                LastPollingTime = DateTime.UtcNow,
                Triggered = false
            }
        };
    }
}