using System.Text;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Response.TransactionalMessage;
using Blackbird.Applications.Sdk.Common.Exceptions;
using HtmlAgilityPack;

namespace Apps.Customer.io.Utils.Converters;

public record CampaignMessageEntity(string Id, string Name, string Body);

public static class CampaignMessageConverter
{
    public static Stream ToHtmlStream(CampaignMessageTranslationResponse campaignMessage)
    {
        var htmlDoc = new HtmlDocument();
        
        var htmlNode = HtmlNode.CreateNode("<html></html>");
        var headNode = HtmlNode.CreateNode("<head></head>");
        headNode.AppendChild(HtmlNode.CreateNode("<meta charset='UTF-8'>"));
        headNode.AppendChild(HtmlNode.CreateNode("<meta name='viewport' content='width=device-width, initial-scale=1.0'>"));

        headNode.AppendChild(HtmlNode.CreateNode($"<meta name='{HtmlConstants.ContentId}' content='{System.Net.WebUtility.HtmlEncode(campaignMessage.Answer.CampaignId.ToString())}'>"));
        headNode.AppendChild(HtmlNode.CreateNode($"<meta name='{HtmlConstants.ContentType}' content='{ContentTypes.CampaignMessage}'>"));
        
        var titleNode = HtmlNode.CreateNode($"<title>{System.Net.WebUtility.HtmlEncode(campaignMessage.Answer.Name)}</title>");
        headNode.AppendChild(titleNode);
        
        var bodyNode = HtmlNode.CreateNode("<body></body>");
        var bodyContent = HtmlNode.CreateNode($"<p>{System.Net.WebUtility.HtmlEncode(campaignMessage.Answer.Body)}</p>");
        
        bodyNode.AppendChild(bodyContent);
        
        htmlNode.AppendChild(headNode);
        htmlNode.AppendChild(bodyNode);
        
        htmlDoc.DocumentNode.AppendChild(htmlNode);

        var htmlString = htmlDoc.DocumentNode.OuterHtml;

        byte[] byteArray = Encoding.UTF8.GetBytes(htmlString);
        return new MemoryStream(byteArray);
    }

    public static CampaignMessageEntity ToCampaignMessageEntity(Stream htmlStream)
    {
        using var reader = new StreamReader(htmlStream, Encoding.UTF8);
        var htmlContent = reader.ReadToEnd();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        var contentIdNode = htmlDoc.DocumentNode.SelectSingleNode($"//meta[@name='{HtmlConstants.ContentId}']");
        var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");
        var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

        var name = titleNode != null ? System.Net.WebUtility.HtmlDecode(titleNode.InnerText) : string.Empty;
        var body = bodyNode != null ? System.Net.WebUtility.HtmlDecode(bodyNode.InnerText) : string.Empty;
        var contentId = contentIdNode?.GetAttributeValue("content", string.Empty) ?? 
                        throw new PluginApplicationException("Missing 'blackbird-content-id' in the uploaded HTML.");
        
        return new CampaignMessageEntity(contentId, name, body);
    }
}