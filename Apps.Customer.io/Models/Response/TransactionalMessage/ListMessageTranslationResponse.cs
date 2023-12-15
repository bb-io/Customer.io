using Apps.Customer.io.Models.Entity;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Response.TransactionalMessage;

public class ListMessageTranslationResponse
{
    [Display("Translation")]
    public EmailTemplateEntity Content { get; set; }
}