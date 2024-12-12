using System.Text.Json.Serialization;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Customer.io.Polling.Models
{
    public class NewsletterEventResponse
    {
        [Display("Newsletters")]
        [JsonProperty("newsletters")]
        public List<Newsletter> Newsletters { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }
    }

    public class Newsletter
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deduplicate_id")]
        public string DeduplicateId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("updated")]
        public long Updated { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new();

        [JsonProperty("subscription_topic_id")]
        public int? SubscriptionTopicId { get; set; }

        [JsonProperty("content_ids")]
        public List<int> ContentIds { get; set; } = new();
    }

}
