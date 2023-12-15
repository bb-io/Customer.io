namespace Apps.Customer.io.Models.Entity;

public class NewsletterEntity
{
    public string Id { get; set; }
    
    public string? Name { get; set; }
    
    public string Subject { get; set; }

    public string DisplayName => Name ?? Subject;
}