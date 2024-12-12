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
        public string Id { get; set; }

        [JsonProperty("campaign_id")]
        public int CampaignId { get; set; }

        [JsonProperty("deduplicate_id")]
        public string DeduplicateId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("updated")]
        public long Updated { get; set; }

        [JsonProperty("sending_state")]
        public string SendingState { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("reply_to")]
        public string ReplyTo { get; set; }

        [JsonProperty("recipient")]
        public string Recipient { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("preheader_text")]
        public string PreheaderText { get; set; }

        [JsonProperty("body_plain")]
        public string BodyPlain { get; set; }

        [JsonProperty("body_amp")]
        public string BodyAmp { get; set; }

        [JsonProperty("headers")]
        public string Headers { get; set; }
    }
}
