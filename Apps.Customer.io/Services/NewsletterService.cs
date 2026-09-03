using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Response;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Models.Response.Newsletter;
using Apps.Customer.io.Utils;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using Apps.Customer.io.Models.Entity.Content;
using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Services.Models;
using Apps.Customer.io.Utils.Converters;

namespace Apps.Customer.io.Services;

public class NewsletterService(InvocationContext invocationContext)
    : CustomerIoInvocable(invocationContext), IContentService
{
    public async Task<Stream> DownloadContentAsync(ContentRequest downloadInput)
    {
        //if (string.IsNullOrWhiteSpace(downloadInput.Language))
        //    throw new PluginMisconfigurationException("Language is required for Newsletters");

        string endpoint = $"v1/newsletters/{downloadInput.ContentId}/language/{downloadInput.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<NewsletterTranslationResponse>(request);

        var entity = response.Content;

        return ContentConverter.Serialize(downloadInput, new ContentDocument
        {
            Name = entity.Name,
            Subject = entity.Subject,
            PreheaderText = entity.PreheaderText,
            Body = entity.Body
        });
    }

    public async Task<ContentResponse> UploadContentAsync(Stream htmlStream, ContentUploadInput uploadInput)
    {
        var htmlString = await new StreamReader(htmlStream, Encoding.UTF8).ReadToEndAsync();

        var payload = new UpdateNewsletterTranslationEntity();
        var contentId = string.Empty;

        if (htmlString.IsJson())
        {
            var content = JsonConvert.DeserializeObject<JsonResponseWithMetadata>(htmlString);
            if (content is null) throw new PluginMisconfigurationException("No Custom.io content found in uploaded file");
            payload.Subject = content.Subject ?? content.Name?.ToString();
            payload.PreheaderText = content.PreheaderText;
            payload.Body = content.Body?.ToString();
            contentId = content.ContentId;
        } else
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlString);

            var contentIdNode = doc.DocumentNode.SelectSingleNode("//meta[@name='blackbird-content-id']");
            contentId = contentIdNode?.GetAttributeValue("content", string.Empty) ??
                            throw new PluginApplicationException("Missing 'blackbird-content-id' in the uploaded HTML.");

            var subjectNode = doc.DocumentNode.SelectSingleNode("//div[@id='subject']");
            var preHeaderNode = doc.DocumentNode.SelectSingleNode("//div[@id='preheader']");

            payload.Subject = subjectNode is null ? null : HtmlEntity.DeEntitize(subjectNode.InnerText)?.Trim();
            payload.PreheaderText = preHeaderNode is null ? null : HtmlEntity.DeEntitize(preHeaderNode.InnerText)?.Trim();

            subjectNode?.Remove();
            preHeaderNode?.Remove();

            // Extract the pre-HTML content if present
            var htmlNode = doc.DocumentNode.SelectSingleNode("//html");
            string finalHtml = doc.DocumentNode.OuterHtml;
            if (htmlNode != null && htmlNode.Attributes[HtmlConstants.PreHtmlContent] != null)
            {
                var preHtmlContent = System.Net.WebUtility.HtmlDecode(htmlNode.GetAttributeValue(HtmlConstants.PreHtmlContent, string.Empty));
                if (!string.IsNullOrEmpty(preHtmlContent))
                {
                    // Remove the custom attribute from the HTML before sending it back
                    htmlNode.Attributes.Remove(HtmlConstants.PreHtmlContent);
                    finalHtml = doc.DocumentNode.OuterHtml;

                    // Prepend the original pre-HTML content
                    finalHtml = preHtmlContent + Environment.NewLine + Environment.NewLine + finalHtml;
                }
            }

            payload.Body = finalHtml;
        }        
        
        var endpoint = $"v1/newsletters/{contentId}/language/{uploadInput.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);
        
        var response = await Client.ExecuteWithErrorHandling<NewsletterTranslationResponse>(request);
        return new()
        {
            ContentId = response.Content.NewsletterId,
            Name = response.Content.Name,
            ContentType = ContentTypes.Newsletter,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(response.Content.Created).UtcDateTime,
            UpdatedAt = DateTimeOffset.FromUnixTimeSeconds(response.Content.Updated).UtcDateTime
        };
    }
}