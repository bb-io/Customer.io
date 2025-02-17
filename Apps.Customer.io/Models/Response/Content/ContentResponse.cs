using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Response.Content;

public class ContentResponse
{
    [Display("Content ID")]
    public string ContentId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    [Display("Content type")]
    public string ContentType { get; set; } = string.Empty;

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    [Display("Updated at")]
    public DateTime UpdatedAt { get; set; }
}