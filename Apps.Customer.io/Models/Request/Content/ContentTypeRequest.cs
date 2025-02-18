using Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Customer.io.Models.Request.Content;

public class ContentTypeRequest
{
    [Display("Content type"), StaticDataSource(typeof(ContentTypeDataHandler))]
    public string ContentType { get; set; } = string.Empty;
}