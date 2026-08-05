using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Entity.DesignStudio;

public class DesignStudioEmailTranslationEntity
{
    [JsonProperty("language_group_id")]
    public string LanguageGroupId { get; set; } = string.Empty;

    [JsonProperty("language")]
    public string Language { get; set; } = string.Empty;

    [JsonProperty("is_template")]
    public bool IsTemplate { get; set; }

    [JsonProperty("is_linked")]
    public bool IsLinked { get; set; }

    [JsonProperty("available_languages")]
    public List<string> AvailableLanguages { get; set; } = [];

    [JsonProperty("created")]
    public long Created { get; set; }

    [JsonProperty("updated")]
    public long Updated { get; set; }

    [JsonProperty("content")]
    public DesignStudioEmailContentEntity Content { get; set; } = new();
}