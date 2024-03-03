
using KTI.Moo.Extensions.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class ProductCategory : CategoryBase
    {

        [JsonProperty("category_id")]
        public override int CategoryId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int position { get; set; }





    }
}
