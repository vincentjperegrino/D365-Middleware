namespace Moo.BC.App
{
    public class Receiver
    {
        [FunctionName("BC-Receiver")]
        public void Run([QueueTrigger("%CompanyID%-bc", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
        {
            var decodedString = KTI.Moo.BC.App.Helpers.Decode.Base64(myQueueItem).FromBrotliAsync().GetAwaiter().GetResult();

            log.LogInformation($"C# Queue trigger function processed: {decodedString}");

            var DomainJObject = JsonConvert.DeserializeObject<JObject>(decodedString);

            var company = DomainJObject["companyid"].Value<int?>();

            var domainType = DomainJObject["domainType"].Value<string>();

            DomainJObject.Remove("companyid");
            DomainJObject.Remove("domainType");

            decodedString = JsonConvert.SerializeObject(DomainJObject);

            if (company is null || company == 0)
            {
                throw new Exception("Attribute companyid missing.");
            }

            if (string.IsNullOrEmpty(domainType))
            {
                throw new Exception("Attribute domainType missing.");
            }

            OrderProcess(log, decodedString, company, domainType);
        }

        private static void OrderProcess(ILogger log, string decodedString, int? company, string domainType)
        {
            if (domainType == KTI.Moo.Base.Helpers.DomainType.order)
            {
                log.LogInformation($"Order Process");
                KTI.Moo.BC.Domain.Order orderDomain = new((int)company);
                orderDomain.upsertWithCustomer(decodedString, log).GetAwaiter().GetResult();

            }
        }
    }
}
