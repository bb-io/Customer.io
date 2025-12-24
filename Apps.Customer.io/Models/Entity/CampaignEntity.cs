using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Apps.Customer.io.Models.Entity;

public class CampaignEntity
{
    [Display("Campaign ID")]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string? Description { get; set; }

    [Display("Creation date"), JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime Created { get; set; }

    [Display("Last updated"), JsonConverter(typeof(NullableUnixDateTimeConverter))]
    public DateTime? Updated { get; set; }

    [Display("First started"), JsonConverter(typeof(NullableUnixDateTimeConverter))]
    public DateTime? FirstStarted { get; set; }

    public bool Active { get; set; }

    public string State { get; set; }

    public IEnumerable<string> Tags { get; set; }

    public IEnumerable<ActionEntity> Actions { get; set; }
}
