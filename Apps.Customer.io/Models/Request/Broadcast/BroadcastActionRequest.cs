using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Broadcast;

public class BroadcastActionRequest
{
    [Display("Broadcast ID"), DataSource(typeof(BroadcastDataHandler))]
    public string BroadcastId { get; set; } = string.Empty;

    [Display("Action ID"), DataSource(typeof(BroadcastActionDataHandler))]
    public string ActionId { get; set; } = string.Empty;
    
    public string? Language { get; set; }
}