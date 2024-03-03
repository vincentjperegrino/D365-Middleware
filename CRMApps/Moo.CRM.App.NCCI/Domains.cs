
using Newtonsoft.Json.Linq;

namespace KTI.Moo.CRM.App.NCCI
{
    public class Domains
    {
        [FunctionName("CRM-Domain")]
        public void Run([QueueTrigger("%CompanyID%-crm", Connection = "AzureQueueConnectionString")]string myQueueItem, ILogger log)
        {
            var decodedString = Helpers.Decode.Base64(myQueueItem);

            log.LogInformation($"C# Queue trigger function processed: {decodedString}");

            var DomainJObject = JsonConvert.DeserializeObject<JObject>(decodedString);

            var company = DomainJObject["companyid"].Value<int?>();

            var domainType = DomainJObject["domainType"].Value<string>();

            if (company is null || company == 0)
            {
                throw new Exception("Attribute companyid missing.");
            }

            if (string.IsNullOrEmpty(domainType))
            {
                throw new Exception("Attribute domainType missing.");
            }

            CustomerProcess(log, decodedString, company, domainType);

            InvoiceProcess(log, decodedString, company, domainType);

            OrderProcess(log, decodedString, company, domainType);

        }

        private static void OrderProcess(ILogger log, string decodedString, int? company, string domainType)
        {
            if (domainType == Base.Helpers.DomainType.invoice)
            {
                Domain.Order orderDomain = new((int)company);

                orderDomain.upsert(decodedString, log).GetAwaiter().GetResult();

            }
        }

        private static void InvoiceProcess(ILogger log, string decodedString, int? company, string domainType)
        {
            if (domainType == Base.Helpers.DomainType.invoice)
            {
                Domain.Invoice invoiceDomain = new((int)company);

                invoiceDomain.upsert(decodedString, log).GetAwaiter().GetResult();

            }
        }

        private static void CustomerProcess(ILogger log, string decodedString, int? company, string domainType)
        {
            if (domainType == Base.Helpers.DomainType.customer)
            {
                Domain.Customer Customerdomain = new((int)company);

                Customerdomain.upsert(decodedString, log).GetAwaiter().GetResult();

            }
        }
    }
}
