using Apps.Customer.io.Models.Request.Newsletter;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Utils.Json.Converters;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Entity;

public class UpdateNewsletterTranslationEntity
{
    public string? Body { get; set; }

    [Display("From ID")]
    [JsonConverter(typeof(StringToIntConverter), nameof(FromId))]
    public string? FromId { get; set; }

    [Display("Reply to ID")]
    [JsonConverter(typeof(StringToIntConverter), nameof(FromId))]
    public string? ReplyToId { get; set; }

    public string? Subject { get; set; }
    
    public string? Recipient { get; set; }

    [Display("Preheader Text")]
    public string? PreheaderText { get; set; }

    [Display("Amp Body")]
    public string? BodyAmp { get; set; }
    
    public UpdateNewsletterTranslationEntity(UpdateNewsletterTranslationRequest request)
    {
        Body = request.Body;
        FromId = request.FromId;
        ReplyToId = request.ReplyToId;
        Subject = request.Subject;
        Recipient = request.Recipient;
        PreheaderText = request.PreheaderText;
        BodyAmp = request.BodyAmp;
    }
}