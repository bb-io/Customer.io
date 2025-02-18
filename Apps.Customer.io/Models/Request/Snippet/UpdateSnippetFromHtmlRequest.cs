using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Customer.io.Models.Request.Snippet;

public class UpdateSnippetFromHtmlRequest : SnippetRequest
{
    public FileReference File { get; set; } = default!;
}