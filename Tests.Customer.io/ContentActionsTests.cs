using Apps.Customer.io.Actions.Content;
using Apps.Customer.io.Constants;
using Apps.Customer.io.Models.Request.Content;
using Apps.Customer.io.Models.Response;
using Newtonsoft.Json;
using System.Net.Mime;
using Tests.Customer.io.Base;

namespace Tests.Customer.io;

[TestClass]
public class ContentActionsTests : TestBase
{
    [TestMethod]
    public async Task DownloadContent_ValidNewsletterId_ShouldDownloadValidHtmlFile()
    {
        var contentActions = new ContentActions(InvocationContext, FileManagementClient);
        var request = new ContentRequest
        {
            ContentId = "2",
            ContentType = ContentTypes.Newsletter,
        };
        
        var result = await contentActions.DownloadContentAsync(request);
        
        Assert.IsNotNull(result);
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }

    [TestMethod]
    public async Task DownloadContent_ValidNewsletterId_ShouldDownloadValidJsonFile()
    {
        var contentActions = new ContentActions(InvocationContext, FileManagementClient);
        var request = new ContentRequest
        {
            ContentId = "2",
            ContentType = ContentTypes.Newsletter,
            FileFormat = MediaTypeNames.Application.Json,
        };

        var result = await contentActions.DownloadContentAsync(request);

        Assert.IsNotNull(result);
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

        var json = FileManagementClient.ReadOutputAsString(result.File);
        Console.WriteLine(json);
        var parsed = JsonConvert.DeserializeObject<JsonResponseWithMetadata>(json);
        Assert.AreEqual("2", parsed.ContentId);
    }

    [TestMethod]
    public async Task UploadContent_ValidNewsletterFile_ShouldDownloadValidHtmlFile()
    {
        var contentActions = new ContentActions(InvocationContext, FileManagementClient);
        var request = new UploadContentRequest
        {
            File = new()
            {
                Name = "[newsletter] 2.html",
                ContentType = "text/html"
            },
            ContentType = ContentTypes.Newsletter,
            Language = "en-US"
        };

        var result = await contentActions.UploadContent(request);
        
        Assert.IsNotNull(result);
        Assert.IsTrue(result.ContentType.Equals(ContentTypes.Newsletter));
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }
    
    
    [TestMethod]
    public async Task DownloadContent_ValidCampaignMessageId_ShouldDownloadValidHtmlFile()
    {
        var contentActions = new ContentActions(InvocationContext, FileManagementClient);
        var request = new ContentRequest
        {
            ContentId = "1",
            ActionId = "15",
            ContentType = ContentTypes.CampaignMessage,
            Language = "en-US"
        };
        
        var result = await contentActions.DownloadContentAsync(request);
        
        Assert.IsNotNull(result);
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }

    [TestMethod]
    public async Task DownloadContent_ValidCampaignMessageId_ShouldDownloadValidJsonFile()
    {
        var contentActions = new ContentActions(InvocationContext, FileManagementClient);
        var request = new ContentRequest
        {
            ContentId = "857",
            ActionId = "40528",
            ContentType = ContentTypes.CampaignMessage,
            FileFormat = MediaTypeNames.Application.Json,
        };

        var result = await contentActions.DownloadContentAsync(request);

        Assert.IsNotNull(result);
        
        var json = FileManagementClient.ReadOutputAsString(result.File);
        Console.WriteLine(json);
        var parsed = JsonConvert.DeserializeObject<JsonResponseWithMetadata>(json);
        Assert.AreEqual("857", parsed.ContentId);
    }

    [TestMethod]
    public async Task UploadContent_ValidCampaignMessageFile_ShouldDownloadValidHtmlFile()
    {
        var contentActions = new ContentActions(InvocationContext, FileManagementClient);
        var request = new UploadContentRequest
        {
            File = new()
            {
                Name = "[campaign_message] 1.html",
                ContentType = "text/html"
            },
            ActionId = "15",
            ContentType = ContentTypes.CampaignMessage,
            Language = "en-US"
        };

        var result = await contentActions.UploadContent(request);
        
        Assert.IsNotNull(result);
        Assert.IsTrue(result.ContentType.Equals(ContentTypes.CampaignMessage));
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }

    [TestMethod]
    public async Task UploadContent_FromDownloadJson()
    {
        var contentActions = new ContentActions(InvocationContext, FileManagementClient);
        var request = new ContentRequest
        {
            ContentId = "857",
            ActionId = "40528",
            ContentType = ContentTypes.CampaignMessage,
            FileFormat = MediaTypeNames.Application.Json,
        };

        var result = await contentActions.DownloadContentAsync(request);

        var uploadRequest = new UploadContentRequest
        {
            File = result.File,
            ActionId = "40528",
            ContentType = ContentTypes.CampaignMessage,
            Language = "en-US"
        };

        var uploadResult = await contentActions.UploadContent(uploadRequest);

        Assert.IsNotNull(uploadResult);
        Assert.IsTrue(uploadResult.ContentType.Equals(ContentTypes.CampaignMessage));
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }
}