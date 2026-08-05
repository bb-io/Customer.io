using Apps.Customer.io.Api;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity.Content;
using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Models.Response.Content;
using Apps.Customer.io.Models.Response.DesignStudio;
using Apps.Customer.io.Utils.Converters;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Customer.io.Services;

public class DesignStudioEmailService(InvocationContext context) : CustomerIoInvocable(context), IContentService
{
    public async Task<Stream> DownloadContentAsync(ContentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Language))
            throw new PluginMisconfigurationException("Language is required for Design Studio emails");

        string endpoint = $"v1/design_studio/emails/{request.ContentId}/languages/{request.Language}";
        var designStudioRequest = new CustomerIoRequest(endpoint, Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<DesignStudioEmailTranslationResponse>(designStudioRequest);
        var content = response.EmailTranslation.Content;

        return ContentConverter.Serialize(request, new ContentDocument
        {
            Name = content.Subject,
            Subject = content.Subject,
            PreheaderText = content.PreheaderText,
            Body = content.Html
        });
    }

    public Task<ContentResponse> UploadContentAsync(Stream htmlStream, string? language, string? actionId)
    {
        throw new NotImplementedException();
    }
}