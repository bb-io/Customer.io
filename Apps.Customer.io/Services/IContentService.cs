using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Models.Response.Content;

namespace Apps.Customer.io.Services;

public interface IContentService
{
    public Task<Stream> DownloadContentAsync(ContentRequest downloadInput);
    public Task<ContentResponse> UploadContentAsync(Stream htmlStream, string? language, string? actionId);
}