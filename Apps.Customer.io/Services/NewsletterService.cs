using System.Text;
using System.Text.RegularExpressions;
using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Models.Response.Newsletter;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using HtmlAgilityPack;
using RestSharp;

namespace Apps.Customer.io.Services;

public class NewsletterService(InvocationContext invocationContext)
    : CustomerIoInvocable(invocationContext), IContentService
{
    public async Task<Stream> DownloadContentAsync(string contentId, string? language, string? actionId)
    {
        var endpoint = $"v1/newsletters/{contentId}/language/{language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<NewsletterTranslationResponse>(request);

        var entity = response.Content;
        var responseContent = response.Content.Body;
        
        // Extract any content before the <!doctype html> or <html tag
        string? preHtmlContent = null;
        var htmlStartMatch = Regex.Match(responseContent, @"(?i)(<(!doctype\s+html|html))");
        if (htmlStartMatch.Success && htmlStartMatch.Index > 0)
        {
            preHtmlContent = responseContent.Substring(0, htmlStartMatch.Index).Trim();
        }

        var doc = new HtmlDocument();
        doc.LoadHtml(entity.Body);

        var innerBody = doc.DocumentNode.SelectSingleNode("//body");
        var finalBodyContent = innerBody != null ? innerBody.InnerHtml : entity.Body;

        var finalDoc = new HtmlDocument();

        var htmlNode = HtmlNode.CreateNode($"<html lang='{language ?? "en"}'></html>");
        if (!string.IsNullOrEmpty(preHtmlContent))
        {
            // Store pre-HTML content in a custom data attribute
            htmlNode.SetAttributeValue(HtmlConstants.PreHtmlContent, System.Net.WebUtility.HtmlEncode(preHtmlContent));
        }
        
        var headNode = HtmlNode.CreateNode("<head></head>");
        headNode.AppendChild(HtmlNode.CreateNode("<meta charset='UTF-8'>"));
        headNode.AppendChild(HtmlNode.CreateNode("<meta name='viewport' content='width=device-width, initial-scale=1.0'>"));

        headNode.AppendChild(HtmlNode.CreateNode($"<meta name='{HtmlConstants.ContentId}' content='{System.Net.WebUtility.HtmlEncode(contentId)}'>"));
        if(actionId != null)
        {
            headNode.AppendChild(HtmlNode.CreateNode($"<meta name='{HtmlConstants.ActionId}' content='{System.Net.WebUtility.HtmlEncode(actionId)}'>"));
        }
        
        headNode.AppendChild(HtmlNode.CreateNode($"<meta name='{HtmlConstants.ContentType}' content='{ContentTypes.Newsletter}'>"));
        
        headNode.AppendChild(HtmlNode.CreateNode($"<title>{System.Net.WebUtility.HtmlEncode(entity.Subject)}</title>"));

        var bodyNode = HtmlNode.CreateNode("<body></body>");

        var subjectNode = HtmlNode.CreateNode($"<div id='subject'>{System.Net.WebUtility.HtmlEncode(entity.Subject)}</div>");
        var preHeaderNode = HtmlNode.CreateNode($"<div id='preheader'>{System.Net.WebUtility.HtmlEncode(entity.PreheaderText)}</div>");

        bodyNode.AppendChild(subjectNode);
        bodyNode.AppendChild(preHeaderNode);

        var contentNode = HtmlNode.CreateNode($"<div id='content'>{finalBodyContent}</div>");
        bodyNode.AppendChild(contentNode);

        htmlNode.AppendChild(headNode);
        htmlNode.AppendChild(bodyNode);
        finalDoc.DocumentNode.AppendChild(htmlNode);

        var htmlString = finalDoc.DocumentNode.OuterHtml;
        return new MemoryStream(Encoding.UTF8.GetBytes(htmlString));
    }

    public async Task<ContentResponse> UploadContentAsync(Stream htmlStream, string? language, string? actionId)
    {
        var htmlString = await new StreamReader(htmlStream, Encoding.UTF8).ReadToEndAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlString);

        var contentIdNode = doc.DocumentNode.SelectSingleNode("//meta[@name='blackbird-content-id']");
        var contentId = contentIdNode?.GetAttributeValue("content", string.Empty) ?? 
                        throw new PluginApplicationException("Missing 'blackbird-content-id' in the uploaded HTML.");

        var subjectNode = doc.DocumentNode.SelectSingleNode("//div[@id='subject']");
        var preHeaderNode = doc.DocumentNode.SelectSingleNode("//div[@id='preheader']");

        var payload = new UpdateNewsletterTranslationEntity
        {
            Subject = subjectNode?.InnerText.Trim(),
            PreheaderText = preHeaderNode?.InnerText.Trim()
        };
        
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
        
        var endpoint = $"v1/newsletters/{contentId}/language/{language}";
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