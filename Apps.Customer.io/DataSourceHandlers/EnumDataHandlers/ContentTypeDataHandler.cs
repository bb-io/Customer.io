using Apps.Customer.io.Constants;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;

public class ContentTypeDataHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> EnumValues => new()
    {
        { ContentTypes.TransactionalMessage, "Transactional message" },
        { ContentTypes.BroadcastMessage, "Broadcast message" },
        { ContentTypes.CampaignMessage, "Campaign message" },
        { ContentTypes.Newsletter, "Newsletter" },
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return EnumValues.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}