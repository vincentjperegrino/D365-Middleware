using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Migration.NCCI.Magento;

public class Customer
{
    [Fact]
    public async Task Migration()
    {
        Domain.Modules.Customer domain = new(3389);

        var result = await domain.getall();

        var results = JsonConvert.DeserializeObject<dynamic>(result);

        var nextlink = (string)results["@odata.nextLink"];

        foreach (var item in results.value)
        {
            try
            {

                var details = new Domain.Models.Customer.Customer(item);
                await domain.ToMigrationQueue(details);

            }
            catch
            {
                continue;
            }

        }

        var notdone = true;

        while (notdone)
        {
            domain = new(3389);

            var Nextresult = await domain.getall(nextlink);

            var Nextresults = JsonConvert.DeserializeObject<dynamic>(Nextresult);

            nextlink = (string)Nextresults["@odata.nextLink"];

            if (string.IsNullOrWhiteSpace(Nextresult))
            {
                break;
            }

            foreach (var item in Nextresults.value)
            {
                try
                {
                    var details = new Domain.Models.Customer.Customer(item);
                    await domain.ToMigrationQueue(details);
                }
                catch
                {

                    continue;
                }
            }

        }

        Assert.True(true);
    }

}
