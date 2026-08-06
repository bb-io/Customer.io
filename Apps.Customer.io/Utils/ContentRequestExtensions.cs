using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Services.Models;

namespace Apps.Customer.io.Utils;

public static class ContentRequestExtensions
{
    public static ContentUploadInput ToUploadInput(this UploadContentRequest request)
    {
        return new() 
        {
            ContentType = request.ContentType,
            ActionId = request.ActionId,
            Language = request.Language,
            UpdateSource = request.UpdateSource
        };
    }
}