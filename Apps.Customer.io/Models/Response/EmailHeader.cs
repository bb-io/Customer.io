using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Response;

public class EmailHeader
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}