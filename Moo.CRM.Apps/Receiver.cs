namespace KTI.Moo.CRM.App;

public class Receiver
{

    //[Singleton(Mode = SingletonMode.Listener)]
    [FunctionName("CRM-Receiver")]
    public void Run([QueueTrigger("%CompanyID%-crm", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {
        var decodedString = Helpers.Decode.Base64(myQueueItem).FromBrotliAsync().GetAwaiter().GetResult();

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

        CustomerProcess(log, decodedString, company, domainType);

        InvoiceProcess(log, decodedString, company, domainType);

        OrderProcess(log, decodedString, company, domainType);

        ProductProcess(log, decodedString, company, domainType);
    }

    private static void ProductProcess(ILogger log, string decodedString, int? company, string domainType)
    {
        if (domainType == Base.Helpers.DomainType.product)
        {
            log.LogInformation($"Product Process");

            KTI.Moo.CRM.Domain.Product productDomain = new((int)company);

            productDomain.upsert(decodedString, log).GetAwaiter().GetResult();

        }
    }

    private static void OrderProcess(ILogger log, string decodedString, int? company, string domainType)
    {
        if (domainType == Base.Helpers.DomainType.order)
        {
            log.LogInformation($"Order Process");
            KTI.Moo.CRM.Domain.Order orderDomain = new((int)company);
            orderDomain.upsertWithCustomer(decodedString, log).GetAwaiter().GetResult();

        }
    }

    private static void InvoiceProcess(ILogger log, string decodedString, int? company, string domainType)
    {
        if (domainType == Base.Helpers.DomainType.invoice)
        {
            log.LogInformation($"Invoice Process");

            var DomainJObject = JsonConvert.DeserializeObject<JObject>(decodedString);
            var InvoiceItems = DomainJObject["invoiceItem"].Values<JObject>();

            DomainJObject.Remove("invoiceItem");
            DomainJObject.Remove("domainType");

            KTI.Moo.CRM.Domain.Invoice invoiceDomain = new((int)company);
            KTI.Moo.CRM.Domain.InvoiceDetails invoiceDetailsDomain = new((int)company);


            decodedString = JsonConvert.SerializeObject(DomainJObject);

            invoiceDomain.upsert(decodedString, log).GetAwaiter().GetResult();

            var settings = new JsonSerializerSettings();

            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;


            foreach (var items in InvoiceItems)
            {
                items.Remove("domainType");

                var Itemcontent = JsonConvert.SerializeObject(items);

                invoiceDetailsDomain.upsert(Itemcontent, log).GetAwaiter().GetResult();
            }

        }
    }

    private static void CustomerProcess(ILogger log, string decodedString, int? company, string domainType)
    {
        if (domainType == Base.Helpers.DomainType.customer)
        {

            log.LogInformation($"Customer Process");

            KTI.Moo.CRM.Domain.Customer Customerdomain = new((int)company);

            var DomainJObject = JsonConvert.DeserializeObject<JObject>(decodedString);
            DomainJObject.Remove("domainType");
            
            decodedString = JsonConvert.SerializeObject(DomainJObject);

            Customerdomain.upsert(decodedString, log).GetAwaiter().GetResult();

        }
    }



}
