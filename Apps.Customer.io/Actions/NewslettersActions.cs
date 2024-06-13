using System.Text;
using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request.Newsletter;
using Apps.Customer.io.Models.Response.Newsletter;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Customer.io.Actions;

[ActionList]
public class NewslettersActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : CustomerIoInvocable(invocationContext)
{
    [Action("Get translation of a newsletter",
        Description = "Get information about a translation of an individual newsletter")]
    public async Task<NewsletterTranslationFileResponse> GetNewsletterTranslation(
        [ActionParameter] NewsletterRequest input)
    {
        return await HandleNewsletterTranslation(input, Method.Get);
    }

    [Action("Update translation of a newsletter", Description = "Update the translation of a newsletter variant")]
    public async Task<NewsletterTranslationFileResponse> UpdateNewsletterTranslation(
        [ActionParameter] NewsletterRequest input,
        [ActionParameter] UpdateNewsletterTranslationRequest payload)
    {
        return await HandleNewsletterTranslation(input, Method.Put, payload);
    }

    private async Task<NewsletterTranslationFileResponse> HandleNewsletterTranslation(
        NewsletterRequest input, 
        Method method, 
        UpdateNewsletterTranslationRequest? payload = null)
    {
        var endpoint = $"v1/newsletters/{input.NewsletterId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, method, Creds);

        if (payload != null)
        {
            request.WithJsonBody(payload, JsonConfig.Settings);
        }

        var response = await Client.ExecuteWithErrorHandling<NewsletterTranslationResponse>(request);
        
        var html = response.Content.Body;
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
        var fileReference = await fileManagementClient.UploadAsync(stream, "text/html", $"{response.Content?.Name} [{response.Content?.Id}].html");
        
        return new NewsletterTranslationFileResponse(response.Content ?? new NewsletterTranslationEntity(), fileReference);
    }
}