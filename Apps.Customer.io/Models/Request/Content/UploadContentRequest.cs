using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Customer.io.Models.Request.Content;

public class UploadContentRequest : ContentTypeRequest
{
    public FileReference File { get; set; } = default!;

    [Display("Action ID")]
    public string? ActionId { get; set; }

    public string Language { get; set; } 
}