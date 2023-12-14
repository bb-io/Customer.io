using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Request.TransactionalMessage;
using Apps.Customer.io.Models.Response.TransactionalMessage;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Customer.io.Actions;

[ActionList]
public class TransactionalMessagesActions : CustomerIoInvocable
{
    public TransactionalMessagesActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Get a translation of a transactional message", Description = "Get information about a translation of an individual transactional message")]
    public Task<ListMessageTranslationResponse> ListTransactionalMessageTranslation(
        [ActionParameter] TransactionalMessageTranslationRequest input)
    {
        var endpoint = $"v1/transactional/{input.TransactionalMessageId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        return Client.ExecuteWithErrorHandling<ListMessageTranslationResponse>(request);
    }
    
    [Action("Update a translation of a transactional message", Description = "Update the body and other data of a specific language variant for a transactional message")]
    public Task<ListMessageTranslationResponse> UpdateTransactionalMessageTranslation(
        [ActionParameter] TransactionalMessageTranslationRequest input,
        [ActionParameter] UpdateMessageTranslationRequest payload)
    {
        var endpoint = $"v1/transactional/{input.TransactionalMessageId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling<ListMessageTranslationResponse>(request);
    }
}