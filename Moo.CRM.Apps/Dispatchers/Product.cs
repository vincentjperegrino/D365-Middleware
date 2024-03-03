using KTI.Moo.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.CRM.App.Dispatchers;

public class Product : Helpers.CompanySettings
{
    private readonly Base.Domain.Dispatchers.IProduct _dispatcher;

    public Product(Base.Domain.Dispatchers.IProduct dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [FunctionName("CRM-Dispatcher-Product")]
    public void Run([QueueTrigger("%CompanyID%-dispatcher-product", Connection = "AzureQueueConnectionString")] string myQueueItem, ILogger log)
    {

        var decodedString = Helpers.Decode.Base64(myQueueItem);

        log.LogInformation($"C# Queue trigger function processed: {decodedString}");

        var company = Convert.ToInt32(Companyid);


        _dispatcher.DispatchProcess(company, decodedString, ConnectionString, log);
    }



}
