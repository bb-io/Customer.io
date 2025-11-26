using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common.Exceptions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text;

namespace Apps.Customer.io.Utils.Converters;

public static class TransactionalMessageConverter
{
    public static Stream ToHtmlStream(EmailTemplateEntity campaignMessage, string transactionalMessage)
    {
        var htmlDoc = new HtmlDocument();

        var htmlNode = HtmlNode.CreateNode("<html></html>");
        var headNode = HtmlNode.CreateNode("<head></head>");
        headNode.AppendChild(HtmlNode.CreateNode("<meta charset='UTF-8'>"));
        headNode.AppendChild(
            HtmlNode.CreateNode("<meta name='viewport' content='width=device-width, initial-scale=1.0'>"));

        headNode.AppendChild(HtmlNode.CreateNode(
            $"<meta name='{HtmlConstants.ContentId}' content='{System.Net.WebUtility.HtmlEncode(transactionalMessage)}'>"));
        headNode.AppendChild(
            HtmlNode.CreateNode($"<meta name='{HtmlConstants.ContentType}' content='{ContentTypes.TransactionalMessage}'>"));

        var titleNode =
            HtmlNode.CreateNode($"<title>{System.Net.WebUtility.HtmlEncode(campaignMessage.Name)}</title>");
        headNode.AppendChild(titleNode);

        var bodyNode = HtmlNode.CreateNode("<body></body>");
        var bodyContent =
            HtmlNode.CreateNode($"<p>{System.Net.WebUtility.HtmlEncode(campaignMessage.Body)}</p>");

        bodyNode.AppendChild(bodyContent);

        htmlNode.AppendChild(headNode);
        htmlNode.AppendChild(bodyNode);

        htmlDoc.DocumentNode.AppendChild(htmlNode);

        var htmlString = htmlDoc.DocumentNode.OuterHtml;

        byte[] byteArray = Encoding.UTF8.GetBytes(htmlString);
        return new MemoryStream(byteArray);
    }

    public static EmailTemplateEntity ToTransactionalMessageEntity(Stream htmlStream)
    {
        using var reader = new StreamReader(htmlStream, Encoding.UTF8);
        var htmlContent = reader.ReadToEnd();

        if (htmlContent.IsJson())
        {
            var content = JsonConvert.DeserializeObject<JsonResponseWithMetadata>(htmlContent);
            if (content is null) throw new PluginMisconfigurationException("No Custom.io content found in uploaded file");

            return new EmailTemplateEntity
            {
                Id = content.ContentId ?? throw new PluginApplicationException("Missing 'contentId' in the uploaded JSON."),
                Name = content.Name?.ToString() ?? throw new PluginApplicationException("Missing 'name' in the uploaded JSON."),
                Body = content.ToString() ?? throw new PluginApplicationException("Missing 'body' in the uploaded JSON."),
            };
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        var contentIdNode = htmlDoc.DocumentNode.SelectSingleNode($"//meta[@name='{HtmlConstants.ContentId}']");
        var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");
        var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

        var name = titleNode != null ? System.Net.WebUtility.HtmlDecode(titleNode.InnerText) : string.Empty;
        var body = bodyNode != null ? System.Net.WebUtility.HtmlDecode(bodyNode.InnerText) : string.Empty;
        var contentId = contentIdNode?.GetAttributeValue("content", string.Empty) ??
                        throw new PluginApplicationException("Missing 'blackbird-content-id' in the uploaded HTML.");

        return new EmailTemplateEntity
        {
            Id = contentId,
            Body = body,
            Name = name
        };
    }
}