using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Newsletter
{
    public class CampaignTranslationRequest
    {
        [Display("Campaign ID"), DataSource(typeof(CampaignDataHandler))]
        public string CampaignId { get; set; } = string.Empty;

        [Display("Action ID"), DataSource(typeof(CampaignActionDataHandler))]
        public string ActionId { get; set; } = string.Empty;

        [Display("Language")]
        public string? Language { get; set; }
    }
}
