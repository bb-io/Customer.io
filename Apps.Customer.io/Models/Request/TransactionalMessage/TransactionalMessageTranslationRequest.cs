using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.TransactionalMessage;

public class TransactionalMessageTranslationRequest
{
    [Display("Transactional message")]
    [DataSource(typeof(TransactionalMessageDataHandler))]
    public string TransactionalMessageId { get; set; }

    public string? Language { get; set; }
}