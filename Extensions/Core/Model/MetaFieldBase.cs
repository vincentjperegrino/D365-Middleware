using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Core.Model
{
    public class MetaFieldBase
    {
        public long id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public string key { get; set; }

        public object value { get; set; }

        public string type { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        public string description { get; set; }

        public long owner_id { get; set; }

        public string owner_resource { get; set; }
    }
}
