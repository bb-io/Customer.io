using Apps.Customer.io.Models.Entity;

namespace Apps.Customer.io.Models.Response.TransactionalMessage;

public class ListMessagesResponse
{
    public IEnumerable<TransactionalMessageEntity> Messages { get; set; }
}