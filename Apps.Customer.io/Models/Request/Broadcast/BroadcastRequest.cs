using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Broadcast;

public class BroadcastRequest
{
    [Display("Broadcast")]
    [DataSource(typeof(BroadcastDataHandler))]
    public string BroadcastId { get; set; }
}