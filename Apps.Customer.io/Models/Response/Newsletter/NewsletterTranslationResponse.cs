using Apps.Customer.io.Models.Entity;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Response.Newsletter;

public class NewsletterTranslationResponse
{
    [JsonProperty("content")]
    public NewsletterTranslationEntity Content { get; set; }
}