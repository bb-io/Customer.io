using Apps.Customer.io.Actions;
using Apps.Customer.io.Models.Request.Newsletter;
using Newtonsoft.Json;
using Tests.Customer.io.Base;

namespace Tests.Customer.io;

[TestClass]
public class NewslettersActionsTests : TestBase
{
    [TestMethod]
    public async Task GetTranslationOfCampaignMessageAsHtml_ValidId_ShouldReturnHtmlFile()
    {
        var broadcastActions = new NewslettersActions(InvocationContext, FileManagementClient);
        var request = new CampaignTranslationRequest
        {
            CampaignId = "1",
            ActionId = "15",
            Language = "en-US"
        };

        var result = await broadcastActions.GetTranslationOfCampaignMessageAsHtmlAsync(request);
        
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.File.Name));
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    } 
        
    [TestMethod]
    public async Task UpdateCampaignTranslationFromHtml_ValidFile_ShouldNotFail()
    {
        var broadcastActions = new NewslettersActions(InvocationContext, FileManagementClient);
        var request = new CampaignTranslationRequest
        {
            CampaignId = "1",
            ActionId = "15",
            Language = "en-US"
        };
        var fileRequest = new UpdateCampaignTranslationFromHtmlRequest
        {
            File = new()
            {
                Name = "Slack Message 1.html"
            }
        };
        
        var result = await broadcastActions.UpdateCampaignTranslationFromHtmlAsync(request, fileRequest);
        
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.Answer.Name));
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }
}