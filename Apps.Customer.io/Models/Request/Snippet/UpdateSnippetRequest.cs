using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Request.Snippet;

public class UpdateSnippetRequest
{
    [Display("Name")]
    public string Name { get; set; } = string.Empty;
    
    [Display("Value")]
    public string Value { get; set; } = string.Empty;
}