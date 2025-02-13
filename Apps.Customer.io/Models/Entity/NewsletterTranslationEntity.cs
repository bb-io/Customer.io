using Apps.Customer.io.Models.Response;
using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Entity;

public class NewsletterTranslationEntity
{
    [Display("Newsletter translation ID"), JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [Display("Newsletter ID"), JsonProperty("newsletter_id")]
    public string NewsletterId { get; set; } = string.Empty;

    [Display("Deduplicate ID"), JsonProperty("deduplicate_id")]
    public string DeduplicateId { get; set; } = string.Empty;
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("layout")]
    public string Layout { get; set; } = string.Empty;
    
    [JsonProperty("body")]
    public string Body { get; set; } = string.Empty;

    [Display("Amp Body"), JsonProperty("body_amp")]
    public string BodyAmp { get; set; } = string.Empty;

    [JsonProperty("language")]
    public string Language { get; set; } = string.Empty;
    
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("from")]
    public string From { get; set; } = string.Empty;

    [Display("From ID"), JsonProperty("from_id")]
    public string FromId { get; set; } = string.Empty;

    [Display("Reply to"), JsonProperty("reply_to")]
    public string ReplyTo { get; set; } = string.Empty;

    [Display("Reply to ID"), JsonProperty("reply_to_id")]
    public string ReplyToId { get; set; } = string.Empty;

    [JsonProperty("preprocessor")]
    public string Preprocessor { get; set; } = string.Empty;
    
    [JsonProperty("recipient")]
    public string Recipient { get; set; } = string.Empty;
    
    [JsonProperty("subject")]
    public string Subject { get; set; } = string.Empty;
    
    [Display("BCC"), JsonProperty("bcc")]
    public string Bcc { get; set; } = string.Empty;

    [Display("Fake BCC"), JsonProperty("fake_bcc")]
    public bool FakeBcc { get; set; }

    [Display("Preheader text"), JsonProperty("preheader_text")]
    public string PreheaderText { get; set; } = string.Empty;
    
    [JsonConverter(typeof(EmailHeaderListConverter)), JsonProperty("headers")]
    public List<EmailHeader> Headers { get; set; } = new();
}