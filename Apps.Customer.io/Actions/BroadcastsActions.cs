using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request;
using Apps.Customer.io.Models.Request.Broadcast;
using Apps.Customer.io.Models.Response.Broadcast;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Customer.io.Actions;

[ActionList]
public class BroadcastsActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : CustomerIoInvocable(invocationContext)
{
    [Action("Get broadcast message",
        Description = "Get information about a translation of message in a broadcast")]
    public async Task<BroadcastActionEntity> GetBroadcastTranslation([ActionParameter] BroadcastActionRequest input)
    {
        var endpoint = $"v1/broadcasts/{input.BroadcastId}/actions/{input.ActionId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<BroadcastTranslationResponse>(request);
        return response.Action;
    }
    
    [Action("Get broadcast message as HTML", Description = "Get broadcast message as HTML")]
    public async Task<FileResponse> GetBroadcastMessageAsHtmlAsync([ActionParameter] BroadcastActionRequest input)
    {
        var endpoint = $"v1/broadcasts/{input.BroadcastId}/actions/{input.ActionId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<BroadcastTranslationResponse>(request);
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        
        await writer.WriteAsync(response.Action.Body);
        await writer.FlushAsync();
        stream.Position = 0;

        var file = await fileManagementClient.UploadAsync(stream, "text/html", $"{response.Action.Id}.html");
        return new FileResponse
        {
            File = file
        };
    }
    
    [Action("Update broadcast message",
        Description = "Update a translation of a specific broadcast action")]
    public async Task<BroadcastActionEntity> UpdateBroadcastTranslation([ActionParameter] BroadcastActionRequest input,
        [ActionParameter] UpdateBroadcastTranslationRequest payload)
    {
        var endpoint = $"v1/broadcasts/{input.BroadcastId}/actions/{input.ActionId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<BroadcastTranslationResponse>(request);
        return response.Action;
    }
    
    [Action("Update broadcast message from HTML",
        Description = "Update a translation of a specific broadcast action from HTML file")]
    public async Task<BroadcastActionEntity> UpdateBroadcastActionFromHtmlAsync([ActionParameter] BroadcastActionRequest input,
        [ActionParameter] FileRequest updateRequest)
    {
        var fileStream = await fileManagementClient.DownloadAsync(updateRequest.File);
        var bytes = await fileStream.GetByteData();
        var body = System.Text.Encoding.Default.GetString(bytes);
        return await UpdateBroadcastTranslation(input, new()
        {
            Body = body
        });
    }
}