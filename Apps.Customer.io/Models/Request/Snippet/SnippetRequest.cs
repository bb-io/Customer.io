using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Snippet;

public class SnippetRequest
{
    [Display("Snippet name"), DataSource(typeof(SnippetDataHandler))]
    public string SnippetName { get; set; } = string.Empty;
}