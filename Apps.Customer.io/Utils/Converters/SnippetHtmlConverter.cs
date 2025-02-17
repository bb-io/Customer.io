using System.Text;
using Apps.Customer.io.Models.Entity;
using HtmlAgilityPack;

namespace Apps.Customer.io.Utils;

public static class SnippetHtmlConverter
{
    public static Stream ToHtmlStream(SnippetEntity snippetEntity)
    {
        var htmlDoc = new HtmlDocument();
        
        var htmlNode = HtmlNode.CreateNode("<html></html>");
        var headNode = HtmlNode.CreateNode("<head></head>");
        var bodyNode = HtmlNode.CreateNode("<body></body>");

        var titleNode = HtmlNode.CreateNode($"<title>{System.Net.WebUtility.HtmlEncode(snippetEntity.Name)}</title>");
        headNode.AppendChild(titleNode);

        var bodyContent = HtmlNode.CreateNode($"<p>{System.Net.WebUtility.HtmlEncode(snippetEntity.Value)}</p>");
        bodyNode.AppendChild(bodyContent);

        htmlNode.AppendChild(headNode);
        htmlNode.AppendChild(bodyNode);
        htmlDoc.DocumentNode.AppendChild(htmlNode);

        var htmlString = htmlDoc.DocumentNode.OuterHtml;

        byte[] byteArray = Encoding.UTF8.GetBytes(htmlString);
        return new MemoryStream(byteArray);
    }

    public static SnippetEntity ToSnippetEntity(Stream htmlStream)
    {
        using var reader = new StreamReader(htmlStream, Encoding.UTF8);
        var htmlContent = reader.ReadToEnd();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");
        var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

        return new SnippetEntity
        {
            Name = titleNode != null ? System.Net.WebUtility.HtmlDecode(titleNode.InnerText) : string.Empty,
            Value = bodyNode != null ? System.Net.WebUtility.HtmlDecode(bodyNode.InnerText) : string.Empty
        };
    }
}