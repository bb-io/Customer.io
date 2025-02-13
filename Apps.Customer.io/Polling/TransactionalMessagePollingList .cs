using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Polling.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Customer.io.Polling
{
    [PollingEventList]
    public class TransactionalMessagePollingList(InvocationContext invocationContext)
        : CustomerIoInvocable(invocationContext)
    {
        [PollingEvent("On transactional messages created or updated",
            "Triggered when a transactional messages is created or updated")]
        public async Task<PollingEventResponse<TransactionalMemory, TransactionalMessageResponse>> OnTransactionalMessageCreatedOrUpdated(
                PollingEventRequest<TransactionalMemory> request)
        {
            if (request.Memory is null)
            {
                return new PollingEventResponse<TransactionalMemory, TransactionalMessageResponse>
                {
                    FlyBird = false,
                    Memory = new TransactionalMemory
                    {
                        LastPollingTime = DateTime.UtcNow,
                        Triggered = false
                    }
                };
            }

            var lastPollingTime = request.Memory.LastPollingTime ?? DateTime.MinValue;

            var requestClient = new CustomerIoRequest("v1/transactional", Method.Get,
                InvocationContext.AuthenticationCredentialsProviders);
            var response = await Client.ExecuteWithErrorHandling<TransactionalMessageResponse>(requestClient);

            if (response == null || response.Messages == null || !response.Messages.Any())
            {
                return new PollingEventResponse<TransactionalMemory, TransactionalMessageResponse>
                {
                    FlyBird = false,
                    Memory = new TransactionalMemory
                    {
                        LastPollingTime = DateTime.UtcNow,
                        Triggered = false
                    }
                };
            }

            var newMessages = response.Messages
                .Where(m => DateTimeOffset.FromUnixTimeSeconds(m.CreatedAt) > lastPollingTime)
                .ToList();

            var updatedMessages = response.Messages
                .Where(m => DateTimeOffset.FromUnixTimeSeconds(m.UpdatedAt) > lastPollingTime &&
                            m.CreatedAt != m.UpdatedAt)
                .ToList();

            if (newMessages.Any() || updatedMessages.Any())
            {
                var latestEventTime = response.Messages
                    .Max(m => DateTimeOffset.FromUnixTimeSeconds(m.UpdatedAt).UtcDateTime);

                return new PollingEventResponse<TransactionalMemory, TransactionalMessageResponse>
                {
                    FlyBird = true,
                    Memory = new TransactionalMemory
                    {
                        LastPollingTime = latestEventTime,
                        Triggered = true
                    },
                    Result = new TransactionalMessageResponse
                    {
                        Messages = newMessages.Concat(updatedMessages).ToList()
                    }
                };
            }

            return new PollingEventResponse<TransactionalMemory, TransactionalMessageResponse>
            {
                FlyBird = false,
                Memory = new TransactionalMemory
                {
                    LastPollingTime = DateTime.UtcNow,
                    Triggered = false
                }
            };
        }
    }
}