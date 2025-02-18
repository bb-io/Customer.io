using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Customer.io.Models.Request.Newsletter;

public class UpdateCampaignTranslationFromHtmlRequest
{
    public FileReference File { get; set; } = default!;
}