using Apps.Customer.io.Models.Response;
using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Apps.Customer.io.Models.Entity;

public class EmailTemplateEntity
{
    [Display("ID")]
    public string Id { get; set; }

    [Display("Name")]
    public string Name { get; set; }

    [Display("Creation date")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? Created { get; set; }

    [Display("Last updated")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? Updated { get; set; }

    public string Body { get; set; }

    public string Language { get; set; }

    public string Type { get; set; }

    [Display("From")]
    public string From { get; set; }

    [Display("From ID")]
    public string FromId { get; set; }

    [Display("Reply to")]
    public string ReplyTo { get; set; }

    [Display("Reply to ID"), JsonProperty("reply_to_id")]
    public string ReplyToId { get; set; }

    public string Preprocessor { get; set; }

    public string Recipient { get; set; }

    public string Subject { get; set; }

    [Display("BCC")]
    public string Bcc { get; set; }

    [Display("Fake BCC")]
    public bool FakeBcc { get; set; }

    [Display("Preheader text")]
    public string PreheaderText { get; set; }
    
    [JsonConverter(typeof(EmailHeaderListConverter)), JsonProperty("headers")]
    public List<EmailHeader> Headers { get; set; } = new();

    [Display("Amp body")]
    public string BodyAmp { get; set; }
}