using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request.Snippet;
using Apps.Customer.io.Models.Response.Broadcast;
using Apps.Customer.io.Models.Response.Snippet;
using Apps.Customer.io.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Customer.io.Actions;

[ActionList("Snippets")]
public class SnippetsActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : CustomerIoInvocable(invocationContext)
{
    [Action("Search snippets", Description = "Returns all snippets in the workspace")]
    public Task<ListSnippetsResponse> ListSnippets()
    {
        var request = new CustomerIoRequest("v1/snippets", Method.Get, Creds);
        return Client.ExecuteWithErrorHandling<ListSnippetsResponse>(request);
    }

    [Action("Get snippet as HTML", Description = "Get a snippet as HTML file")]
    public async Task<FileResponse> GetSnippetAsHtmlAsync([ActionParameter] SnippetRequest snippetRequest)
    {
        var snippets = await ListSnippets();
        var snippet = snippets.Snippets.FirstOrDefault(x => x.Name == snippetRequest.SnippetName)
                      ?? throw new PluginApplicationException(
                          $"Could not find a snippet with provided name: {snippetRequest.SnippetName}");

        var htmlStream = SnippetHtmlConverter.ToHtmlStream(snippet);
        var fileReference = await fileManagementClient.UploadAsync(htmlStream, "text/html", $"{snippet.Name}.html");
        return new()
        {
            File = fileReference
        };
    }

    [Action("Update snippet", Description = "Update the name or value of a snippet")]
    public async Task<SnippetEntity> UpdateSnippet([ActionParameter] UpdateSnippetRequest input)
    {
        var request = new CustomerIoRequest("v1/snippets", Method.Put, Creds)
            .WithJsonBody(input, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<SnippetResponse>(request);
        return response.Snippet;
    }
    
    [Action("Update snippet from HTML", Description = "Update a snippet's value from an uploaded HTML file")]
    public async Task<SnippetEntity> UpdateSnippetFromHtmlAsync([ActionParameter] UpdateSnippetFromHtmlRequest updateSnippetRequest)
    {
        await using var htmlStream = await fileManagementClient.DownloadAsync(updateSnippetRequest.File);
        var memoryStream = new MemoryStream();
        await htmlStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        var snippetEntity = SnippetHtmlConverter.ToSnippetEntity(memoryStream);
        return await UpdateSnippet(new()
        {
            SnippetName = updateSnippetRequest.SnippetName,
            Value = snippetEntity.Value
        });
    }
}