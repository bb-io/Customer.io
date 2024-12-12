using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Customer.io.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Customer.io.Polling.Models
{
    public class CampaignIdentifier
    {
        [Display("Campaign ID"), DataSource(typeof(CampaignDataHandler))]
        public string CampaignId { get; set; } = string.Empty;
    }
}
