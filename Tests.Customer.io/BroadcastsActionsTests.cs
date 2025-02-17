using Apps.Customer.io.Actions;
using Apps.Customer.io.Models.Request;
using Apps.Customer.io.Models.Request.Broadcast;
using Newtonsoft.Json;
using Tests.Customer.io.Base;

namespace Tests.Customer.io;

[TestClass]
public class BroadcastsActionsTests : TestBase
{
    [TestMethod]
    public async Task GetBroadcastTranslation_ValidId_ShouldReturnBroadcastActionObject()
    {
        var broadcastActions = new BroadcastsActions(InvocationContext, FileManagementClient);
        var request = new BroadcastActionRequest
        {
            BroadcastId = "2",
            ActionId = "2",
            Language = "en-US"
        };

        var result = await broadcastActions.GetBroadcastTranslation(request);
        
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.Id));
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }
    
    [TestMethod]
    public async Task GetBroadcastMessageAsHtml_ValidId_ShouldReturnHtmlFile()
    {
        var broadcastActions = new BroadcastsActions(InvocationContext, FileManagementClient);
        var request = new BroadcastActionRequest
        {
            BroadcastId = "2",
            ActionId = "2",
            Language = "en-US"
        };

        var result = await broadcastActions.GetBroadcastMessageAsHtmlAsync(request);
        
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.File.Name));
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }
    
    [TestMethod]
    public async Task UpdateBroadcastActionFromHtml_ValidHtmlFile_ShouldReturnBroadcastActionObject()
    {
        var broadcastActions = new BroadcastsActions(InvocationContext, FileManagementClient);
        var broadcastActionRequest = new BroadcastActionRequest
        {
            BroadcastId = "2",
            ActionId = "2",
            Language = "en-US"
        };
        var updateRequest = new FileRequest
        {
            File = new()
            {
                Name = "9.html"
            }
        };

        var result = await broadcastActions.UpdateBroadcastActionFromHtmlAsync(broadcastActionRequest, updateRequest);
        
        Assert.IsNotNull(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.Id));
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }
}