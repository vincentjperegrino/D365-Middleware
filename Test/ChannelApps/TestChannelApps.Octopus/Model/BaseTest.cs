using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;


namespace TestChannelApps.Octopus.Model;

public class BaseTest
{

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
