using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Apps.Customer.io.Models.Entity;

public class BroadcastActionEntity
{
    [Display("Action ID")] public string Id { get; set; }

    [Display("Broadcast ID")] public string BroadcastId { get; set; }

    public string Name { get; set; }

    public string Layout { get; set; }

    public string Body { get; set; }

    public string Type { get; set; }

    [Display("Creation date")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? Created { get; set; }

    [Display("Last updated")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? Updated { get; set; }

    [Display("Sending state")] public string SendingState { get; set; }

    public string Language { get; set; }

    public string From { get; set; }

    [Display("From ID")] public string FromId { get; set; }

    [Display("Reply to")] public string ReplyTo { get; set; }

    [Display("Reply to ID")] public string ReplyToId { get; set; }

    public string Preprocessor { get; set; }

    public string Recipient { get; set; }

    public string Subject { get; set; }

    [Display("BCC")] public string Bcc { get; set; }

    [Display("Fake BCC")] public bool FakeBcc { get; set; }

    [Display("Preheader text")] public string PreheaderText { get; set; }

    public List<EmailHeader> Headers { get; set; }

    [Display("Amp body")] public string BodyAmp { get; set; }
}