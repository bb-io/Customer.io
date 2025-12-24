using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Request.Campaign;

public class SearchCampaignsRequest
{
    [Display("Name contains")]
    public string? NameContains { get; set; }

    public IEnumerable<string>? Tags { get; set; }
}
