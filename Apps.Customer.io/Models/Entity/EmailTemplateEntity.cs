using Apps.Customer.io.Models.Response;
using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Apps.Customer.io.Models.Entity;

public class EmailTemplateEntity
{
    [Display("Translation ID")]
    public string Id { get; set; } = string.Empty;

    [Display("Name")]
    public string Name { get; set; } = string.Empty;

    [Display("Creation date"), JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? Created { get; set; }

    [Display("Last updated"), JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? Updated { get; set; }

    public string Body { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    [Display("From")]
    public string From { get; set; } = string.Empty;

    [Display("From ID")]
    public string FromId { get; set; } = string.Empty;

    [Display("Reply to")]
    public string ReplyTo { get; set; } = string.Empty;

    [Display("Reply to ID"), JsonProperty("reply_to_id")]
    public string ReplyToId { get; set; } = string.Empty;

    public string Preprocessor { get; set; } = string.Empty;

    public string Recipient { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    [Display("BCC")]
    public string Bcc { get; set; } = string.Empty;

    [Display("Fake BCC")]
    public bool FakeBcc { get; set; }

    [Display("Preheader text")]
    public string PreheaderText { get; set; } = string.Empty;
    
    [JsonConverter(typeof(EmailHeaderListConverter)), JsonProperty("headers")]
    public List<EmailHeader> Headers { get; set; } = new();

    [Display("Amp body")]
    public string BodyAmp { get; set; } = string.Empty;
}