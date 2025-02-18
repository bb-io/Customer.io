using Apps.Customer.io.Models.Entity;

namespace Apps.Customer.io.Models.Response.Broadcast;

public class BroadcastTranslationResponse
{
    public BroadcastActionEntity Action { get; set; } = new();
}