using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Broadcast;

public class BroadcastActionRequest
{
    [Display("Broadcast")]
    [DataSource(typeof(BroadcastDataHandler))]
    public string BroadcastId { get; set; }
    
    [Display("Action")]
    [DataSource(typeof(BroadcastActionDataHandler))]
    public string ActionId { get; set; }
    
    public string? Language { get; set; }
}