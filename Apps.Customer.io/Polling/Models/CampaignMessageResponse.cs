using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Customer.io.Polling.Models
{
    public class CampaignMessageResponse
    {
        [Display("Actions")]
        [JsonProperty("actions")]
        public List<CampaignMessageAction> Actions { get; set; }
    }

    public class CampaignMessageAction
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("updated")]
        public long Updated { get; set; }
    }
}
