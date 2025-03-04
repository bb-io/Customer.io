using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Customer.io.DataSourceHandlers;
using Apps.Customer.io.Models.Request.Newsletter;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.Customer.io.Base;

namespace Tests.Customer.io
{
    [TestClass]
    public class DataHandlerTests :TestBase
    {
        [TestMethod]
        public async Task GetCampainId_ReturnsValue()
        {
            var dataHandler = new CampaignDataHandler(InvocationContext);

            var result = await dataHandler.GetDataAsync(new DataSourceContext { SearchString = "" }, CancellationToken.None);

            foreach (var item in result)
            {
                Console.WriteLine($"{item.DisplayName} - {item.Value}");
            }
            Assert.IsNotNull(result);
            Console.WriteLine(result);
        }


        [TestMethod]
        public async Task GetActionId_ReturnsValue()
        {
            var input = new CampaignTranslationRequest { CampaignId = "1" };
            var dataHandler = new CampaignActionDataHandler(InvocationContext, input);

            var result = await dataHandler.GetDataAsync(new DataSourceContext { SearchString = "" }, CancellationToken.None);

            foreach (var item in result)
            {
                Console.WriteLine($"{item.DisplayName} - {item.Value}");
            }
            Assert.IsNotNull(result);
            Console.WriteLine(result);
        }
    }
}
