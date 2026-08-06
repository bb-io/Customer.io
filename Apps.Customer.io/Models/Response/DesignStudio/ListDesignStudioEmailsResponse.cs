using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Entity.DesignStudio;
using Newtonsoft.Json;

namespace Apps.Customer.io.Models.Response.DesignStudio;

public class ListDesignStudioEmailsResponse
{
    [JsonProperty("emails")]
    public List<DesignStudioEmailEntity> Emails { get; set; } = [];
}