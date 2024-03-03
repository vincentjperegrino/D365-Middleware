using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.FO.App.Helper
{
    public class ODataHelper
    {

        public static string ExtractValueObject(string jsonString)
        {
            // parse the JSON string into a JObject
            JObject jsonObject = JObject.Parse(jsonString);

            // get the value property as a JToken
            JToken valueToken = jsonObject["value"];

            // convert the JToken to a string
            string valueString = valueToken.ToString();

            return valueString;
        }

    }
}
