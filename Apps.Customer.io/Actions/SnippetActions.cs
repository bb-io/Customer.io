using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request.Snippet;
using Apps.Customer.io.Models.Response.Snippet;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Customer.io.Actions;

[ActionList]
public class SnippetActions : CustomerIoInvocable
{
    public SnippetActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("List snippets", Description = "List all snippets in the workspace")]
    public Task<ListSnippetsResponse> ListSnippets()
    {
        var request = new CustomerIoRequest("v1/snippets", Method.Get, Creds);
        return Client.ExecuteWithErrorHandling<ListSnippetsResponse>(request);
    }

    [Action("Update snippet", Description = "Update the name or value of a snippet")]
    public async Task<SnippetEntity> UpdateSnippet([ActionParameter] UpdateSnippetRequest input)
    {
        var request = new CustomerIoRequest("v1/snippets", Method.Put, Creds)
            .WithJsonBody(input, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<SnippetResponse>(request);
        return response.Snippet;
    }
}