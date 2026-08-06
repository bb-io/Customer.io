using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Response;
public class JsonResponseWithMetadata
{
    [JsonProperty("contentId")]
    public string? ContentId { get; set; }

    [JsonProperty("actionId")]
    public string? ActionId { get; set;}

    [JsonProperty("contentType")]
    public string? ContentType { get; set;}

    [JsonProperty("messageId")]
    public string? MessageId { get; set; }

    [JsonProperty("name")]
    public object? Name { get; set; }

    [JsonProperty("body")]
    public object? Body { get; set; }
    
    [JsonProperty("subject")]
    public string? Subject { get; set; }

    [JsonProperty("preheaderText")]
    public string? PreheaderText { get; set; }
}
