using Apps.Customer.io.Constants;
using Apps.Customer.io.Utils;
using Apps.Customer.io.Utils.ContentType;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.Customer.io.Base;

namespace Tests.Customer.io.Unit;

[TestClass]
public class ContentTypeResolverTests : TestBase
{
    [TestMethod]
    public async Task Resolve_NewsletterWithContentTypeInMeta_ReturnsMetaContentType()
    {
        // Arrange
        var file = new FileReference { Name = "[newsletter] 2.html" };
        string html = await DownloadHtml(file);

        // Act
        string contentType = ContentTypeResolver.Resolve(html, null);

        // Assert
        Assert.AreEqual(ContentTypes.Newsletter, contentType);
    }
    
    [TestMethod]
    public async Task Resolve_DesignStudioEmailWithContentTypeInMeta_ReturnsMetaContentType()
    {
        // Arrange
        var file = new FileReference { Name = "[design_studio_email] 1.html" };
        string html = await DownloadHtml(file);

        // Act
        string contentType = ContentTypeResolver.Resolve(html, null);

        // Assert
        Assert.AreEqual(ContentTypes.DesignStudioEmail, contentType);
    }
    
    [TestMethod]
    public async Task Resolve_DesignStudioEmailWithContentTypeInMetaAndInput_ReturnsMetaContentType()
    {
        // Arrange
        var file = new FileReference { Name = "[design_studio_email] 1.html" };
        string html = await DownloadHtml(file);
        string fallbackContentType = ContentTypes.Newsletter;

        // Act
        string contentType = ContentTypeResolver.Resolve(html, fallbackContentType);

        // Assert
        Assert.AreEqual(ContentTypes.DesignStudioEmail, contentType);
    }
    
    [TestMethod]
    public async Task Resolve_WithContentTypeInInputOnly_ReturnsInputContentType()
    {
        // Arrange
        var file = new FileReference { Name = "Slack Message 1.html" };
        string html = await DownloadHtml(file);
        string fallbackContentType = ContentTypes.TransactionalMessage;

        // Act
        string contentType = ContentTypeResolver.Resolve(html, fallbackContentType);

        // Assert
        Assert.AreEqual(fallbackContentType, contentType);
    }
    
    [TestMethod]
    public async Task Resolve_WithoutContentTypeAnywhere_ThrowsException()
    {
        // Arrange
        var file = new FileReference { Name = "Slack Message 1.html" };
        string html = await DownloadHtml(file);

        // Act
        Assert.ThrowsException<PluginMisconfigurationException>(() => ContentTypeResolver.Resolve(html, null));
    }

    private async Task<string> DownloadHtml(FileReference file)
    {
        await using var stream = await FileManagementClient.DownloadAsync(file);
        return await FileTransformer.ToHtmlString(stream, file);
    }
}