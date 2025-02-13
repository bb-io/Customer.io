using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;

public class SendingStateDataHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> EnumValues => new()
    {
        { "automatic", "Automatic" },
        { "draft", "Draft" },
        { "off", "Off" }
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}