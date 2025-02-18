using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Response.Content;

public class SearchContentResponse
{
    [Display("Content collection")]
    public List<ContentResponse> Content { get; set; } = new();
}