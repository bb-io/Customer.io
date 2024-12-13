using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request.Newsletter;
using Apps.Customer.io.Models.Response.Newsletter;
using Apps.Customer.io.Models.Response.TransactionalMessage;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using HtmlAgilityPack;
using RestSharp;
using System.Text;

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
        [ActionParameter] UpdateNewsletterTranslationEntity payload)
    {
        return await HandleNewsletterTranslation(input, Method.Put, payload);
    }

    private async Task<NewsletterTranslationFileResponse> HandleNewsletterTranslation(
        NewsletterRequest input,
        Method method,
        UpdateNewsletterTranslationEntity? payload = null)
    {
        var endpoint = $"v1/newsletters/{input.NewsletterId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, method, Creds);

        if (payload != null)
        {
            if (payload.File != null)
            {
                var file = await fileManagementClient.DownloadAsync(payload.File);
                using var reader = new StreamReader(file);
                payload.Body = await reader.ReadToEndAsync();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(payload.Body);

                var subjectMeta = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='subject']");
                var preheaderMeta = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='preheader']");

                if (subjectMeta != null)
                {
                    payload.Subject = subjectMeta.GetAttributeValue("content", string.Empty);
                    subjectMeta.Remove();
                }

                if (preheaderMeta != null)
                {
                    payload.PreheaderText = preheaderMeta.GetAttributeValue("content", string.Empty);
                    preheaderMeta.Remove();
                }

                payload.Body = htmlDoc.DocumentNode.OuterHtml;
            }
        }

        var response = await Client.ExecuteWithErrorHandling<NewsletterTranslationResponse>(request);

        var subject = response.Content?.Subject ?? "";
        var preheader = response.Content?.PreheaderText ?? "";
        var body = response.Content?.Body ?? "";

        var html = $@"<!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <meta name='subject' content='{subject}'>
                <meta name='preheader' content='{preheader}'>
                <title>{subject}</title>
            </head>
            <body>
                {body}
            </body>
        </html>";


        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
        var fileReference = await fileManagementClient.UploadAsync(stream, "text/html", $"{response.Content?.Name} [{response.Content?.Id}].html");

        return new NewsletterTranslationFileResponse(response.Content ?? new NewsletterTranslationEntity(), fileReference);
    }

    [Action("Get a translation of a campaign message", Description = "Get a translation of a campaign message")]
    public async Task<CampaignMessageTranslationResponse> GetTranslationsForCampaign(
        [ActionParameter] CampaignTranslationRequest input)
    {
        var endpoint = $"v1/campaigns/{input.CampaignId}/actions/{input.ActionId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<CampaignMessageTranslationResponse>(request);

        return response ?? new CampaignMessageTranslationResponse();
    }

    [Action("Update a translation of a campaign message", Description = "Update a translation of a campaign message")]
    public async Task<CampaignMessageTranslationResponse> UpdateCampaignTranslation(
    [ActionParameter] CampaignTranslationRequest input,
    [ActionParameter] UpdateCampaignTranslationRequest updateRequest)
    {
        var endpoint = $"v1/campaigns/{input.CampaignId}/actions/{input.ActionId}/language/{input.Language}";

        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
                .WithJsonBody(updateRequest, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<CampaignMessageTranslationResponse>(request);

        return response;
    }
}