﻿using System.Text;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Response.TransactionalMessage;
using Blackbird.Applications.Sdk.Common.Exceptions;
using HtmlAgilityPack;

namespace Apps.Customer.io.Utils.Converters;

public record CampaignMessageEntity(string Id, string Name, string Body, string Subject, string PreHeader);

public static class CampaignMessageConverter
{
    private const string DataSectionSubject = "subject";
    private const string DataSectionPreHeader = "preheader";
    
    public static Stream ToHtmlStream(CampaignMessageTranslationResponse campaignMessage)
    {
        var htmlDoc = new HtmlDocument();

        if (campaignMessage.Answer.Type == "email")
        {
            htmlDoc.LoadHtml(campaignMessage.Answer.Body);

            var headNode = htmlDoc.DocumentNode.SelectSingleNode("//head");
            if (headNode == null)
            {
                var htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//html");
                headNode = HtmlNode.CreateNode("<head></head>");
                htmlNode?.PrependChild(headNode);
            }

            InjectMetaTag(headNode, HtmlConstants.ContentId, campaignMessage.Answer.CampaignId.ToString());
            InjectMetaTag(headNode, HtmlConstants.ContentType, ContentTypes.CampaignMessage);
            InjectMetaTag(headNode, HtmlConstants.MessageType, campaignMessage.Answer.Type);

            var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
            if (bodyNode == null)
            {
                var htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//html");
                bodyNode = HtmlNode.CreateNode("<body></body>");
                if (htmlNode != null)
                {
                    htmlNode.AppendChild(bodyNode);
                }
                else
                {
                    htmlDoc.DocumentNode.AppendChild(bodyNode);
                }
            }

            if (!string.IsNullOrEmpty(campaignMessage.Answer.PreheaderText))
            {
                var preheaderHtml = $"<div data-section='{DataSectionPreHeader}'>{System.Net.WebUtility.HtmlEncode(campaignMessage.Answer.PreheaderText)}</div>";
                var preheaderNode = HtmlNode.CreateNode(preheaderHtml);
                bodyNode.PrependChild(preheaderNode);
            }

            if (!string.IsNullOrEmpty(campaignMessage.Answer.Subject))
            {
                var subjectHtml = $"<div data-section='{DataSectionSubject}'>{System.Net.WebUtility.HtmlEncode(campaignMessage.Answer.Subject)}</div>";
                var subjectNode = HtmlNode.CreateNode(subjectHtml);
                bodyNode.PrependChild(subjectNode);
            }
        }
        else
        {
            var htmlNode = HtmlNode.CreateNode("<html></html>");
            var headNode = HtmlNode.CreateNode("<head></head>");

            headNode.AppendChild(HtmlNode.CreateNode("<meta charset='UTF-8'>"));
            headNode.AppendChild(
                HtmlNode.CreateNode("<meta name='viewport' content='width=device-width, initial-scale=1.0'>"));

            InjectCommonMetaTags(headNode, campaignMessage.Answer.CampaignId.ToString(), campaignMessage.Answer.Type);

            var titleNode =
                HtmlNode.CreateNode($"<title>{System.Net.WebUtility.HtmlEncode(campaignMessage.Answer.Name)}</title>");
            headNode.AppendChild(titleNode);

            var bodyNode = HtmlNode.CreateNode("<body></body>");
            var bodyContent =
                HtmlNode.CreateNode($"<p>{System.Net.WebUtility.HtmlEncode(campaignMessage.Answer.Body)}</p>");
            bodyNode.AppendChild(bodyContent);

            htmlNode.AppendChild(headNode);
            htmlNode.AppendChild(bodyNode);
            htmlDoc.DocumentNode.AppendChild(htmlNode);
        }

        var htmlString = htmlDoc.DocumentNode.OuterHtml;
        return new MemoryStream(Encoding.UTF8.GetBytes(htmlString));
    }

    public static CampaignMessageEntity ToCampaignMessageEntity(Stream htmlStream)
    {
        using var reader = new StreamReader(htmlStream, Encoding.UTF8);
        var htmlContent = reader.ReadToEnd();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        var contentIdNode = htmlDoc.DocumentNode.SelectSingleNode($"//meta[@name='{HtmlConstants.ContentId}']");
        var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");

        var name = titleNode != null ? System.Net.WebUtility.HtmlDecode(titleNode.InnerText) : string.Empty;
        var contentId = contentIdNode?.GetAttributeValue("content", string.Empty) ??
                        throw new PluginApplicationException("Missing 'blackbird-content-id' in the uploaded HTML.");
        
        var messageTypeNode = htmlDoc.DocumentNode.SelectSingleNode($"//meta[@name='{HtmlConstants.MessageType}']");
        var messageType = messageTypeNode?.GetAttributeValue("content", string.Empty) ??
                        throw new PluginApplicationException("Missing 'message-type' meta tag in the uploaded HTML.");
        
        if (messageType == "email")
        {
            RemoveMetaTag(htmlDoc, HtmlConstants.ContentId);
            RemoveMetaTag(htmlDoc, HtmlConstants.ContentType);
            RemoveMetaTag(htmlDoc, HtmlConstants.MessageType);
            
            string subject = ExtractAndRemoveSection(htmlDoc, DataSectionSubject);
            string preHeader = ExtractAndRemoveSection(htmlDoc, DataSectionPreHeader);

            var fullHtml = htmlDoc.DocumentNode.OuterHtml;
            return new CampaignMessageEntity(contentId, name, fullHtml, subject, preHeader);  
        }
        
        var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
        var body = bodyNode != null ? System.Net.WebUtility.HtmlDecode(bodyNode.InnerText) : string.Empty;
        return new CampaignMessageEntity(contentId, name, body, string.Empty, string.Empty);
    }
    
    private static void InjectCommonMetaTags(HtmlNode headElement, string contentId, string messageType)
    {
        InjectMetaTag(headElement, HtmlConstants.ContentId, contentId);
        InjectMetaTag(headElement, HtmlConstants.ContentType, ContentTypes.CampaignMessage);
        InjectMetaTag(headElement, HtmlConstants.MessageType, messageType);
    }
    
    private static void InjectMetaTag(HtmlNode headNode, string name, string content)
    {
        var existingMeta = headNode.SelectSingleNode($"//meta[@name='{name}']");
        if (existingMeta == null)
        {
            var metaTag = HtmlNode.CreateNode($"<meta name='{name}' content='{System.Net.WebUtility.HtmlEncode(content)}'>");
            headNode.AppendChild(metaTag);
        }
        else
        {
            existingMeta.SetAttributeValue("content", System.Net.WebUtility.HtmlEncode(content));
        }
    }
    
    private static void RemoveMetaTag(HtmlDocument document, string metaName)
    {
        document.DocumentNode.SelectSingleNode($"//meta[@name='{metaName}']")?.Remove();
    }
    
    private static string ExtractAndRemoveSection(HtmlDocument htmlDoc, string dataSectionName)
    {
        var node = htmlDoc.DocumentNode.SelectSingleNode($"//*[@data-section='{dataSectionName}']");
        if (node != null)
        {
            string text = System.Net.WebUtility.HtmlDecode(node.InnerText);
            node.Remove();
            return text;
        }
        return string.Empty;
    }
}