using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.Customer.io;

public class Application : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.Marketing];
        set { }
    }
    
    public string Name
    {
        get => "Customer.io";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}