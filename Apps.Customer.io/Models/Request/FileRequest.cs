using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Customer.io.Models.Request;

public class FileRequest
{
    public FileReference File { get; set; } = default!;
}