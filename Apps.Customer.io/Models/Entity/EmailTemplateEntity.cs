using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Entity;

public class EmailTemplateEntity
{
    [Display("ID")]
    public string Id { get; set; }

    [Display("Name")]
    public string Name { get; set; }

    [Display("Creation date")]
    public DateTime Created { get; set; }

    [Display("Last updated")]
    public DateTime Updated { get; set; }

    public string Body { get; set; }

    public string Language { get; set; }

    public string Type { get; set; }

    [Display("From")]
    public string From { get; set; }

    [Display("From ID")]
    public string FromId { get; set; }

    [Display("Reply to")]
    public string ReplyTo { get; set; }

    [Display("Reply to ID")]
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

    public List<EmailHeader> Headers { get; set; }

    [Display("Amp body")]
    public string BodyAmp { get; set; }
}