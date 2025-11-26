using Apps.Customer.io.Models.Response.Content;

namespace Apps.Customer.io.Services;

public interface IContentService
{
    public Task<Stream> DownloadContentAsync(string contentId, string? language, string? actionId, string? fileFormat);
    public Task<ContentResponse> UploadContentAsync(Stream htmlStream, string? language, string? actionId);
}