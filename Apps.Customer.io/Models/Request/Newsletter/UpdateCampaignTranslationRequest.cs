using Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Request.Newsletter
{
    public class UpdateCampaignTranslationRequest
    {
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("body_amp")]
        public string? BodyAmp { get; set; }

        [JsonProperty("sending_state")]
        [StaticDataSource(typeof(SendingStateDataHandler))]
        public string? SendingState { get; set; }

        [JsonProperty("from_id")]
        public int? FromId { get; set; }

        [JsonProperty("reply_to_id")]
        public int? ReplyToId { get; set; }

        [JsonProperty("recipient")]
        public string? Recipient { get; set; }

        [JsonProperty("subject")]
        public string? Subject { get; set; }

        [JsonProperty("preheader_text")]
        public string? PreheaderText { get; set; }

        [JsonProperty("headers")]
        public string? Headers { get; set; }
    }
}
