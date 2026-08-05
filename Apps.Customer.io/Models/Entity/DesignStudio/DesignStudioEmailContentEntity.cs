using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Entity.DesignStudio;

public class DesignStudioEmailContentEntity
{
    [JsonProperty("subject")]
    public string Subject { get; set; } = string.Empty;

    [JsonProperty("preheader_text")]
    public string PreheaderText { get; set; } = string.Empty;

    [JsonProperty("html")]
    public string Html { get; set; } = string.Empty;

    [JsonProperty("amp")]
    public string Amp { get; set; } = string.Empty;

    [JsonProperty("text")]
    public string Text { get; set; } = string.Empty;
}