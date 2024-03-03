namespace Moo.FO.App.Queue.Helpers
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

        public static string ExtractNextLink(string jsonString)
        {
            JObject json = JObject.Parse(jsonString);

            // Check if the "@odata.nextLink" property exists
            if (json.TryGetValue("@odata.nextLink", out JToken nextLinkToken))
            {
                return nextLinkToken.ToString();
            }

            // If "@odata.nextLink" is not present, return null or handle it as needed
            return null;
        }
    }
}
