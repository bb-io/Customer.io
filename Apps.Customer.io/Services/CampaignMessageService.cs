using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Models.Response.TransactionalMessage;
using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Customer.io.Services;

public class CampaignMessageService(InvocationContext invocationContext)
    : CustomerIoInvocable(invocationContext), IContentService
{
    public async Task<Stream> DownloadContentAsync(string contentId, string? language, string? actionId)
    {        
        if (string.IsNullOrEmpty(actionId))
        {
            throw new PluginMisconfigurationException(
                "'Action ID' is null or empty, but it is a required input for the 'Campaign message' content type. " +
                "Please provide an 'Action ID' for this action.");
        }
        
        var endpoint = $"v1/campaigns/{contentId}/actions/{actionId}/language/{language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<CampaignMessageTranslationResponse>(request);
        return CampaignMessageConverter.ToHtmlStream(response);
    }

    public async Task<ContentResponse> UploadContentAsync(Stream htmlStream, string? language, string? actionId)
    {         
        if (string.IsNullOrEmpty(actionId))
        {
            throw new PluginMisconfigurationException(
                "'Action ID' is null or empty, but it is a required input for the 'Campaign message' content type. " +
                "Please provide an 'Action ID' for this action.");
        }
        
        var campaignMessageEntity = CampaignMessageConverter.ToCampaignMessageEntity(htmlStream);
        var endpoint = $"v1/campaigns/{campaignMessageEntity.Id}/actions/{actionId}/language/{language}";
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(new
            {
                name = campaignMessageEntity.Name,
                body = campaignMessageEntity.Body
            }, JsonConfig.Settings);
        
        var response = await Client.ExecuteWithErrorHandling<CampaignMessageTranslationResponse>(request);
        return new()
        {
            ContentId = response.Answer.CampaignId.ToString(),
            Name = response.Answer.Name,
            ContentType = ContentTypes.CampaignMessage,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(response.Answer.Created).UtcDateTime,
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(response.Answer.Updated).UtcDateTime,
        };
    }
}