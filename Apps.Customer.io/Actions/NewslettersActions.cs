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
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using Apps.Customer.io.Models.Response.Broadcast;
using Apps.Customer.io.Utils.Converters;

namespace Apps.Customer.io.Actions;

[ActionList]
public class NewslettersActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : CustomerIoInvocable(invocationContext)
{
    [Action("Get translation of newsletter",
        Description = "Get information about a translation of an individual newsletter")]
    public async Task<NewsletterTranslationFileResponse> GetNewsletterTranslation([ActionParameter] NewsletterRequest input)
    {
        return await HandleNewsletterTranslation(input, Method.Get);
    }

    [Action("Update translation of newsletter", Description = "Update the translation of a newsletter variant")]
    public async Task<NewsletterTranslationFileResponse> UpdateNewsletterTranslation(
        [ActionParameter] NewsletterRequest input,
        [ActionParameter] UpdateNewsletterTranslationEntity payload)
    {
        return await HandleNewsletterTranslation(input, Method.Put, payload);
    }

    [Action("Get translation of campaign message", Description = "Get a translation of a campaign message")]
    public async Task<CampaignMessageTranslationResponse> GetTranslationsForCampaign(
        [ActionParameter] CampaignTranslationRequest input)
    {
        var endpoint = $"v1/campaigns/{input.CampaignId}/actions/{input.ActionId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<CampaignMessageTranslationResponse>(request);

        return response ?? new CampaignMessageTranslationResponse();
    }

    [Action("Get translation of campaign message as HTML",
        Description = "Get a translation of a campaign message as HTML")]
    public async Task<FileResponse> GetTranslationOfCampaignMessageAsHtmlAsync(
        [ActionParameter] CampaignTranslationRequest input)
    {
        var endpoint = $"v1/campaigns/{input.CampaignId}/actions/{input.ActionId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<CampaignMessageTranslationResponse>(request);
        var htmlStream = CampaignMessageConverter.ToHtmlStream(response);
        var fileReference =
            await fileManagementClient.UploadAsync(htmlStream, "text/html", $"{response.Answer.Name}.html");
        return new()
        {
            File = fileReference
        };
    }

    [Action("Update translation of campaign message", Description = "Update a translation of a campaign message")]
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
    
    [Action("Update translation of campaign message from HTML", Description = "Update a translation of a campaign message")]
    public async Task<CampaignMessageTranslationResponse> UpdateCampaignTranslationFromHtmlAsync(
        [ActionParameter] CampaignTranslationRequest input,
        [ActionParameter] UpdateCampaignTranslationFromHtmlRequest updateRequest)
    {
        var endpoint = $"v1/campaigns/{input.CampaignId}/actions/{input.ActionId}/language/{input.Language}";
        
        await using var htmlStream = await fileManagementClient.DownloadAsync(updateRequest.File);
        var memoryStream = new MemoryStream();
        await htmlStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        var campaignMessageEntity = CampaignMessageConverter.ToCampaignMessageEntity(memoryStream);
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(new
            {
                name = campaignMessageEntity.Name,
                body = campaignMessageEntity.Body
            }, JsonConfig.Settings);

        var response = await Client.ExecuteWithErrorHandling<CampaignMessageTranslationResponse>(request);
        return response;
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

                var subjectDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='subject']");
                var preheaderDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='preheader']");

                if (subjectDiv != null)
                    payload.Subject = subjectDiv.InnerText.Trim();

                if (preheaderDiv != null)
                    payload.PreheaderText = preheaderDiv.InnerText.Trim();

                if (method == Method.Put)
                {
                    subjectDiv?.Remove();
                    preheaderDiv?.Remove();
                }

                payload.Body = htmlDoc.DocumentNode.OuterHtml;
            }
            
            request.WithJsonBody(payload, JsonConfig.Settings);
        }
        
        var response = await Client.ExecuteWithErrorHandling<NewsletterTranslationResponse>(request);
        var entity = response.Content;
        var subject = entity.Subject ?? "";
        var preheader = entity.PreheaderText ?? "";
        var body = entity.Body ?? "";
        
        var doc = new HtmlDocument();
        doc.LoadHtml(body);

        var innerBody = doc.DocumentNode.SelectSingleNode("//body");
        string finalBodyContent;

        if (innerBody != null)
        {
            finalBodyContent = innerBody.InnerHtml;
        }
        else
        {
            finalBodyContent = body;
        }

        if (method == Method.Get)
        {
            finalBodyContent = $@"<div id='subject'>{subject}</div>
                                  <div id='preheader'>{preheader}</div>
                                  {finalBodyContent}";
        }

        var html = $@"<!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>{subject}</title>
                    </head>
                    <body>
                        {finalBodyContent}
                    </body>
                    </html>";

        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
        var fileReference = await fileManagementClient.UploadAsync(stream, "text/html", $"{entity?.Name} [{entity?.Id}].html");

        return new NewsletterTranslationFileResponse(entity ?? new NewsletterTranslationEntity(), fileReference);
    }
}