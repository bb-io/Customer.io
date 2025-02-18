using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Content;

public class ContentRequest : ContentTypeRequest
{
    [Display("Content ID"), DataSource(typeof(ContentDataHandler))]
    public string ContentId { get; set; } = string.Empty;

    [Display("Action ID"), DataSource(typeof(ActionDataHandler))]
    public string? ActionId { get; set; }

    public string? Language { get; set; }
}