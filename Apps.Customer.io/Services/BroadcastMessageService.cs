using System.Net.Mime;
using System.Text;
using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request.Broadcast;
using Apps.Customer.io.Models.Response;
using Apps.Customer.io.Models.Response.Broadcast;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Utils;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Customer.io.Services;

public class BroadcastMessageService(InvocationContext invocationContext)
    : CustomerIoInvocable(invocationContext), IContentService
{
    public async Task<Stream> DownloadContentAsync(string contentId, string? language, string? actionId, string? fileFormat)
    {
        if (string.IsNullOrEmpty(actionId))
        {
            throw new PluginMisconfigurationException(
                "'Action ID' is null or empty, but it is a required input for the 'Broadcast message' content type. " +
                "Please provide an 'Action ID' for this action.");
        }

        var endpoint = $"v1/broadcasts/{contentId}/actions/{actionId}/language/{language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<BroadcastTranslationResponse>(request);

        if (fileFormat == MediaTypeNames.Application.Json)
        {
            var wrappedContent = new JsonResponseWithMetadata
            {
                ContentId = contentId,
                ActionId = actionId,
                ContentType = ContentTypes.BroadcastMessage,
                Name = response.Action.Name,
                Body = response.Action.Body,
            };
            var json = JsonConvert.SerializeObject(wrappedContent, Formatting.Indented);
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        }

        var bodyHtml = response.Action.Body;

        var doc = new HtmlDocument();
        doc.LoadHtml(bodyHtml);

        var headNode = doc.DocumentNode.SelectSingleNode("//head");
        if (headNode == null)
        {
            var htmlNode = doc.DocumentNode.SelectSingleNode("//html");
            headNode = HtmlNode.CreateNode("<head></head>");
            if (htmlNode != null)
            {
                htmlNode.PrependChild(headNode);
            }
            else
            {
                throw new PluginApplicationException("Invalid HTML structure: Missing <html> element.");
            }
        }

        InjectMetaTag(headNode, HtmlConstants.ContentId, contentId);
        InjectMetaTag(headNode, HtmlConstants.ActionId, actionId);
        InjectMetaTag(headNode, HtmlConstants.ContentType, ContentTypes.BroadcastMessage);

        var modifiedHtml = doc.DocumentNode.OuterHtml;
        return new MemoryStream(Encoding.UTF8.GetBytes(modifiedHtml));
    }

    public async Task<ContentResponse> UploadContentAsync(Stream htmlStream, string? language, string? actionId)
    {
        var bytes = await htmlStream.GetByteData();
        var htmlString = Encoding.Default.GetString(bytes);

        if (htmlString.IsJson())
        {
            var content = JsonConvert.DeserializeObject<JsonResponseWithMetadata>(htmlString);
            if (content is null) throw new PluginMisconfigurationException("No Custom.io content found in uploaded file");
            var entity = await UpdateBroadcastTranslation(new BroadcastActionRequest()
            {
                BroadcastId = content.ContentId ?? throw new PluginApplicationException("Missing 'contentId' in the uploaded JSON."),
                ActionId = content.ActionId ?? throw new PluginApplicationException("Missing 'actionId' in the uploaded JSON."),
                Language = language
            }, new()
            {
                Body = content.Body?.ToString()
            });

            return new()
            {
                ContentId = entity.BroadcastId,
                Name = entity.Name,
                ContentType = ContentTypes.BroadcastMessage,
                CreatedAt = entity.Created ?? DateTime.MinValue,
                UpdatedAt = entity.Updated ?? DateTime.MinValue
            };
        }

        var doc = new HtmlDocument();
        doc.LoadHtml(htmlString);

        var contentIdNode = doc.DocumentNode.SelectSingleNode($"//meta[@name='{HtmlConstants.ContentId}']");
        var actionIdNode = doc.DocumentNode.SelectSingleNode($"//meta[@name='{HtmlConstants.ActionId}']");

        var actualContentId = contentIdNode?.GetAttributeValue("content", null) ??
            throw new PluginApplicationException(ExceptionMessages.CouldntFindContentIdInHtml);
        var actualActionId = actionId ?? actionIdNode?.GetAttributeValue("content", null);

        if (string.IsNullOrEmpty(actualActionId))
        {
            throw new PluginApplicationException(ExceptionMessages.CouldntFindActionIdInHtml);
        }

        var broadcastEntity = await UpdateBroadcastTranslation(new BroadcastActionRequest()
        {
            BroadcastId = actualContentId,
            ActionId = actualActionId,
            Language = language
        }, new()
        {
            Body = htmlString
        });

        return new()
        {
            ContentId = broadcastEntity.BroadcastId,
            Name = broadcastEntity.Name,
            ContentType = ContentTypes.BroadcastMessage,
            CreatedAt = broadcastEntity.Created ?? DateTime.MinValue,
            UpdatedAt = broadcastEntity.Updated ?? DateTime.MinValue
        };
    }

    private async Task<BroadcastActionEntity> UpdateBroadcastTranslation(BroadcastActionRequest input,
        UpdateBroadcastTranslationRequest payload)
    {
        var endpoint = $"v1/broadcasts/{input.BroadcastId}/actions/{input.ActionId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<BroadcastTranslationResponse>(request);
        return response.Action;
    }

    private void InjectMetaTag(HtmlNode headNode, string metaName, string metaContent)
    {
        var existingMeta = headNode.SelectSingleNode($"//meta[@name='{metaName}']");
        if (existingMeta == null)
        {
            var metaTag = HtmlNode.CreateNode($"<meta name='{metaName}' content='{System.Net.WebUtility.HtmlEncode(metaContent)}'>");
            headNode.AppendChild(metaTag);
        }
        else
        {
            existingMeta.SetAttributeValue("content", System.Net.WebUtility.HtmlEncode(metaContent));
        }
    }
}