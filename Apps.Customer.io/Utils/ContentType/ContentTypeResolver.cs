using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common.Exceptions;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Apps.Customer.io.Utils.ContentType;

public static class ContentTypeResolver
{
    public static string Resolve(string fileContent, string? inputContentType)
    {
        bool isJson = fileContent.IsJson();
        string? fileContentType = isJson ? FromJson(fileContent) : FromHtml(fileContent);
        string? contentType = string.IsNullOrWhiteSpace(fileContentType) ? inputContentType : fileContentType;

        if (string.IsNullOrWhiteSpace(contentType))
            throw new PluginMisconfigurationException("Couldn't determine the content type. Please specify it in the input");

        return contentType;
    }

    private static string? FromJson(string json)
    {
        return JsonConvert.DeserializeObject<JsonResponseWithMetadata>(json)?.ContentType;
    }

    private static string? FromHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        return doc.GetMeta(HtmlConstants.ContentType);
    }
}