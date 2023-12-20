using Apps.Customer.io.Models.Entity;

namespace Apps.Customer.io.Models.Response.Broadcast;

public class ListBroadcastActionsResponse
{
    public IEnumerable<BroadcastActionEntity> Actions { get; set; }
}