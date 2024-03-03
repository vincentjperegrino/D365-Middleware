
using Newtonsoft.Json;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class TokenParameters
    {
        [JsonProperty("UserName")]
        public string username { get; set; }
        [JsonProperty("Password")]
        public string password { get; set; }
    }
}
