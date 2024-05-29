using Apps.Customer.io.Models.Response;
using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Entity;

public class NewsletterTranslationEntity
{
    [Display("ID"), JsonProperty("id")]
    public string Id { get; set; }

    [Display("Newsletter ID"), JsonProperty("newsletter_id")]
    public string NewsletterId { get; set; }

    [Display("Deduplicate ID"), JsonProperty("deduplicate_id")]
    public string DeduplicateId { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("layout")]
    public string Layout { get; set; }
    
    [JsonProperty("body")]
    public string Body { get; set; }

    [Display("Amp Body"), JsonProperty("body_amp")]
    public string BodyAmp { get; set; }

    [JsonProperty("language")]
    public string Language { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("from")]
    public string From { get; set; }

    [Display("From ID"), JsonProperty("from_id")]
    public string FromId { get; set; }

    [Display("Reply to"), JsonProperty("reply_to")]
    public string ReplyTo { get; set; }

    [Display("Reply to ID"), JsonProperty("reply_to_id")]
    public string ReplyToId { get; set; }

    [JsonProperty("preprocessor")]
    public string Preprocessor { get; set; }
    
    [JsonProperty("recipient")]
    public string Recipient { get; set; }
    
    [JsonProperty("subject")]
    public string Subject { get; set; }
    
    [Display("BCC"), JsonProperty("bcc")]
    public string Bcc { get; set; }

    [Display("Fake BCC"), JsonProperty("fake_bcc")]
    public bool FakeBcc { get; set; }

    [Display("Preheader text"), JsonProperty("preheader_text")]
    public string PreheaderText { get; set; }
    
    [JsonConverter(typeof(EmailHeaderListConverter)), JsonProperty("headers")]
    public List<EmailHeader> Headers { get; set; } = new();
}