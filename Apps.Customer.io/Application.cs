using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io;

public class Application : IApplication
{
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