using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Campaign;

public class SearchCampaignsRequest
{
    [Display("Name contains")]
    public string? NameContains { get; set; }

    [Display("Tags"), DataSource(typeof(CampaignTagsDataHandler))]
    public IEnumerable<string>? Tags { get; set; }
}
