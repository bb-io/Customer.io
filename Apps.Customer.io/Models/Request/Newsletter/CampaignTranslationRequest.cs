using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Customer.io.Models.Request.Newsletter
{
    public class CampaignTranslationRequest
    {
        [Display("Campaign ID")]
        public string CampaignId { get; set; }

        [Display("Action")]
        public string ActionId {  get; set; }

        [Display("Language")]
        public string? Language { get; set; }
    }
}
