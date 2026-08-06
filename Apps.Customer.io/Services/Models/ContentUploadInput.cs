namespace Apps.Customer.io.Services.Models;

public class ContentUploadInput
{
    public string ContentType { get; set; } = string.Empty;
    public string? ContentId { get; set; }
    public string? ActionId { get; set; }
    public string? Language { get; set; }
    public bool? UpdateSource { get; set; }
}