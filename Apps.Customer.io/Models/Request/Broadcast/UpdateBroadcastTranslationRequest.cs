using Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Utils.Json.Converters;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Request.Broadcast;

public class UpdateBroadcastTranslationRequest
{
    public string? Body { get; set; }
    
    [Display("Sending state"), StaticDataSource(typeof(SendingStateDataHandler))]
    public string? SendingState { get; set; }
    
    [Display("From ID"), JsonConverter(typeof(StringToIntConverter), nameof(FromId))]
    public string? FromId { get; set; }

    [Display("Reply to ID"), JsonConverter(typeof(StringToIntConverter), nameof(FromId))]
    public string? ReplyToId { get; set; }
    
    public string? Subject { get; set; }
    
    public string? Recipient { get; set; }

    [Display("Preheader Text")]
    public string? PreheaderText { get; set; }

    [Display("Amp Body")]
    public string? BodyAmp { get; set; }
}