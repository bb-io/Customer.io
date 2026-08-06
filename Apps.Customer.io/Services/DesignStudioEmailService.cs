using System.Text;
using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity.Content;
using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Models.Response;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Models.Response.DesignStudio;
using Apps.Customer.io.Services.Models;
using Apps.Customer.io.Utils;
using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Customer.io.Services;

public class DesignStudioEmailService(InvocationContext context) : CustomerIoInvocable(context), IContentService
{
    public async Task<Stream> DownloadContentAsync(ContentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Language))
            throw new PluginMisconfigurationException("Language is required for Design Studio emails");

        string endpoint = $"v1/design_studio/emails/{request.ContentId}/languages/{request.Language}";
        var designStudioRequest = new CustomerIoRequest(endpoint, Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<DesignStudioEmailTranslationResponse>(designStudioRequest);
        var content = response.EmailTranslation.Content;

        return ContentConverter.Serialize(request, new ContentDocument
        {
            Name = content.Subject,
            Subject = content.Subject,
            PreheaderText = content.PreheaderText,
            Body = content.Html
        });
    }

    public async Task<ContentResponse> UploadContentAsync(Stream htmlStream, ContentUploadInput uploadInput)
    {
        if (string.IsNullOrWhiteSpace(uploadInput.Language))
            throw new PluginMisconfigurationException("Language is required for Design Studio emails");

        var fileContent = await new StreamReader(htmlStream, Encoding.UTF8).ReadToEndAsync();
        var (fileContentId, document) = fileContent.IsJson() ? ParseJson(fileContent) : ParseHtml(fileContent);
        
        string contentId = 
            uploadInput.ContentId ?? 
            fileContentId ?? 
            throw new PluginMisconfigurationException(ExceptionMessages.CouldntFindContentIdInHtml);
        
        string endpoint = $"v1/design_studio/emails/{contentId}/languages/{uploadInput.Language}";
        var updateRequest = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(new
            {
                content = new
                {
                    subject = document.Subject,
                    preheader_text = document.PreheaderText,
                    html = document.Body
                }
            });

        await Client.ExecuteWithErrorHandling(updateRequest);

        var getTranslationRequest = new CustomerIoRequest(endpoint, Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<DesignStudioEmailTranslationResponse>(getTranslationRequest);
        var entity = response.EmailTranslation;

        return new ContentResponse
        {
            ContentId = fileContentId,
            Name = entity.Content.Subject,
            ContentType = ContentTypes.DesignStudioEmail,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(entity.Created).UtcDateTime,
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(entity.Updated).UtcDateTime
        };
    }

    private static (string ContentId, ContentDocument Document) ParseHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var contentId = doc.DocumentNode
                            .SelectSingleNode($"//meta[@name='{HtmlConstants.ContentId}']")?
                            .GetAttributeValue("content", null)
            ?? throw new PluginMisconfigurationException(ExceptionMessages.CouldntFindContentIdInHtml);

        var contentNode = doc.DocumentNode.SelectSingleNode("//div[@id='content']");
        if (contentNode is null)
            throw new PluginMisconfigurationException("Could not find content in the file. Make sure it was downloaded from this app");

        var body = html.Substring(contentNode.InnerStartIndex, contentNode.InnerLength).Trim();

        var subjectNode = doc.DocumentNode.SelectSingleNode("//div[@id='subject']");
        var preheaderNode = doc.DocumentNode.SelectSingleNode("//div[@id='preheader']");

        return (contentId, new ContentDocument
        {
            Subject = subjectNode is null ? null : HtmlEntity.DeEntitize(subjectNode.InnerText)?.Trim(),
            PreheaderText = preheaderNode is null ? null : HtmlEntity.DeEntitize(preheaderNode.InnerText)?.Trim(),
            Body = body
        });
    }

    private static (string ContentId, ContentDocument Document) ParseJson(string json)
    {
        var content = JsonConvert.DeserializeObject<JsonResponseWithMetadata>(json)
            ?? throw new PluginMisconfigurationException("No Customer.io content found in the uploaded file.");

        var contentId = content.ContentId ?? throw new PluginMisconfigurationException("Missing 'contentId' in the uploaded JSON.");

        return (contentId, new ContentDocument
        {
            Subject = content.Subject ?? content.Name?.ToString(),
            PreheaderText = content.PreheaderText,
            Body = content.Body?.ToString()
        });
    }
}