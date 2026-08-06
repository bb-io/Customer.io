using System.Net.Mime;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Models.Request.DesignStudio;
using Apps.Customer.io.Models.Response.Broadcast;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Services;
using Apps.Customer.io.Services.Models;
using Apps.Customer.io.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Customer.io.Actions;

[ActionList("Design studio emails")]
public class DesignStudioEmailActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
    : CustomerIoInvocable(invocationContext)
{
    private readonly DesignStudioEmailService _service = new(invocationContext);

    [Action("Download design studio email", Description = "Download a design studio email translation")]
    public async Task<FileResponse> DownloadDesignStudioEmail([ActionParameter] DownloadDesignStudioEmailRequest input)
    {
        var stream = await _service.DownloadContentAsync(new ContentRequest
        {
            ContentType = ContentTypes.DesignStudioEmail,
            ContentId = input.EmailId,
            Language = input.Language,
            FileFormat = input.FileFormat
        });

        string extension = input.FileFormat == MediaTypeNames.Application.Json ? "json" : "html";
        var fileReference = await fileManagementClient.UploadAsync(
            stream,
            input.FileFormat ?? MediaTypeNames.Text.Html,
            $"{input.EmailId}_{input.Language}.{extension}");

        return new() { File = fileReference };
    }

    [Action("Update Design Studio email", Description = "Update a design studio email translation from a file")]
    public async Task<ContentResponse> UpdateDesignStudioEmail([ActionParameter] UploadDesignStudioEmailRequest input)
    {
        await using var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var uploadStream = await FileTransformer.ToHtml(fileStream, input.File);

        return await _service.UploadContentAsync(uploadStream, new ContentUploadInput()
        {
            ContentType = ContentTypes.DesignStudioEmail,
            ContentId = input.EmailId,
            Language = input.Language,
        });
    }
}