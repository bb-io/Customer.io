using Apps.Customer.io.DataSourceHandlers;
using Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System.Net.Mime;

namespace Apps.Customer.io.Models.Request.Content;

public class ContentRequest : ContentTypeRequest
{
    [Display("Content ID"), DataSource(typeof(ContentDataHandler))]
    public string ContentId { get; set; } = string.Empty;

    [Display("Action ID"), DataSource(typeof(ActionDataHandler))]
    public string? ActionId { get; set; }

    public string? Language { get; set; }

    [Display("File format", Description = "Format of the file to be downloaded, defaults to an interoperable HTML.")]
    [StaticDataSource(typeof(DownloadFileFormatHandler))]
    public string? FileFormat { get; set; } = MediaTypeNames.Text.Html;
}