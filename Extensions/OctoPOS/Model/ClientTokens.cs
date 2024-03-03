using KTI.Moo.Extensions.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Linq;

namespace KTI.Moo.Extensions.OctoPOS.Model
{
    public class ClientTokens : ClientTokensBase
    {

        [JsonProperty("access_token")]
        public override string AccessToken { get; set; }

        //   [JsonProperty("expires_in")]
        public override DateTime AccessExpiration { get; set; }

        [JsonProperty("expires_in")]
        public int expires_in { get; set; }

        [JsonProperty("ApiAuth")]
        public override string Id { get; set; }

    }


}
