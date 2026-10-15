using Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Customer.io.Models.Request.Content;

public class UploadContentRequest
{
    [Display("Content type", Description = "Used only if the file doesn't contain content type metadata")]
    [StaticDataSource(typeof(ContentTypeDataHandler))]
    public string? ContentType { get; set; }
    
    public FileReference File { get; set; } = default!;

    [Display("Action ID")]
    public string? ActionId { get; set; }

    public string? Language { get; set; }

    [Display("Update source")]
    public bool? UpdateSource { get; set; }
}