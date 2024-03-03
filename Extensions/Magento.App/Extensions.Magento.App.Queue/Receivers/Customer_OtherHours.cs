using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.App.Queue.Receivers;

public class Customer_OtherHours : CompanySettings
{

    private readonly Magento.Domain.Customer _CustomerDomain;
    private readonly IDistributedCache _cache;

    public Customer_OtherHours(IDistributedCache cache)
    {
        _CustomerDomain = new(config, cache);
        _cache = cache;
    }


    [FunctionName("Magento_PollCustomer_OtherHours")]
    public void Run([TimerTrigger("0 */10 0-2,14-23 * * *")] TimerInfo otherhours, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Customer Queue trigger function processed at " + DateTimeHelper.PHTnow().ToString("yyyy MMM, dd hh:mm tt"));


            _ = int.TryParse(Environment.GetEnvironmentVariable("pastminutescoveredCustomer"), out int RecentlyAddedOrderInThePastMinutes);

            if (RecentlyAddedOrderInThePastMinutes == 0)
            {
                var defaultminutes = -5;
                RecentlyAddedOrderInThePastMinutes = defaultminutes;
            }

            _ = int.TryParse(Environment.GetEnvironmentVariable("offsetminutes"), out int addOffsetMinutes);

            if (addOffsetMinutes == 0)
            {
                var defaultOffsetminutes = -5;
                addOffsetMinutes = defaultOffsetminutes;
            }


            ///For Testing
            //RecentlyAddedOrderInThePastMinutes = -3;
            //addOffsetMinutes = 0;
            //var startDate = DateTime.UtcNow.AddHours(RecentlyAddedOrderInThePastMinutes);
            ///For Testing

            var startDate = DateTime.UtcNow.AddMinutes(RecentlyAddedOrderInThePastMinutes);
            var endDate = DateTime.UtcNow.AddMinutes(addOffsetMinutes);


            KTI.Moo.Extensions.Magento.App.Queue.Receivers.Customer.process(_CustomerDomain, startDate , endDate, log);
        }
        catch (System.Exception ex)
        {
            log.LogInformation(ex.Message);
            throw new System.Exception(ex.Message);
        }
    }



}
