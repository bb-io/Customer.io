using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Content;

public class ContentOptionalRequest
{
    [Display("Content ID"), DataSource(typeof(ContentDataHandler))]
    public string? ContentId { get; set; }
}