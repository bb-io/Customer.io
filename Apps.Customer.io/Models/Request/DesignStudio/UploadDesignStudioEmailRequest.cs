using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Customer.io.Models.Request.DesignStudio;

public class UploadDesignStudioEmailRequest
{
    [Display("Email ID"), DataSource(typeof(DesignStudioEmailDataHandler))]
    public string? EmailId { get; set; }

    [Display("Language")]
    public string Language { get; set; } = string.Empty;

    [Display("File")] 
    public FileReference File { get; set; } = null!;
}