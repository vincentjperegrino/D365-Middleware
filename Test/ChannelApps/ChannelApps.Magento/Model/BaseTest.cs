using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;


namespace TestChannelApps.Magento.Model;

public class BaseTest
{

    public KTI.Moo.Extensions.Magento.Service.Config config = new()
    {
        companyid = "3388",
        defaultURL = "https://nespresso.novateurshop.com/rest/default/V1",
        username = "nova-admin",
        password = "passw0rd",
        redisConnectionString = "ktimooextensiontokens.redis.cache.windows.net:6380,password=JMovvX2JQ3kKxmsHpB12tjnrfL4Thuv1eAzCaHQikCs=,ssl=True,abortConnect=False"
    };

    public string connectionstringKTI = "DefaultEndpointsProtocol=https;AccountName=ktimoostorage;AccountKey=2bobbq72sp+aqWGx4f1S0yBMplnrqnc0rfcgOb3JTdVAiyAO3OmPPS/s61umC2/OsV/8t8mGF2KJBB3B8O3mcQ==;EndpointSuffix=core.windows.net";

    public string connectionstringNCCIuat = "DefaultEndpointsProtocol=https;AccountName=ktimoostoragencciuat;AccountKey=t7YYfz7GsBd2OK2zxidEuhGfJykCZEQzdheQ754gedR1QJiyQ51mp27PmQRnpGTcafr2Tlx/L+P6+AStNaiA8Q==;EndpointSuffix=core.windows.net";

    public string CustomerQueueName = $"3388-magento-extension-customer-dispatcher";


    public string GetJSONwithInvoice()
    {

        JObject o1 = JObject.Parse(File.ReadAllText("..\\..\\..\\Model\\TestDTO.json"));

        return JsonConvert.SerializeObject(o1);
    }

    public string GetJSON_without_Invoice()
    {

        JObject o1 = JObject.Parse(File.ReadAllText("..\\..\\..\\Model\\TestDTOwithoutInvoice.json"));

        return JsonConvert.SerializeObject(o1);
    }

    public string GetJSON_CustomerDispatcher()
    {

        JObject o1 = JObject.Parse(File.ReadAllText("..\\..\\..\\Model\\Dispatchers\\CustomerQueueSampleFromCRM.json"));

        return JsonConvert.SerializeObject(o1);
    }

}
