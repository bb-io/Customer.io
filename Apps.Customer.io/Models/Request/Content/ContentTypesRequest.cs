using Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Customer.io.Models.Request.Content;

public class ContentTypesRequest
{
    [Display("Content types"), StaticDataSource(typeof(ContentTypeDataHandler))]
    public IEnumerable<string>? ContentTypes { get; set; }
}