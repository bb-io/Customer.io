using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request.Broadcast;
using Apps.Customer.io.Models.Response.Broadcast;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Customer.io.Actions;

[ActionList]
public class BroadcastsActions : CustomerIoInvocable
{
    public BroadcastsActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Get translation of a broadcast message",
        Description = "Get information about a translation of message in a broadcast")]
    public async Task<BroadcastActionEntity> GetBroadcastTranslation(
        [ActionParameter] BroadcastActionRequest input)
    {
        var endpoint = $"v1/broadcasts/{input.BroadcastId}/actions/{input.ActionId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<BroadcastTranslationResponse>(request);
        return response.Action;
    }

    [Action("Update a translation of a broadcast message",
        Description = "Update a translation of a specific broadcast action")]
    public async Task<BroadcastActionEntity> UpdateBroadcastTranslation(
        [ActionParameter] BroadcastActionRequest input,
        [ActionParameter] UpdateBroadcastTranslationRequest payload)
    {
        var endpoint = $"v1/broadcasts/{input.BroadcastId}/actions/{input.ActionId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<BroadcastTranslationResponse>(request);
        return response.Action;
    }
}