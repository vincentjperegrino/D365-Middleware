
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KTI.Moo.Extensions.Magento.Model
{
    public class Category: ProductCategory
    {
        [JsonProperty("id")]
        public override int CategoryId { get; set; }

        public string[] available_sort_by { get; set; }

        public string children { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool include_in_menu { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool is_active { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        [JsonProperty("name")]
        public override string Name { get; set; }

        public int level { get; set; }

        public int parent_id { get; set; }

        public string path { get; set; }

        public int product_count { get; set; }

        public List<Attribute> custom_attributes { get; set; }

        public ProductExtensionAttributes extension_attributes { get; set; }

        public List<Category> children_data { get; set; }


    }
}
