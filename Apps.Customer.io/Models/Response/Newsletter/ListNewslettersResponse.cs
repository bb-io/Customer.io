using Apps.Customer.io.Models.Entity;

namespace Apps.Customer.io.Models.Response.Newsletter;

public class ListNewslettersResponse
{
    public IEnumerable<NewsletterEntity> Newsletters { get; set; }
}