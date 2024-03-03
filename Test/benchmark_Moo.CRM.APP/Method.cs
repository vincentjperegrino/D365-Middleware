using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace benchmark_Moo.CRM.APP
{
    public class Method
    {

        public string GetFromStrongTYPE(string decodedString)
        {
            var settings = new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new Helpers.JSONSerializerHelper.IgnoreResolver()
            };


            var custom = JsonConvert.DeserializeObject<StrongTypeClass>(decodedString, settings);
            string CustomerJson = JsonConvert.SerializeObject(custom, settings);
            return CustomerJson;

        }


        public string GetFromJObject(string decodedString)
        {

            var CustomerJObject = JsonConvert.DeserializeObject<JObject>(decodedString);
            CustomerJObject.Remove("companyid");

            var CustomerJson = JsonConvert.SerializeObject(CustomerJObject);


            return CustomerJson;
        }


        public string GetJSON()
        {

            JObject o1 = JObject.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "\\sample.json")));

            return JsonConvert.SerializeObject(o1);
        }



    }
}
