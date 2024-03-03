using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.App.Receivers
{
    public class UpdateShipmentCallback : Helpers.CompanySettings
    {

        [FunctionName("CRM-Shipment-Callback")]
        public void Run([QueueTrigger("%CompanyID%-crm-callback-shipment", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
        {
            var decodedString = Helpers.Decode.Base64(myQueueItem);

            log.LogInformation($"C# Queue trigger function processed: {decodedString}");

            var orderStatus = JsonConvert.DeserializeObject<CRM.Model.ShipmentBase>(decodedString);

            KTI.Moo.CRM.Domain.Shipment Domain = new(int.Parse(Companyid));

            Domain.update(decodedString, orderStatus.kti_shipmentid, log).GetAwaiter().GetResult(); 
        }

    }
}
