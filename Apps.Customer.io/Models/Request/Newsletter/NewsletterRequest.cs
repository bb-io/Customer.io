using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Newsletter;

public class NewsletterRequest
{
    [Display("Newsletter ID"), DataSource(typeof(NewsletterDataHandler))]
    public string NewsletterId { get; set; } = string.Empty;
    
    public string? Language { get; set; }
}