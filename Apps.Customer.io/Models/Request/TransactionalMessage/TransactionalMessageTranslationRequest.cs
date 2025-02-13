using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.TransactionalMessage;

public class TransactionalMessageTranslationRequest
{
    [Display("Transactional message ID")]
    [DataSource(typeof(TransactionalMessageDataHandler))]
    public string TransactionalMessageId { get; set; } = string.Empty;

    public string? Language { get; set; }
}