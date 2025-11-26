using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System.Net.Mime;

namespace Apps.Customer.io.DataSourceHandlers.EnumDataHandlers;
public class DownloadFileFormatHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem>
        {
            new DataSourceItem(MediaTypeNames.Text.Html, "Interoperable HTML (Default)"),
            new DataSourceItem(MediaTypeNames.Application.Json, "JSON")
        };
    }
}
