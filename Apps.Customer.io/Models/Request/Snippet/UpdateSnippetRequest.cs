using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Request.Snippet;

public class UpdateSnippetRequest : SnippetRequest
{
    [Display("Value")]
    public string Value { get; set; } = string.Empty;
}