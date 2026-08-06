using Apps.Customer.io.Models.Entity.DesignStudio;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Response.DesignStudio;

public class DesignStudioEmailTranslationResponse
{
    [JsonProperty("email_translation")]
    public DesignStudioEmailTranslationEntity EmailTranslation { get; set; } = new();
}