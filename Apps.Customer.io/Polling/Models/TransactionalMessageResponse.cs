using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apps.Customer.io.Polling.Models
{
    public class TransactionalMessageResponse
    {
        [JsonPropertyName("messages")]
        public List<TransactionalMessage> Messages { get; set; }
    }

    public class TransactionalMessage
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("send_to_unsubscribed")]
        public bool SendToUnsubscribed { get; set; }

        [JsonProperty("link_tracking")]
        public bool LinkTracking { get; set; }

        [JsonProperty("open_tracking")]
        public bool OpenTracking { get; set; }

        [JsonProperty("hide_message_body")]
        public bool HideMessageBody { get; set; }

        [JsonProperty("queue_drafts")]
        public bool QueueDrafts { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }
    }
}
