using Apps.Customer.io.Models.Response;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Entity;

public class NewsletterTranslationEntity
{
    [Display("ID")]
    public string Id { get; set; }

    [Display("Newsletter ID")]
    public string NewsletterId { get; set; }

    [Display("Deduplicate ID")]
    public string DeduplicateId { get; set; }

    public string Name { get; set; }
    
    public string Layout { get; set; }
    
    public string Body { get; set; }

    [Display("Amp Body")]
    public string BodyAmp { get; set; }

    public string Language { get; set; }
    
    public string Type { get; set; }

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
}