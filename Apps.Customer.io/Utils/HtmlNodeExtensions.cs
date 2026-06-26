using HtmlAgilityPack;

namespace Apps.Customer.io.Utils;

public static class HtmlNodeExtensions
{
    public static void UpsertMeta(this HtmlNode headNode, string name, string value)
    {
        headNode.SelectSingleNode($"meta[@name='{name}']")?.Remove();
        headNode.AppendChild(HtmlNode.CreateNode(
            $"<meta name='{name}' content='{System.Net.WebUtility.HtmlEncode(value)}'>"));
    }
}