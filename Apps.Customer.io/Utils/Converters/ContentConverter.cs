using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Entity.Content;
using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common.Exceptions;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Apps.Customer.io.Utils.Converters;

public static class ContentConverter
{
    public static Stream Serialize(ContentRequest request, ContentDocument document, bool bodyIsJson = false)
    {
        return request.FileFormat switch
        {
            MediaTypeNames.Application.Json => ToJsonStream(request, document, bodyIsJson),
            MediaTypeNames.Text.Html => ToHtmlStream(request, document),
            _ => throw new PluginMisconfigurationException($"This file format is not supported: '{request.FileFormat}'")
        };
    }

    public static Stream ToJsonStream(ContentRequest request, ContentDocument document, bool bodyIsJson = false)
    {
        var body = bodyIsJson && !string.IsNullOrEmpty(document.Body)
            ? JsonConvert.DeserializeObject(document.Body)
            : document.Body;
        
        var wrappedContent = new JsonResponseWithMetadata
        {
            ContentId = request.ContentId,
            ActionId = request.ActionId,
            ContentType = request.ContentType,
            MessageId = document.MessageId,
            Name = document.Name,
            Body = body
        };

        var json = JsonConvert.SerializeObject(wrappedContent, Formatting.Indented);
        return new MemoryStream(Encoding.UTF8.GetBytes(json));
    }

    public static Stream ToHtmlStream(ContentRequest request, ContentDocument document)
    {
        var body = document.Body ?? string.Empty;

        string? preHtmlContent = null;
        var htmlStartMatch = Regex.Match(body, @"(?i)(<(!doctype\s+html|html))");
        if (htmlStartMatch.Success && htmlStartMatch.Index > 0)
            preHtmlContent = body[..htmlStartMatch.Index].Trim();

        var doc = new HtmlDocument();
        doc.LoadHtml(body);

        var innerBody = doc.DocumentNode.SelectSingleNode("//body");
        var finalBodyContent = innerBody?.InnerHtml ?? body;

        var htmlNode = HtmlNode.CreateNode($"<html lang='{request.Language ?? "en"}'></html>");
        if (!string.IsNullOrEmpty(preHtmlContent))
            htmlNode.SetAttributeValue(HtmlConstants.PreHtmlContent, WebUtility.HtmlEncode(preHtmlContent));

        var sourceHead = doc.DocumentNode.SelectSingleNode("//head");
        var headNode = sourceHead?.Clone() ?? HtmlNode.CreateNode("<head></head>");

        if (headNode.SelectSingleNode("meta[@charset] | meta[@http-equiv='Content-Type']") == null)
            headNode.AppendChild(HtmlNode.CreateNode("<meta charset='UTF-8'>"));

        if (headNode.SelectSingleNode("meta[@name='viewport']") == null)
            headNode.AppendChild(HtmlNode.CreateNode("<meta name='viewport' content='width=device-width, initial-scale=1.0'>"));

        headNode.UpsertMeta(HtmlConstants.ContentId, request.ContentId);
        if (request.ActionId != null)
            headNode.UpsertMeta(HtmlConstants.ActionId, request.ActionId);
        headNode.UpsertMeta(HtmlConstants.ContentType, request.ContentType);

        string encodedSubject = WebUtility.HtmlEncode(document.Subject ?? string.Empty);
        var titleNode = headNode.SelectSingleNode("title");
        if (titleNode != null)
            titleNode.InnerHtml = encodedSubject;
        else
            headNode.AppendChild(HtmlNode.CreateNode($"<title>{encodedSubject}</title>"));

        var bodyNode = HtmlNode.CreateNode("<body></body>");
        bodyNode.AppendChild(HtmlNode.CreateNode($"<div id='subject'>{encodedSubject}</div>"));
        bodyNode.AppendChild(HtmlNode.CreateNode($"<div id='preheader'>{WebUtility.HtmlEncode(document.PreheaderText ?? string.Empty)}</div>"));
        bodyNode.AppendChild(HtmlNode.CreateNode($"<div id='content'>{finalBodyContent}</div>"));

        htmlNode.AppendChild(headNode);
        htmlNode.AppendChild(bodyNode);

        var finalDoc = new HtmlDocument();
        finalDoc.DocumentNode.AppendChild(htmlNode);

        return new MemoryStream(Encoding.UTF8.GetBytes(finalDoc.DocumentNode.OuterHtml));
    }
}