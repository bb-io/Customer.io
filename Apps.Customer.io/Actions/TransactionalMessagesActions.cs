using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request;
using Apps.Customer.io.Models.Request.TransactionalMessage;
using Apps.Customer.io.Models.Response.Broadcast;
using Apps.Customer.io.Models.Response.TransactionalMessage;
using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Customer.io.Actions;

[ActionList]
public class TransactionalMessagesActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : CustomerIoInvocable(invocationContext)
{
    [Action("Get translation of a transactional message", Description = "Get information about a translation of an individual transactional message")]
    public async Task<EmailTemplateEntity> GetTransactionalMessageTranslation([ActionParameter] TransactionalMessageTranslationRequest input)
    {
        var endpoint = $"v1/transactional/{input.TransactionalMessageId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<ListMessageTranslationResponse>(request);
        return response.Content;
    }
    
    [Action("Get translation of a transactional message as HTML", Description = "Get information about a translation of an individual transactional message")]
    public async Task<FileResponse> GetTransactionalMessageTranslationAsHtmlAsync([ActionParameter] TransactionalMessageTranslationRequest input)
    {
        var endpoint = $"v1/transactional/{input.TransactionalMessageId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<ListMessageTranslationResponse>(request);
        var stream = TransactionalMessageConverter.ToHtmlStream(response.Content, input.TransactionalMessageId);
        var file = await fileManagementClient.UploadAsync(stream, "text/html", $"{response.Content.Id}.html");
        return new FileResponse
        {
            File = file
        };
    }
    
    [Action("Update translation of a transactional message", Description = "Update the body and other data of a specific language variant for a transactional message")]
    public async Task<EmailTemplateEntity> UpdateTransactionalMessageTranslation([ActionParameter] TransactionalMessageTranslationRequest input,
        [ActionParameter] UpdateMessageTranslationRequest payload)
    {
        var endpoint = $"v1/transactional/{input.TransactionalMessageId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<ListMessageTranslationResponse>(request);
        return response.Content;
    }
    
    [Action("Update translation of a transactional message from HTML", Description = "Update the body and other data of a specific language variant for a transactional message")]
    public async Task<EmailTemplateEntity> UpdateTransactionalMessageTranslationFromHtml([ActionParameter] TransactionalMessageTranslationRequest input,
        [ActionParameter] FileRequest fileRequest)
    {
        var fileStream = await fileManagementClient.DownloadAsync(fileRequest.File);
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        var campaignMessageEntity = TransactionalMessageConverter.ToTransactionalMessageEntity(memoryStream);
        return await UpdateTransactionalMessageTranslation(input, new UpdateMessageTranslationRequest
        {
            Body = campaignMessageEntity.Body
        });
    }
}