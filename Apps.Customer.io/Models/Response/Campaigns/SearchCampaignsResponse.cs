using Apps.Customer.io.Models.Entity;

namespace Apps.Customer.io.Models.Response.Campaigns;

public record SearchCampaignsResponse(List<CampaignEntity> Campaigns);
