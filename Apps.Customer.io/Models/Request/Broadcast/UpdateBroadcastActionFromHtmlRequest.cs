using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Customer.io.Models.Request.Broadcast;

public class UpdateBroadcastActionFromHtmlRequest
{
    public FileReference File { get; set; } = default!;
}