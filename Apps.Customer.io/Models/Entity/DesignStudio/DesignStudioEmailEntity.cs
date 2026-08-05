using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Entity.DesignStudio;

public class DesignStudioEmailEntity
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}