using Apps.Customer.io.Models.Request.Content;

namespace Apps.Customer.io.Services.Models;

public class ContentUploadInput
{
    public ContentUploadInput() { }

    public ContentUploadInput(UploadContentRequest request)
    {
        ActionId = request.ActionId;
        Language = request.Language;
        UpdateSource = request.UpdateSource;
    }
    
    public string? ContentId { get; set; }
    public string? ActionId { get; set; }
    public string? Language { get; set; }
    public bool? UpdateSource { get; set; }
}