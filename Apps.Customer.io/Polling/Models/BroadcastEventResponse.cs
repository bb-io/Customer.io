using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Customer.io.Polling.Models
{
    public class BroadcastEventResponse
    {
        [Display("Broadcasts")]
        [JsonProperty("broadcasts")]
        public List<Broadcast> Broadcasts { get; set; }
    }

    public class Broadcast
    {
        [Display("Broadcast ID")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deduplicate_id")]
        public string DeduplicateId { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("updated")]
        public long Updated { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("actions")]
        public List<string> Actions { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("first_started")]
        public long FirstStarted { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }
    }
}
