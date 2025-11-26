using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request.TransactionalMessage;
using Apps.Customer.io.Models.Response;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Models.Response.TransactionalMessage;
using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Mime;
using System.Text;

namespace Apps.Customer.io.Services;

public class TransactionalMessageService(InvocationContext invocationContext)
    : CustomerIoInvocable(invocationContext), IContentService
{
    public async Task<Stream> DownloadContentAsync(string contentId, string? language, string? actionId, string fileFormat)
    {        
        var endpoint = $"v1/transactional/{contentId}/language/{language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<ListMessageTranslationResponse>(request);

        if (fileFormat == MediaTypeNames.Application.Json)
        {
            var wrappedContent = new JsonResponseWithMetadata
            {
                ContentId = contentId,
                ActionId = actionId,
                ContentType = ContentTypes.TransactionalMessage,
                Name = response.Content.Name,
                Body = response.Content.Body,
            };
            var json = JsonConvert.SerializeObject(wrappedContent, Formatting.Indented);
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        }

        return TransactionalMessageConverter.ToHtmlStream(response.Content, contentId);
    }

    public async Task<ContentResponse> UploadContentAsync(Stream htmlStream, string? language, string? actionId)
    {        
        var campaignMessageEntity = TransactionalMessageConverter.ToTransactionalMessageEntity(htmlStream);
        var response = await UpdateTransactionalMessageTranslation(new()
        {
            TransactionalMessageId = campaignMessageEntity.Id,
            Language = language
        }, new()
        {
            Body = campaignMessageEntity.Body
        });
        
        return new()
        {
            ContentId = campaignMessageEntity.Id,
            Name = response.Name,
            ContentType = ContentTypes.TransactionalMessage,
            CreatedAt = response.Created ?? DateTime.MinValue,
            UpdatedAt = response.Updated ?? DateTime.MinValue
        };
    }

    private async Task<EmailTemplateEntity> UpdateTransactionalMessageTranslation(TransactionalMessageTranslationRequest input, UpdateMessageTranslationRequest payload)
    {
        var endpoint = $"v1/transactional/{input.TransactionalMessageId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<ListMessageTranslationResponse>(request);
        return response.Content;
    }
}