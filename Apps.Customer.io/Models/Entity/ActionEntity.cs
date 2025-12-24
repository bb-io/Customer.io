using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Entity;

public class ActionEntity
{
    [Display("Action ID")]
    public int Id { get; set; }

    [Display("Action type")]
    public string Type { get; set; }
}
