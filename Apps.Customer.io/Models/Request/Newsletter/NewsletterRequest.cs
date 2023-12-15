using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Models.Request.Newsletter;

public class NewsletterRequest
{
    [Display("Newsletter")]
    [DataSource(typeof(NewsletterDataHandler))]
    public string NewsletterId { get; set; }
    
    public string Language { get; set; }
}