using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Response
{
    public class ListCampaignsResponse
    {
        [JsonProperty("campaigns")]
        public List<Campaign> Campaigns { get; set; }
    }

    public class Campaign
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

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}
