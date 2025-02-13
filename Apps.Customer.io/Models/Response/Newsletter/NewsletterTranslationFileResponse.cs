using Apps.Customer.io.Models.Entity;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Customer.io.Models.Response.Newsletter;

public class NewsletterTranslationFileResponse : NewsletterTranslationEntity
{
    [Display("HTML document")]
    public FileReference File { get; set; }
    
    public NewsletterTranslationFileResponse(NewsletterTranslationEntity entity, FileReference file)
    {
        File = file;
        Id = entity.Id;
        NewsletterId = entity.NewsletterId;
        DeduplicateId = entity.DeduplicateId;
        Name = entity.Name;
        Layout = entity.Layout;
        Body = entity.Body;
        BodyAmp = entity.BodyAmp;
        Language = entity.Language;
        Type = entity.Type;
        From = entity.From;
        FromId = entity.FromId;
        ReplyTo = entity.ReplyTo;
        ReplyToId = entity.ReplyToId;
        Preprocessor = entity.Preprocessor;
        PreheaderText = entity.PreheaderText;
        Subject = entity.Subject;
    }
}