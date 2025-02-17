using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Response.TransactionalMessage
{
    public class CampaignMessageTranslationResponse
    {
        [JsonProperty("action")]
        public ActionEntity Answer { get; set; }
    }

    public class ActionEntity
    {
        [JsonProperty("id")]
        [Display("Action ID", Description ="Action ID")]
        public string Id { get; set; }

        [JsonProperty("campaign_id")]
        [Display("Campaign ID", Description ="Campaign ID")]
        public int CampaignId { get; set; }


        [Display("Parent action ID", Description = "Parent action")]
        public int ParentActionId { get; set; }

        [JsonProperty("deduplicate_id")]
        [Display("Deduplicate ID", Description ="Deduplicate")]
        public string DeduplicateId { get; set; }

        [JsonProperty("name")]
        [Display("Name", Description ="Name")]
        public string Name { get; set; }

        [JsonProperty("layout")]
        [Display("Layout", Description ="Layout")]
        public string Layout { get; set; }

        [Display("Created", Description ="Created")]
        public long Created { get; set; } 

        [Display("Updated", Description ="Updated")]
        public long Updated { get; set; }

        [JsonProperty("body")]
        [Display("Body", Description ="Body")]
        public string Body { get; set; }

        [Display("Body AMP", Description ="Body AMP")]
        public string BodyAmp { get; set; }

        [JsonProperty("language")]
        [Display("Language", Description ="Language")]
        public string Language { get; set; }

        [Display("Type", Description ="Type")]
        public string Type { get; set; }

        [Display("Sending state", Description ="State")]
        public string SendingState { get; set; }

        [Display("From", Description = "Message from")]
        public string From { get; set; }

        [Display("From ID", Description = "Message from ID")]
        public int? FromId { get; set; }

        [Display("Reply to", Description ="Reply to")]
        public string ReplyTo { get; set; }

        [Display("Reply to ID", Description = "Reply to ID")]
        public int? ReplyToId { get; set; }

        [Display("Preprocessor")]
        public string Preprocessor { get; set; }

        [Display("Recipient")]
        public string Recipient { get; set; }

        [Display("Subject")]
        public string Subject { get; set; }

        [Display("BCC")]
        public string Bcc { get; set; }

        [Display("Fake BCC")]
        public bool FakeBcc { get; set; }

        [Display("Preheader text")]
        public string PreheaderText { get; set; }

        [Display("Headers")]
        public string Headers { get; set; }
    }

}
