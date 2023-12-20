using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;

public class SendingStateDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "automatic", "Automatic" },
        { "draft", "Draft" },
        { "off", "Off" }
    };
}