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
        this.Id = entity.Id;
        this.NewsletterId = entity.NewsletterId;
        this.DeduplicateId = entity.DeduplicateId;
        this.Name = entity.Name;
        this.Layout = entity.Layout;
        this.Body = entity.Body;
        this.BodyAmp = entity.BodyAmp;
        this.Language = entity.Language;
        this.Type = entity.Type;
        this.From = entity.From;
        this.FromId = entity.FromId;
        this.ReplyTo = entity.ReplyTo;
        this.ReplyToId = entity.ReplyToId;
        this.Preprocessor = entity.Preprocessor;
    }
}