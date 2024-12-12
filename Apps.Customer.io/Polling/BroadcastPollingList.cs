using Apps.Customer.io.Actions.Base;
using Apps.Customer.io.Api;
using Apps.Customer.io.Models.Response.Broadcast;
using Apps.Customer.io.Polling.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Customer.io.Polling
{
    [PollingEventList]
    public class BroadcastPollingList(InvocationContext invocationContext): BasePollingAction(invocationContext, null)
    {

        [PollingEvent("On broadcast created or updated", "Triggered when a new broadcast was created or updated")]
        public async Task<PollingEventResponse<BroadcastMemory, BroadcastEventResponse>> OnBroadcastCreatedOrUpdated(
            PollingEventRequest<BroadcastMemory> request)
        {
            if (request.Memory is null)
            {
                return new()
                {
                    FlyBird = false,
                    Memory = new()
                    {
                        LastPollingTime = DateTime.UtcNow,
                        Triggered = false
                    }
                };
            }

            var requestClient = new CustomerIoRequest("v1/broadcasts", Method.Get,Creds);
            var response = await Client.ExecuteWithErrorHandling<BroadcastEventResponse>(requestClient);

            if (response == null || response.Broadcasts == null || !response.Broadcasts.Any())
            {
                return new PollingEventResponse<BroadcastMemory, BroadcastEventResponse>
                {
                    FlyBird = false,
                    Memory = new BroadcastMemory
                    {
                        LastPollingTime = DateTime.UtcNow,
                        Triggered = false
                    }
                };
            }

            var lastPollingTime = request.Memory.LastPollingTime ?? DateTime.MinValue;

            var newBroadcasts = response.Broadcasts
                .Where(b => DateTimeOffset.FromUnixTimeSeconds(b.Created) > lastPollingTime)
                .OrderByDescending(b => b.Created)
                .ToList();

            var updatedBroadcasts = response.Broadcasts
                .Where(b => DateTimeOffset.FromUnixTimeSeconds(b.Updated) > lastPollingTime && b.Created != b.Updated)
                .ToList();


            if (newBroadcasts.Any() || updatedBroadcasts.Any())
            {
                var latestEventTime = newBroadcasts
                    .Concat(updatedBroadcasts)
                    .Max(b => DateTimeOffset.FromUnixTimeSeconds(b.Updated).UtcDateTime);

                return new PollingEventResponse<BroadcastMemory, BroadcastEventResponse>
                {
                    FlyBird = true,
                    Memory = new BroadcastMemory
                    {
                        LastPollingTime = latestEventTime,
                        Triggered = true
                    },
                    Result = new BroadcastEventResponse
                    {
                        Broadcasts = newBroadcasts.Concat(updatedBroadcasts).ToList()
                    }
                };
            }

            return new PollingEventResponse<BroadcastMemory, BroadcastEventResponse>
            {
                FlyBird = false,
                Memory = new BroadcastMemory
                {
                    LastPollingTime = DateTime.UtcNow,
                    Triggered = false
                }
            };
        }
    }
}
