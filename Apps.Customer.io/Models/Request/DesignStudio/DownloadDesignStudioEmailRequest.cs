using System.Net.Mime;
using Apps.Customer.io.DataSourceHandlers;
using Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.DesignStudio;

public class DownloadDesignStudioEmailRequest
{
    [Display("Email ID"), DataSource(typeof(DesignStudioEmailDataHandler))]
    public string EmailId { get; set; } = string.Empty;

    [Display("Language")]
    public string Language { get; set; } = string.Empty;

    [Display("File format", Description = "Format of the file to be downloaded, defaults to an interoperable HTML.")]
    [StaticDataSource(typeof(DownloadFileFormatHandler))]
    public string? FileFormat { get; set; } = MediaTypeNames.Text.Html;
}