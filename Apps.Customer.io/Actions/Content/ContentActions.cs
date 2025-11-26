using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Models.Response.Broadcast;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Services.Factories;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using System.Net.Mime;

namespace Apps.Customer.io.Actions.Content;

[ActionList("Content")]
public class ContentActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : CustomerIoInvocable(invocationContext)
{
    private readonly ContentServiceFactory _contentServiceFactory = new(invocationContext);
    
    [Action("Download content", Description = "Download content in HTML format based on specified ID")]
    public async Task<FileResponse> DownloadContentAsync([ActionParameter] ContentRequest contentRequest)
    {
        var service = _contentServiceFactory.GetService(contentRequest.ContentType);
        var stream = await service.DownloadContentAsync(contentRequest.ContentId, contentRequest.Language,
            contentRequest.ActionId, contentRequest.FileFormat);

        var extension = contentRequest.FileFormat == MediaTypeNames.Text.Html ? "html" : "json";

        var fileName = string.IsNullOrEmpty(contentRequest.ActionId)
            ? $"{contentRequest.ContentId}.{extension}"
            : $"{contentRequest.ContentId}_{contentRequest.ActionId}.{extension}";

        var fileReference = await fileManagementClient.UploadAsync(stream, contentRequest.FileFormat ?? MediaTypeNames.Text.Html, fileName);

        return new()
        {
            File = fileReference
        };
    }
    
    [Action("Upload content", Description = "Update content from HTML file")]
    public async Task<ContentResponse> UploadContent([ActionParameter] UploadContentRequest uploadContentRequest)
    {
        if (String.IsNullOrEmpty(uploadContentRequest.Language))
        {
            throw new PluginMisconfigurationException("Language input cannot be empty.");
        }

        var service = _contentServiceFactory.GetService(uploadContentRequest.ContentType);
        var fileStream = await fileManagementClient.DownloadAsync(uploadContentRequest.File);
        
        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        return await service.UploadContentAsync(memoryStream, uploadContentRequest.Language, uploadContentRequest.ActionId);
    }
}