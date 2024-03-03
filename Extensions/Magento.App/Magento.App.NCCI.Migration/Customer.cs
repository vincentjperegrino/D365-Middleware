using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.Magento.App.NCCI.Migration;

public class Customer
{
    [FunctionName("Customer")]
    public void Run([QueueTrigger("3389-magento-migration-customer1", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            process(myQueueItem, log);
        }
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }

    public bool process(string myQueueItem, ILogger log)
    {
        var config = Helper.ConfigHelper.Get();

        Magento.Domain.Customer Domain = new(config);

        var Model = JsonConvert.DeserializeObject<Magento.Model.Customer>(myQueueItem);

        if (string.IsNullOrWhiteSpace(Model.firstname))
        {
            string[] lastnamesWords = Model.lastname.Split(' ');
            Model.firstname = lastnamesWords[0];

            Model.address.Select(addr =>
            {
                addr.first_name = lastnamesWords[0];
                if (string.IsNullOrWhiteSpace(addr.address_city))
                {
                    addr.address_city = "city";
                }
                if (string.IsNullOrWhiteSpace(addr.address_postalcode))
                {
                    addr.address_postalcode = "0000";
                }

                if (string.IsNullOrWhiteSpace(addr.telephone))
                {
                    addr.Telephone = new()
                    {
                        new()
                        {
                            primary = true,
                            telephone = "09000000000"
                        }

                    };
                }

                if (string.IsNullOrWhiteSpace(addr.street[0]))
                {
                    addr.address_line1 = "street";
                }

                return addr;

            }).ToList();

            if (string.IsNullOrWhiteSpace(Model.email))
            {
                Model.email = $"{lastnamesWords[0]}@email.com";
            }
        }


        Domain.Upsert(Model);

        return true;
    }

}
