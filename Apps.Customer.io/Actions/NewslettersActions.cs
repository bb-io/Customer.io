using Apps.Customer.io.Api;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Invocables;
using Apps.Customer.io.Models.Entity;
using Apps.Customer.io.Models.Request.Newsletter;
using Apps.Customer.io.Models.Response.Newsletter;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Customer.io.Actions;

[ActionList]
public class NewslettersActions : CustomerIoInvocable
{
    public NewslettersActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }
    
    [Action("Get translation of a newsletter", Description = "Get information about a translation of an individual newsletter")]
    public async Task<NewsletterTranslationEntity> GetNewsletterTranslation(
        [ActionParameter] NewsletterRequest input)
    {
        var endpoint = $"v1/newsletters/{input.NewsletterId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<NewsletterTranslationResponse>(request);
        return response.Content;
    }    
    
    [Action("Update translation of a newsletter", Description = "Update the translation of a newsletter variant")]
    public async Task<NewsletterTranslationEntity> UpdateNewsletterTranslation(
        [ActionParameter] NewsletterRequest input,
        [ActionParameter] UpdateNewsletterTranslationRequest payload)
    {
        var endpoint = $"v1/newsletters/{input.NewsletterId}/language/{input.Language}";
        var request = new CustomerIoRequest(endpoint, Method.Put, Creds)
            .WithJsonBody(payload, JsonConfig.Settings);;

        var response = await Client.ExecuteWithErrorHandling<NewsletterTranslationResponse>(request);
        return response.Content;
    }
}