using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Configuration;

namespace Tests.Customer.io.Base;

public class TestBase
{
    protected TestBase()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Creds = config.GetSection("ConnectionDefinition").GetChildren()
            .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value!)).ToList();
        var folderLocation = config.GetSection("TestFolder").Value!;

        InvocationContext = new InvocationContext
        {
            AuthenticationCredentialsProviders = Creds,
        };

        FileManagementClient = new FileManager(folderLocation);
    }

    protected IEnumerable<AuthenticationCredentialsProvider> Creds { get; set; }

    protected InvocationContext InvocationContext { get; set; }

    protected FileManager FileManagementClient { get; set; }

    protected static void PrintDataHandlerResult(IEnumerable<DataSourceItem> items)
    {
        Console.WriteLine($"Count: {items.Count()}");

        foreach (var item in items)
            Console.WriteLine($"{item.Value} - {item.DisplayName}");
    }
}