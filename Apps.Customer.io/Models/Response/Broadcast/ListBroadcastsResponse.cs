using Apps.Customer.io.Models.Entity;

namespace Apps.Customer.io.Models.Response.Broadcast;

public class ListBroadcastsResponse
{
    public IEnumerable<BroadcastEntity> Broadcasts { get; set; }
}